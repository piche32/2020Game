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
        animator.SetFloat("Speed", Vector3.Magnitude(nvAgent.velocity));
      Debug.Log(Vector3.Magnitude(nvAgent.velocity));
        //Vector3 v = (nvAgent.nextPosition - transform.position) * Time.deltaTime;
    }

}
