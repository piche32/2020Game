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

    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 v = (nvAgent.nextPosition - transform.position) * Time.deltaTime;
        animator.SetFloat("Speed", nvAgent.speed);
       // Debug.Log(v);
        //Vector3 v = (nvAgent.nextPosition - transform.position) * Time.deltaTime;
    }

}
