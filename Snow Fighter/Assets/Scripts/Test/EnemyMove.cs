using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    NavMeshAgent nvAgent;
    // Start is called before the first frame update
    void Start()
    {
        nvAgent = GetComponent<NavMeshAgent>();
        nvAgent.enabled = true;
        nvAgent.SetDestination(GameObject.Find("Player").transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        nvAgent.SetDestination(GameObject.Find("Player").transform.position);

    }
}
