using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBallScript : MonoBehaviour
{

    [SerializeField] float destroyTime = 10.0f;
    float time;
    string shooterName;

    [SerializeField] float damage = 10.0f;

    private void Start()
    {
        shooterName = null;
        time = 0.0f;
    }

    private void OnEnable()
    {
        time = 0.0f;

        GetComponent<Rigidbody>().velocity = Vector3.zero;
        if(shooterName != null)
        Physics.IgnoreCollision(GameObject.FindGameObjectWithTag(shooterName).GetComponent<Collider>(), GetComponentInChildren<Collider>());
        shooterName = null;
    }

    public void Initialize(string tag, float power, Vector3 pos, Quaternion rot)
    {
        shooterName = tag;
        time = 0.0f;
        transform.position = pos;
        transform.rotation = rot;
        
        GetComponent<Rigidbody>().AddForce(transform.forward * power);
        Physics.IgnoreCollision(GameObject.FindGameObjectWithTag(tag).GetComponent<Collider>(), GetComponentInChildren<Collider>());
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
            PlayerScript player = collision.transform.GetComponent<PlayerScript>();
            player.Hp -= damage;
            player.checkHp();
            UIManager.Instance.SetPlayerHPSlider(player.Hp);
            //Debug.Log(player.Hp);

        }
        if(shooterName == "Player" && collision.transform.tag == "Enemy")
        {
            EnemyAIScript enemy = collision.transform.GetComponent<EnemyAIScript>();
            enemy.Hp -= damage;
            //Debug.Log(enemy.GetComponent<EnemyAIScript>().Hp);
            enemy.checkHp();
        }
        SnowBallPoolingScript.Instance.ReturnObject(this);

    }

}
