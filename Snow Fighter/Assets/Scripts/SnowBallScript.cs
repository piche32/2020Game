using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBallScript : MonoBehaviour
{
    [SerializeField] float destroyTime = 10.0f;
    float time;
    string shooterName;

    [SerializeField] float damage = 10.0f;
    private void OnEnable()
    {
        time = 0.0f;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().velocity = Vector3.zero;

        if (shooterName != null)
        {
            GameObject shooter = GameObject.FindGameObjectWithTag(shooterName);
            if (shooter != null) 
                Physics.IgnoreCollision(shooter.GetComponent<Collider>(), GetComponentInChildren<Collider>(), false); 
        }
        shooterName = null;
        GetComponentInChildren<SphereCollider>().enabled = false;
    }

    /*private void Start()
    {
        //shooterName = null;
        //time = 0.0f;
    }*/

    

    public void Initialize(string tag, float power, Vector3 pos, Quaternion rot)
    {
        shooterName = tag;
        time = 0.0f;
        transform.position = pos;
        transform.rotation = rot;
        GetComponent<Rigidbody>().useGravity =true;

        GetComponent<Rigidbody>().AddForce(transform.forward * power);
        Physics.IgnoreCollision(GameObject.FindGameObjectWithTag(tag).GetComponent<Collider>(), GetComponentInChildren<Collider>());
        Physics.IgnoreLayerCollision(GameObject.FindGameObjectWithTag(tag).layer, this.gameObject.layer);
        Debug.Log("tag: "+ tag+ Physics.GetIgnoreCollision(GameObject.FindGameObjectWithTag(tag).GetComponent<Collider>(), GetComponentInChildren<Collider>()));
        GetComponentInChildren<SphereCollider>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (time > destroyTime)
            SnowBallPoolingScript.Instance.ReturnObject(this);
    }



    private void OnCollisionEnter(Collision collision)
    {
        if (shooterName == "Enemy" && collision.transform.tag == "Player")
        {
            SnowParticlePooling particles = GameObject.Find("SnowParticles").GetComponent<SnowParticlePooling>();
            GameObject destroyedEffect = particles.GetObject();
            destroyedEffect.transform.position = transform.position;
            destroyedEffect.transform.LookAt(destroyedEffect.transform.position - Vector3.Normalize(transform.forward));

            PlayerScript player = collision.transform.GetComponent<PlayerScript>();
            player.Hp -= damage;
            player.checkHp();
            GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>().SetPlayerHPSlider(player.Hp);
            //Debug.Log(player.Hp);

        }
        if (shooterName == "Player" && collision.transform.tag == "Enemy")
        {
            SnowParticlePooling particles = GameObject.Find("SnowParticles").GetComponent<SnowParticlePooling>();
            GameObject destroyedEffect = particles.GetObject();
            destroyedEffect.transform.position = transform.position;
            destroyedEffect.transform.LookAt(destroyedEffect.transform.position - Vector3.Normalize(transform.forward));

            EnemyAIScript enemy = collision.transform.GetComponent<EnemyAIScript>();
            enemy.Hp -= damage;
            //Debug.Log(enemy.GetComponent<EnemyAIScript>().Hp);
            enemy.checkHp();
        }

        if (collision.transform.tag != "Player" || collision.transform.tag != "Enemy")
        {
            SnowParticlePooling particles = GameObject.Find("SnowParticles").GetComponent<SnowParticlePooling>();
            GameObject destroyedEffect = particles.GetObject();
            destroyedEffect.transform.position = transform.position;
            destroyedEffect.transform.LookAt(destroyedEffect.transform.position - Vector3.Normalize(transform.forward));

            SnowBallPoolingScript.Instance.ReturnObject(this);
        }
    }

}
