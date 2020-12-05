using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBallScript : MonoBehaviour
{
    [SerializeField] float destroyTime = 10.0f;
    float time;
    Transform shooter;

    [SerializeField] float damage = 10.0f;

    //포물선 운동
  //  bool isMoving;
    Transform target;
    Vector3 destPos;
    Vector3 startPos;

    float initialAngle; //나중에 각도도 바꿔보기

    //float vx; //x축 속도
    //float vy;//y축 속도
    //float vz;
    //float g;//중력가속도
    //float dat;//도착점 도달 시간
    //float mh;//최고점-시작점(높이)
    //float dh;//도착점 높이
    //float my = 4.0f;//최고점 높이
    //float mht = .6f; //최고점 도달 시간

   

    //void PreCalcultate()
    //{
    //    if(target != null)
    //        destPos = target.position;
    //    else
    //    {
    //        destPos = transform.forward * 7.0f;
    //    }
    //    startPos = transform.position;
    //    destPos.y = destPos.y + 0.5f;
    //    dh = destPos.y - startPos.y;
    //    mh = my - startPos.y;

    //    g = 2 * mh / (mht * mht);

    //    vy = Mathf.Sqrt(2 * g * mh);

    //    float a = g;
    //    float b = -2 * vy;
    //    float c = 2 * dh;

    //    dat = (-b + Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a);
    //    vx = -(startPos.x - destPos.x) / dat;
    //    vz = -(startPos.z - destPos.z) / dat;

    //}
    
    //Vector3 Move()
    //{
    //    if (time > dat) {

    //        return this.transform.position;
    //    }

    //    float x = startPos.x + vx * time;
    //    float y = startPos.y + vy * time - 0.5f * g * time * time;
    //    float z = startPos.z + vz * time;
    //    return new Vector3(x, y, z);
    //}

    private void OnEnable()
    {

        initialAngle = 45f;

        time = 0.0f;


        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().velocity = Vector3.zero;

        GetComponentInChildren<Collider>().enabled = false;
        if (shooter != null )
        { 
      //      Physics.IgnoreCollision(GameObject.FindGameObjectWithTag(shooter.tag).GetComponent<Collider>(), GetComponentInChildren<Collider>(), false);

        }
        shooter = null;

        this.gameObject.layer = LayerMask.NameToLayer("Default");
        //포물선 운동 초기화
        //isMoving = false;
        target = null;

        destPos = this.transform.position;
        startPos = this.transform.position;

         //vx =0.0f; //x축 속도
         //vy = 0.0f;//y축 속도
         //vz = 0.0f;
         //g = 0.0f;//중력가속도
         //dat = 0.0f;//도착점 도달 시간
         //mh = 0.0f;//도착점 높이
         //dh = 0.0f;//진행 시간
    }

    public void Initialize( float power, Vector3 pos, Quaternion rot, Transform shooter, Transform target = null)
    {
        this.shooter = shooter;
        if(shooter.tag == "Player")
        {
            this.gameObject.layer = LayerMask.NameToLayer("PlayerSnowBall");
        }
        else if(shooter.tag == "Enemy")
        {
            this.gameObject.layer = LayerMask.NameToLayer("EnemySnowBall");
        }
        else
        {
            Debug.Log("Error with shooterName");
        }
       // time = 0.0f;
       // isMoving = true;
        transform.position = pos;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        GetComponent<Rigidbody>().useGravity =true;

        if (target != null)
        {
            this.target = target;
            destPos = target.position + Vector3.up;
            Debug.Log(target.name);
            //startPos = transform.position;
        }
        else
        {
            destPos = transform.position + shooter.forward * 7;
            // startPos = transform.position;
        }

  //      Physics.IgnoreCollision(GameObject.FindGameObjectWithTag(shooter.tag).GetComponent<Collider>(), GetComponentInChildren<Collider>());
        GetComponentInChildren<Collider>().enabled = true;

        Fire();

        //PreCalcultate();

        /*GetComponent<Rigidbody>().AddForce(transform.forward * power);
        Physics.IgnoreCollision(GameObject.FindGameObjectWithTag(tag).GetComponent<Collider>(), GetComponentInChildren<Collider>());
        Physics.IgnoreLayerCollision(GameObject.FindGameObjectWithTag(tag).layer, this.gameObject.layer);
        Debug.Log("tag: "+ tag+ Physics.GetIgnoreCollision(GameObject.FindGameObjectWithTag(tag).GetComponent<Collider>(), GetComponentInChildren<Collider>()));
        GetComponentInChildren<SphereCollider>().enabled = true;*/
    }

    // Update is called once per frame
    void Update()
    {
        //if (isMoving == false) {
        //    return;
        //}
        time += Time.deltaTime;

        // transform.position = Move();
        if (time > destroyTime)
            SnowBallPoolingScript.Instance.ReturnObject(this);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag != null || other.tag != "")

        {
            if (shooter.tag == other.tag) return;
            if (other.transform.parent != null)if(shooter.tag == other.transform.parent.tag) return;
            if (other.name == "FollowColl" || other.name == "AttackColl") return;
            if (other.name == "Sight Camera") return;
            if (shooter.tag == "Enemy" && other.tag == "Player")
            {
                //SnowParticlePooling particles = GameObject.Find("SnowParticles").GetComponent<SnowParticlePooling>();
                //GameObject destroyedEffect = particles.GetObject();
                //destroyedEffect.transform.position = transform.position;
                //destroyedEffect.transform.LookAt(destroyedEffect.transform.position - Vector3.Normalize(transform.forward));

                PlayerScript player = other.transform.GetComponent<PlayerScript>();
                player.Hp -= damage;
                player.checkHp();
                GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>().SetPlayerHPSlider(player.Hp);
                //Debug.Log(player.Hp);

            }
            if (shooter.tag == "Player" && other.tag == "Enemy")
            {
                //SnowParticlePooling particles = GameObject.Find("SnowParticles").GetComponent<SnowParticlePooling>();
                //GameObject destroyedEffect = particles.GetObject();
                //destroyedEffect.transform.position = transform.position;
                //destroyedEffect.transform.LookAt(destroyedEffect.transform.position - Vector3.Normalize(transform.forward));

                EnemyAIScript enemy = other.transform.GetComponent<EnemyAIScript>();
                enemy.Hp -= damage;
                Debug.Log(enemy.GetComponent<EnemyAIScript>().Hp);
                enemy.checkHp();
                GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>().SetEnemyHPSlider(enemy.transform, enemy.Hp);
            }
        }

            SnowParticlePooling particles = GameObject.Find("SnowParticles").GetComponent<SnowParticlePooling>();
            GameObject destroyedEffect = particles.GetObject();
            destroyedEffect.transform.position = transform.position;
        //destroyedEffect.transform.LookAt(destroyedEffect.transform.position - Vector3.Normalize(transform.forward));
        Debug.Log("trigger point: "+other.ClosestPointOnBounds(transform.position)+" transform.position: "+transform.position);

        destroyedEffect.transform.LookAt(Vector3.Normalize(other.ClosestPointOnBounds(transform.position) - transform.position) + transform.position);


            SnowBallPoolingScript.Instance.ReturnObject(this);
      

    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (shooterName == collision.transform.tag) return;
    //    if (shooterName == "Enemy" && collision.transform.tag == "Player")
    //    {
    //        SnowParticlePooling particles = GameObject.Find("SnowParticles").GetComponent<SnowParticlePooling>();
    //        GameObject destroyedEffect = particles.GetObject();
    //        destroyedEffect.transform.position = transform.position;
    //        destroyedEffect.transform.LookAt(destroyedEffect.transform.position - Vector3.Normalize(transform.forward));

    //        PlayerScript player = collision.transform.GetComponent<PlayerScript>();
    //        player.Hp -= damage;
    //        player.checkHp();
    //        GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>().SetPlayerHPSlider(player.Hp);
    //        //Debug.Log(player.Hp);

    //    }
    //    if (shooterName == "Player" && collision.transform.tag == "Enemy")
    //    {
    //        SnowParticlePooling particles = GameObject.Find("SnowParticles").GetComponent<SnowParticlePooling>();
    //        GameObject destroyedEffect = particles.GetObject();
    //        destroyedEffect.transform.position = transform.position;
    //        destroyedEffect.transform.LookAt(destroyedEffect.transform.position - Vector3.Normalize(transform.forward));

    //        EnemyAIScript enemy = collision.transform.GetComponent<EnemyAIScript>();
    //        enemy.Hp -= damage;
    //        Debug.Log(enemy.GetComponent<EnemyAIScript>().Hp);
    //        enemy.checkHp();
    //        GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>().SetEnemyHPSlider(enemy.transform, enemy.Hp);
    //    }

    //    if (collision.transform.tag != "Player" || collision.transform.tag != "Enemy")
    //    {
    //        SnowParticlePooling particles = GameObject.Find("SnowParticles").GetComponent<SnowParticlePooling>();
    //        GameObject destroyedEffect = particles.GetObject();
    //        destroyedEffect.transform.position = transform.position;
    //        destroyedEffect.transform.LookAt(destroyedEffect.transform.position - Vector3.Normalize(transform.forward));

    //        SnowBallPoolingScript.Instance.ReturnObject(this);
    //    }
    //}

    Vector3 GetVelocity() //Trajectory를 위한 속도 구하기
    {
        float gravitiy = Physics.gravity.magnitude;
        float angle = initialAngle * Mathf.Deg2Rad;

        Vector3 currentPos = transform.position;

        Vector3 planarTarget = new Vector3(destPos.x, 0, destPos.z);
        Vector3 planarPosition = new Vector3(currentPos.x, 0, currentPos.z);

        float distance = Vector3.Distance(planarTarget, planarPosition);
        float yOffset = currentPos.y - destPos.y;

        float initialVelocity = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * gravitiy * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));
        Vector3 velocity = new Vector3(0f, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));

        float angleBetweenObjects = Vector3.Angle(Vector3.forward, planarTarget - planarPosition) * (destPos.x > currentPos.x ? 1 : -1);
        Vector3 finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;

        return finalVelocity;
    }

    void SetVelocity(Vector3 velocity)
    {
        GetComponent<Rigidbody>().velocity = (velocity);
    }

    void Fire() {
        Vector3 velocity = GetVelocity();
        SetVelocity(velocity);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position,destPos);
    }
}
