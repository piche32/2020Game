using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowScript : MonoBehaviour
{
    EnemyAIScript enemyAI;
    // Start is called before the first frame update
    void Start()
    {
        enemyAI = this.gameObject.GetComponentInParent<EnemyAIScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (enemyAI.getState() == EnemyState.STATE_ATTACKING) return;
        if (other.tag == "Player")
        {
            if(enemyAI.isTargetInSight())
            {
                enemyAI.setState(EnemyState.STATE_FOLLOWING);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            enemyAI.setState(EnemyState.STATE_IDLE);
        }
    }
}
