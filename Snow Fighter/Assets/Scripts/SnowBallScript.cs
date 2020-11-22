using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBallScript : MonoBehaviour
{
    [SerializeField] float destroyTime = 10.0f;
    float time;
    string shooterName;

    [SerializeField] float damage = 10.0f;

    //포물선 운동
    bool isMoving;
    Transform target;
    Vector3 destPos;
    Vector3 startPos;

    float vx; //x축 속도
    float vy;//y축 속도
    float vz;
    float g;//중력가속도
    float dat;//도착점 도달 시간
    float mh;//도착점 높이
    float dh;//진행 시간
    float my = 4.0f;//최고점 높이
    float mht = .6f; //최고점 도달 시간

    void PreCalcultate()
    {
        if(target != null)
            destPos = target.position;
        else
        {
            destPos = transform.position + transform.forward * 15.0f;
        }
        startPos = transform.position;
        destPos.y = destPos.y + 0.5f;
        dh = destPos.y - startPos.y;
        mh = my - startPos.y;

        g = 2 * mh / (mht * mht);

        vy = Mathf.Sqrt(2 * g * mh);

        float a = g;
        float b = -2 * vy;
        float c = 2 * dh;

        dat = (-b +
            Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a);
        vx = -(startPos.x - destPos.x) / dat;
        vz = -(startPos.z - destPos.z) / dat;

    }
    
    Vector3 Move()
    {
        if (time > dat) {

            return this.transform.position;
        }

        float x = startPos.x + vx * time;
        float y = startPos.y + vy * time - 0.5f * g * time * time;
        float z = startPos.z + vz * time;
        return new Vector3(x, y, z);
    }

    private void OnEnable()
    {

        time = 0.0f;

        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().velocity = Vector3.zero;

        if (shooterName != null && shooterName != "")
        {
            GameObject shooter = GameObject.FindGameObjectWithTag(shooterName);
            if (shooter != null) 
                Physics.IgnoreCollision(shooter.GetComponent<Collider>(), GetComponentInChildren<Collider>(), false); 
        }
        shooterName = null;
        GetComponentInChildren<Collider>().enabled = false;

        //포물선 운동 초기화
        isMoving = false;
        target = null;

        destPos = this.transform.position;
        startPos = this.transform.position;

         vx =0.0f; //x축 속도
         vy = 0.0f;//y축 속도
         vz = 0.0f;
         g = 0.0f;//중력가속도
         dat = 0.0f;//도착점 도달 시간
         mh = 0.0f;//도착점 높이
         dh = 0.0f;//진행 시간
    }

    public void Initialize(string tag, float power, Vector3 pos, Quaternion rot, Transform target = null)
    {
        shooterName = tag;
       // time = 0.0f;
        isMoving = true;
        transform.position = pos;
        transform.rotation = rot;
        GetComponent<Rigidbody>().useGravity =true;

        if (target != null)
        {
            this.target = target;
            Debug.Log(target.name);
            //startPos = transform.position;
        }
        /*else
        {
            destPos = transform.position + transform.forward * 3.0f;
           // startPos = transform.position;
        }*/

        PreCalcultate();
        Physics.IgnoreCollision(GameObject.FindGameObjectWithTag(tag).GetComponent<Collider>(), GetComponentInChildren<Collider>());
        Physics.IgnoreLayerCollision(GameObject.FindGameObjectWithTag(tag).layer, this.gameObject.layer);
        GetComponentInChildren<Collider>().enabled = true;


        /*GetComponent<Rigidbody>().AddForce(transform.forward * power);
        Physics.IgnoreCollision(GameObject.FindGameObjectWithTag(tag).GetComponent<Collider>(), GetComponentInChildren<Collider>());
        Physics.IgnoreLayerCollision(GameObject.FindGameObjectWithTag(tag).layer, this.gameObject.layer);
        Debug.Log("tag: "+ tag+ Physics.GetIgnoreCollision(GameObject.FindGameObjectWithTag(tag).GetComponent<Collider>(), GetComponentInChildren<Collider>()));
        GetComponentInChildren<SphereCollider>().enabled = true;*/
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving == false) {
            SnowBallPoolingScript.Instance.ReturnObject(this);

            return;
        }
        time += Time.deltaTime;
        transform.position = Move();
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
            Debug.Log(enemy.GetComponent<EnemyAIScript>().Hp);
            enemy.checkHp();
            GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>().SetEnemyHPSlider(enemy.transform, enemy.Hp);
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
