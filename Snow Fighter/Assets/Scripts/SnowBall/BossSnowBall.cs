using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum AttackType
{
    None = 0,
    Roll,
    Throw,
    Num
}

public class BossSnowBall : MonoBehaviour
{
    AttackType attackType;
    bool isRoll = false;
    bool isThrow = false;

    float time = 0.0f;
    float destroyTime = 5.0f;
    float damage = 0.0f;

    Rigidbody rb = null;

    //public Transform sight;
    public GameObject graphic;

    public GameObject particleObj;
    ParticleSystem particle;

    public GameObject effectArea;

    public Vector3 throwInterval;
    public float throwAngle;

    SphereCollider coll;

    Transform snowStart;

    public Transform sight;
    
    private void Start()
    {
        attackType = AttackType.None; 
        rb = this.GetComponent<Rigidbody>();
        disabled();
        particle = particleObj.GetComponent<ParticleSystem>();
        particleObj.SetActive(false);

        effectArea.SetActive(false);
       
        coll = this.GetComponent<SphereCollider>();

        snowStart = null;
    }

    private void Update()
    {
        if (isRoll || isThrow)
        {
            if (time > destroyTime)
            {
                this.disabled();
                return;
            }
            time += Time.deltaTime;
        }

        if (particleObj.activeSelf)
        {
            if (particle.isStopped)
            {
                particleObj.SetActive(false);
            }
        }

        if (graphic.activeSelf && snowStart != null)
        {
            transform.rotation = snowStart.rotation;

            if (attackType == AttackType.Roll)
            {
                transform.position = snowStart.position;
                transform.localScale = Vector3.one;
             //   particleObj.transform.localScale = Vector3.one * 2;
            }
            if (attackType == AttackType.Throw)
            {
                transform.position = snowStart.position + (snowStart.right * throwInterval.x) + (snowStart.forward * throwInterval.z) + (snowStart.up * throwInterval.z);
                transform.localScale = Vector3.one * 2;
              //  particleObj.transform.localScale = Vector3.one * 4;
            }
        }
    }

    void resetActionBool()
    {
        isRoll = false;
        isThrow = false;
    }

    public void init(string sAttackType, Transform startTrans, float _damage)
    {
        // transform.parent = snowStart;
        // transform.rotation = snowStart.rotation;
        snowStart = startTrans;
        if (sAttackType == "Roll")
        {
            attackType = AttackType.Roll;
            transform.position = snowStart.position;
            transform.localScale = Vector3.one / 2;
            particleObj.transform.localScale = Vector3.one * 2;
        }
        if(sAttackType == "Throw")
        {
            attackType = AttackType.Throw;
            transform.position = snowStart.position + (snowStart.right * throwInterval.x) + (snowStart.forward * throwInterval.z) + (snowStart.up * throwInterval.z);
            transform.localScale = Vector3.one;
            particleObj.transform.localScale = Vector3.one * 4;
        }

        coll.enabled = true;
        coll.isTrigger = true;

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        rb.velocity = Vector3.zero;

        graphic.SetActive(true);
        particleObj.SetActive(false);
        effectArea.SetActive(false);

        damage = _damage;

        resetActionBool();
    }

    public void Roll(Vector3 forward, float power, Vector3 destination)
    {
       
        isRoll = true;
       
        time = 0.0f;
        transform.parent = null;
        rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints.None;
        
        transform.LookAt(this.transform.position + forward);
        coll.isTrigger = false;
        Vector3 dir = destination - this.transform.position;
        //dir.y = this.transform.position.y;
        dir = dir.normalized;
    //    sight.position = this.transform.position + dir;

        rb.velocity = dir * power;
        snowStart = null;
    }

    public void Throw(Vector3 forward, float power)
    {
        isThrow = true;

        time = 0.0f;
        transform.parent = null;
        rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints.None;

        coll.isTrigger = false;

        transform.LookAt(this.transform.position + forward);
        Vector3 dir = forward;
        Quaternion rotate = Quaternion.Euler(throwAngle, 0.0f, 0.0f);
        dir = rotate * dir;

        rb.velocity = dir.normalized * power;
        sight.position = transform.position + dir * power;

        snowStart = null;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == " <multi>wall") return;

        if (isRoll)
        {
            if (collision.gameObject.tag == "Ground") return;
            if (collision.gameObject.tag == "Enemy") return;

            if (collision.gameObject.tag == "Player")
            {
                //플레이어 공격
                PlayerScript player = collision.transform.GetComponent<PlayerScript>();
                player.damaged(-damage);
            //    EventContainer.Instance.Events["OnPlayerDamaged"].Invoke();
            }
        }
        if (isThrow)
        {
            if (collision.gameObject.tag == "Enemy") return;

            //눈덩이 이펙트 새로 만들기
            effectArea.SetActive(true);

            if (collision.gameObject.tag == "Player")
            {
                PlayerScript player = collision.transform.GetComponent<PlayerScript>();
                player.damaged(-damage);
               //// EventContainer.Instance.Events["OnPlayerDamaged"].Invoke();
            }
        }

        disabled();

        //눈 부서지는 파티클 재생
        particleObj.transform.position = transform.position;
        particleObj.transform.LookAt(Vector3.Normalize(collision.collider.ClosestPointOnBounds(transform.position) - transform.position) + transform.position);
        particle.Play();
    }

    void disabled()
    {
        if (isRoll)
        {
            isRoll = false;
        }
        if (isThrow)
        {
            isThrow = false;
        }
        graphic.SetActive(false);
        rb.velocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        rb.isKinematic = true;
        GetComponent<SphereCollider>().enabled = false;
       
        particleObj.SetActive(true);
    }
}
