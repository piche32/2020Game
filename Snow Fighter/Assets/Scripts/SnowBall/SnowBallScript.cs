using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SnowBallScript : MonoBehaviour
{
    //원래 짜두었던 코드
    //[SerializeField] float destroyTime = 10.0f;
    //float time;
    //Transform shooter;

    //[SerializeField] float damage = 10.0f;

    ////포물선 운동
    ////  bool isMoving;
    //Transform target;
    //Vector3 destPos;
    //Vector3 startPos;

    //bool isFired;
    //public bool IsFired { get { return isFired; } set { isFired = value; } }

    //float initialAngle; //나중에 각도도 바꿔보기

    //Rigidbody rb;
    //private void OnEnable()
    //{

    //    initialAngle = 20f;

    //    time = 0.0f;
    //    isFired = false;

    //    rb = GetComponent<Rigidbody>();
    //    rb.useGravity = false;
    //    rb.velocity = Vector3.zero;
    //    rb.isKinematic = true;

    //    GetComponentInChildren<Collider>().enabled = false;

    //    shooter = null;

    //    this.gameObject.layer = LayerMask.NameToLayer("Default");
    //    //포물선 운동 초기화
    //    //isMoving = false;
    //    target = null;

    //    destPos = this.transform.position;
    //    startPos = this.transform.position;

    //}

    //public void Initialize(float power, Vector3 pos, Quaternion rot, Transform shooter, Transform target = null)
    //{
    //    rb.isKinematic = false;
    //    this.shooter = shooter;
    //    if (shooter.tag == "Player")
    //    {
    //        this.gameObject.layer = LayerMask.NameToLayer("PlayerSnowBall");
    //    }
    //    else if (shooter.tag == "Enemy")
    //    {
    //        this.gameObject.layer = LayerMask.NameToLayer("EnemySnowBall");
    //    }
    //    else
    //    {
    //        Debug.Log("Error with shooterName");
    //    }
    //    // time = 0.0f;
    //    // isMoving = true;
    //    transform.position = pos;
    //    transform.rotation = Quaternion.Euler(Vector3.zero);

    //    rb.useGravity = true;

    //    if (target != null)
    //    {
    //        this.target = target;
    //        destPos = target.position + Vector3.up;
    //        Debug.Log(target.name);
    //        //startPos = transform.position;
    //    }
    //    else
    //    {
    //        destPos = transform.position + shooter.forward * 10;
    //        // startPos = transform.position;
    //    }

    //    //      Physics.IgnoreCollision(GameObject.FindGameObjectWithTag(shooter.tag).GetComponent<Collider>(), GetComponentInChildren<Collider>());
    //    GetComponentInChildren<Collider>().enabled = true;

    //    Fire();

    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    time += Time.deltaTime;

    //    // transform.position = Move();
    //    /*if (time > destroyTime && isFired) 
    //        SnowBallPoolingScript.Instance.ReturnObject(this);*/
    //}

    //private void OnTriggerEnter(Collider other)
    //{

    //    if (other.tag != null || other.tag != "")
    //    {
    //        if (shooter.tag == other.tag) return;
    //        if (other.transform.parent != null) if (shooter.tag == other.transform.parent.tag) return;
    //        if (other.name == "FollowColl" || other.name == "AttackColl") return;
    //        if (other.name == "Sight Camera") return;
    //        if (shooter.tag == "Enemy" && other.tag == "Player")
    //        {
    //            PlayerScript player = other.transform.GetComponent<PlayerScript>();
    //            player.Hp -= damage;
    //            player.checkHp();
    //            GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>().SetPlayerHPSlider(player.Hp);
    //            //Debug.Log(player.Hp);

    //        }
    //        if (shooter.tag == "Player" && other.tag == "Enemy")
    //        {
    //            EnemyAIScript enemy = other.transform.GetComponent<EnemyAIScript>();
    //            enemy.Hit(damage);
    //        }
    //    }

    //    SnowParticlePooling particles = GameObject.Find("SnowParticles").GetComponent<SnowParticlePooling>();

    //    GameObject destroyedEffect = particles.GetObject();
    //    destroyedEffect.transform.position = transform.position;
    //    //destroyedEffect.transform.LookAt(destroyedEffect.transform.position - Vector3.Normalize(transform.forward));
    //    Debug.Log("trigger point: " + other.ClosestPointOnBounds(transform.position) + " transform.position: " + transform.position);

    //    destroyedEffect.transform.LookAt(Vector3.Normalize(other.ClosestPointOnBounds(transform.position) - transform.position) + transform.position);

    //    SnowBallPoolingScript.Instance.ReturnObject(this);
    //}

    //Vector3 GetVelocity() //Trajectory를 위한 속도 구하기
    //{
    //    float gravitiy = Physics.gravity.magnitude;
    //    float angle = initialAngle * Mathf.Deg2Rad;

    //    Vector3 currentPos = transform.position;

    //    Vector3 planarTarget = new Vector3(destPos.x, 0, destPos.z);
    //    Vector3 planarPosition = new Vector3(currentPos.x, 0, currentPos.z);

    //    float distance = Vector3.Distance(planarTarget, planarPosition);
    //    float yOffset = currentPos.y - destPos.y;

    //    float initialVelocity = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * gravitiy * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));
    //    Vector3 velocity = new Vector3(0f, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));

    //    float angleBetweenObjects = Vector3.Angle(Vector3.forward, planarTarget - planarPosition) * (destPos.x > currentPos.x ? 1 : -1);
    //    Vector3 finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;

    //    if (shooter.tag == "Player")
    //    {
    //        // Player가 보는 방향으로 속도를 더해줄 수 있게, transform.forward와 velocity를 내적한다.
    //        Vector3 innerProduct = shooter.transform.forward * Vector3.Dot(shooter.GetComponent<Rigidbody>().velocity, shooter.transform.forward);
    //        finalVelocity += innerProduct;
    //    }

    //    else
    //        finalVelocity += shooter.GetComponent<NavMeshAgent>().velocity;
    //    return finalVelocity;
    //}

    //void SetVelocity(Vector3 velocity)
    //{
    //    rb.velocity = velocity;
    //}

    //void Fire()
    //{
    //    Vector3 velocity = GetVelocity();
    //    SetVelocity(velocity);
    //}

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawLine(transform.position, destPos);
    //}

    float time;
    Transform shooter;

    [SerializeField] float damage = 10.0f;

    bool isFired;
    public bool IsFired { get { return isFired; } set { isFired = value; } }
    private float power = 10.0f;

    Rigidbody rb;

    Vector3 interpolationSight; //시야각 보간
    private void OnEnable()
    {
        time = 0.0f;
        isFired = false;

        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;

        GetComponentInChildren<Collider>().enabled = false;

        shooter = null;

        this.gameObject.layer = LayerMask.NameToLayer("Default");

        interpolationSight = Vector3.zero;
        power = 10.0f;
    }

    public void Initialize(float power, Vector3 pos, Quaternion rot, Transform shooter, Transform target = null, Vector3 deltaSight = new Vector3()) //deltaSight: 시야각 보간 벡터
    {
        interpolationSight = deltaSight;
        rb.isKinematic = false;
        this.shooter = shooter;
        if (shooter.tag == "Player")
        {
            this.gameObject.layer = LayerMask.NameToLayer("PlayerSnowBall");
        }
        else if (shooter.tag == "Enemy")
        {
            this.gameObject.layer = LayerMask.NameToLayer("EnemySnowBall");
        }
        else
        {
            Debug.Log("Error with shooterName");
        }

        transform.position = pos;
        transform.rotation = Quaternion.Euler(Vector3.zero);

        rb.useGravity = true;

        GetComponentInChildren<Collider>().enabled = true;

        Fire();

    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != null || other.tag != "")
        {
            if (shooter.tag == other.tag) return;
            if (other.transform.parent != null) if (shooter.tag == other.transform.parent.tag) return;
            if (other.name == "FollowColl" || other.name == "AttackColl") return;
            if (other.name == "Sight Camera") return;
            if (shooter.tag == "Enemy" && other.tag == "Player")
            {
                PlayerScript player = other.transform.GetComponent<PlayerScript>();
                player.setHP(-damage);
            }
            if (shooter.tag == "Player" && other.tag == "Enemy")
            {
                EnemyAIScript enemy = other.transform.GetComponent<EnemyAIScript>();
                enemy.Hit(damage);
            }
        }

        SnowParticlePooling particles = GameObject.Find("SnowParticles").GetComponent<SnowParticlePooling>();

        GameObject destroyedEffect = particles.GetObject();
        destroyedEffect.transform.position = transform.position;
        //destroyedEffect.transform.LookAt(destroyedEffect.transform.position - Vector3.Normalize(transform.forward));
        Debug.Log("trigger point: " + other.ClosestPointOnBounds(transform.position) + " transform.position: " + transform.position);

        destroyedEffect.transform.LookAt(Vector3.Normalize(other.ClosestPointOnBounds(transform.position) - transform.position) + transform.position);

        SnowBallPoolingScript.Instance.ReturnObject(this);
        
    }

    Vector3 GetVelocity()
    {
        Vector3 finalVelocity;
        if (shooter.CompareTag("Player"))
        {
            finalVelocity = shooter.GetComponentInChildren<PlayerSightScript>().transform.forward * power + interpolationSight;
        }
        else finalVelocity = shooter.transform.forward * power + interpolationSight;

        Vector3 shooterVel = shooter.GetComponent<Rigidbody>().velocity;
        finalVelocity += shooterVel;

        return finalVelocity;
    }

    void SetVelocity(Vector3 velocity)
    {
        rb.velocity = velocity;
    }

    void Fire()
    {
        Vector3 velocity = GetVelocity();
        SetVelocity(velocity);
    }

}
