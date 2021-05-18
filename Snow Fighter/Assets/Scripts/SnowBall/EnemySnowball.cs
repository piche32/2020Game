using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySnowball : MonoBehaviour
{
    public GameObject projectile;
    public SphereCollider sensorTrigger = null;
    GameObject player;
    public float shootForce;
    public float attackCoolDown = 1;

    private float timer;

    private Vector3 targetPos;
    private float distanceToTarget;
    private Vector3 targetDirection;
    private float targetSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(timer >= attackCoolDown)
        {
            ShootProjectile();
        }
    }

    //AI has a large Sphere Collider attached to detect collisions of potential threats
    //When there is a collider,
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Get target info
            Vector3 velocity = other.attachedRigidbody.velocity;
            targetPos = other.transform.position;
            distanceToTarget = Vector3.Distance(sensorTrigger.transform.position, other.transform.position);
        }
    }

    void ShootProjectile()
    {
        float projectileTimeToTarget = distanceToTarget / shootForce;
        
        //float projectedTargetTravelDistance = targetSpeed * projectileTimeToTarget;

        Vector3 projectedTarget = targetPos + targetDirection * projectileTimeToTarget;
        projectedTarget.y += 1; //aim at center of target if 2m high

        GameObject go = Instantiate(projectile, transform.position, Quaternion.identity);
        go.transform.LookAt(projectedTarget);
        Rigidbody rb = go.GetComponent<Rigidbody>();
        rb.velocity = go.transform.forward * shootForce;
        rb.useGravity = true;

        timer = 0f;
    }
}
