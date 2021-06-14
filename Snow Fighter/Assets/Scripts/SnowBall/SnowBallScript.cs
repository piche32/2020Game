using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SnowBallScript : MonoBehaviour
{
    #region projectile using rigidbody
    //그냥 rigidbody에게 포물선을 맡기는 코드
    /*float time;
    Transform shooter;

    [SerializeField] float damage = 10.0f;

    bool isFired;
    public bool IsFired { get { return isFired; } set { isFired = value; } }
    private float power = 10.0f;

    Rigidbody rb;

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

        power = 10.0f;
    }

    public void Initialize(float power, Vector3 pos, Quaternion rot, Transform shooter, Transform target = null)
    {
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
            finalVelocity = shooter.GetComponentInChildren<PlayerSightScript>().transform.forward * power;
        }
        else finalVelocity = shooter.transform.forward * power;

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
        // Vector3 velocity = GetVelocity();
        // SetVelocity(velocity);
    }
    */

    #endregion projectile using rigidbody

    float time;
    Transform shooter;
    public Transform Shooter { get { return shooter; } set { shooter = value; } }

    [SerializeField] float damage = 10.0f;

    bool isFired;
    public bool IsFired { get { return isFired; } set { isFired = value; } }
    private float power = 10.0f;

    Rigidbody rb;

    Vector3 target;
    public float firingAngle = 45.0f; //포물선 각도
    float gravity = 9.8f;

    public float offset = 2.0f; //디폴트 눈 던지는 높이

    Coroutine projectileCoroutine;

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

        power = 10.0f;

        //firingAngle = 45.0f; //제일 높이 오르는 각도
    }

    public void Initialize(float power, Vector3 pos, Quaternion rot, Transform shooter, Vector3 target)
    {
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

        this.target = target;

        if(Vector3.Equals(target, Vector3.negativeInfinity))
            Fire();
        else
            projectileCoroutine = StartCoroutine(projectile());

    }
    
    public void Initialize( Transform shooter)
    {
        //rb.isKinematic = false;
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

        GetComponentInChildren<Collider>().enabled = true;


    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        //필드 밖으로 떨어졌을 경우, 삭제
        if(time > 30) SnowBallPoolingScript.Instance.ReturnObject(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name.ToString());
        if (other.tag != null || other.tag != "")
        {
            if (other.name == "<multi>wall") return;
            if (shooter.tag == other.tag) return;
            if (other.transform.parent != null) if (shooter.tag == other.transform.parent.tag) return;
            if (other.name == "FollowColl" || other.name == "AttackColl") return;
            if (other.name == "Sight Camera") return;
            if (shooter.tag == "Enemy" && other.tag == "Player")
            {
                PlayerScript player = other.transform.GetComponent<PlayerScript>();
                player.damaged(-damage);
                //player.setHP(-damage);
            }
            if (shooter.tag == "Player" && other.tag == "Enemy")
            {
                Enemy.Ver2.Enemy enemy = other.transform.GetComponent<Enemy.Ver2.Enemy>();
                //EnemyAIScript enemy = other.transform.GetComponent<EnemyAIScript>();
                if(enemy != null)
                    enemy.Hit(damage);
            }
            if(other.tag == "AttackingTestObj")
            {
                other.transform.GetComponent<AttackingTest>().Hit(damage);
            }

        }

        SnowParticlePooling particles = GameObject.Find("SnowParticles").GetComponent<SnowParticlePooling>();

        GameObject destroyedEffect = particles.GetObject();
        destroyedEffect.transform.position = transform.position;
        //destroyedEffect.transform.LookAt(destroyedEffect.transform.position - Vector3.Normalize(transform.forward));
        //Debug.Log("trigger point: " + other.ClosestPointOnBounds(transform.position) + " transform.position: " + transform.position);

        destroyedEffect.transform.LookAt(Vector3.Normalize(other.ClosestPointOnBounds(transform.position) - transform.position) + transform.position);
        
        SnowBallPoolingScript.Instance.ReturnObject(this);

    }

    IEnumerator projectile()
    {

        //Short delay added before Projectile is thrown
        //yield return new WaitForSeconds(1.5f);

        //Move projectile to the position of throwing object + add some offset if needed.
        //Projectile.position = myTransform.position;

        //Cacluate distance to target
        float targetDistance = Vector3.Distance(transform.position, target);

        //Calculate the velocity needed to throw the object to the target at specified angle.
        float projectileVelocity = targetDistance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);

        //Extract the X Y component of the velocity
        float Vx = Mathf.Sqrt(projectileVelocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectileVelocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

        //Calculate flight time.
        float flightDuration = targetDistance / Vx;

        //Rotate projectile to face the target.
        transform.rotation = Quaternion.LookRotation(target - transform.position);

        float elapseTime = 0;

        while (elapseTime < flightDuration)
        {
            transform.Translate(0, (Vy - (gravity * elapseTime)) * Time.deltaTime, Vx * Time.deltaTime);
            elapseTime += Time.deltaTime;

            yield return null;
        }

    }

    Vector3 GetVelocity()
    {
        Vector3 finalVelocity;
        Vector3 shooterVel;
        if (shooter.CompareTag("Player"))
        {
            finalVelocity = shooter.GetComponentInChildren<PlayerSightScript>().transform.forward * power;
            shooterVel = shooter.GetComponent<Rigidbody>().velocity;
            finalVelocity += shooterVel;
            finalVelocity += Vector3.up * offset;

            return finalVelocity;
        }
        else finalVelocity = shooter.transform.forward * power/2 + shooter.transform.up * power/2;

        shooterVel = shooter.GetComponent<Rigidbody>().velocity;
        finalVelocity += shooterVel;
        //finalVelocity += Vector3.up * offset;

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
