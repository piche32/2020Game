using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSnowBall : MonoBehaviour
{
    bool isRoll = false;
    float time = 0.0f;
    float destroyTime = 5.0f;
    float damage = 0.0f;

    Rigidbody rb = null;

    //public Transform sight;
    public GameObject graphic;

    public GameObject particleObj;
    ParticleSystem particle;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        disabled();
        particle = particleObj.GetComponent<ParticleSystem>();
        particleObj.SetActive(false);
        
    }
    private void Update()
    {
        if (isRoll)
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
    }
    public void Roll(Vector3 forward, float power, Vector3 destination)
    {
        isRoll = true;
        time = 0.0f;
        transform.parent = null;
        rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints.None;
        
        transform.LookAt(this.transform.position + forward);
        GetComponent<SphereCollider>().isTrigger = false;
        Vector3 dir = destination - this.transform.position;
        //dir.y = this.transform.position.y;
        dir = dir.normalized;
    //    sight.position = this.transform.position + dir;

        rb.velocity = dir.normalized * power;
    }

    public void init(Transform snowStart, float _damage)
    {
        transform.parent = snowStart;
        transform.position = snowStart.position;
        transform.rotation = snowStart.rotation;
        transform.localScale = Vector3.one / 2;
        GetComponent<SphereCollider>().enabled = true;
        GetComponent<SphereCollider>().isTrigger = true;
        
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        rb.velocity = Vector3.zero;

        graphic.SetActive(true);
        isRoll = false;
        particleObj.SetActive(false);

        damage = _damage;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground") return;
        if (collision.gameObject.tag == "Enemy") return;

        if(collision.gameObject.tag == "Player")
        {
            //플레이어 공격
            PlayerScript player = collision.transform.GetComponent<PlayerScript>();
            player.damaged(-damage);
            EventContainer.Instance.Events["OnPlayerAttacked"].Invoke();
        }
        disabled();
        particleObj.transform.position = transform.position;
        particleObj.transform.LookAt(Vector3.Normalize(collision.collider.ClosestPointOnBounds(transform.position) - transform.position) + transform.position);
        particle.Play();
    }

    void disabled()
    {
        graphic.SetActive(false);
        rb.velocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        rb.isKinematic = true;
        GetComponent<SphereCollider>().enabled = false;
        isRoll = false;
        particleObj.SetActive(true);
    }
}
