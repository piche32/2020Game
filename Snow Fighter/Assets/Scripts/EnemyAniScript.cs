using System.Collections;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAniScript : MonoBehaviour
{
    NavMeshAgent nvAgent;
    Animator animator;
    Rigidbody rig;
    // Start is called before the first frame update
    void Start()
    {
        nvAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        rig = GetComponent<Rigidbody>();

       // nvAgent.updatePosition = false;
    }

    // Update is called once per frame
    void Update()
    {

        //Vector3 v = (nvAgent.nextPosition - transform.position) * Time.deltaTime;
        //animator.SetFloat("Speed", Vector3.Distance(Vector3.zero, v));
       // Debug.Log(v);
        //Vector3 v = (nvAgent.nextPosition - transform.position) * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        Vector3 v = (nvAgent.nextPosition - transform.position) * Time.deltaTime;
        animator.SetFloat("Speed", Vector3.Distance(Vector3.zero, v));
        Debug.Log(v);
    }

    private void OnAnimatorMove()
    {
       // transform.position = nvAgent.nextPosition;
    }
}
