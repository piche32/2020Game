using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackScript : MonoBehaviour
{
    EnemyAIScript enemyAI;
    // Start is called before the first frame update
    void Start()
    {
        enemyAI = this.gameObject.GetComponentInParent<EnemyAIScript>();
    }

    // Update is called once per frame
  
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;
        if(enemyAI.CurState == EnemyState.STATE_FOLLOWING)
        {
            enemyAI.setState(EnemyState.STATE_ATTACKING);
            return;
        }
        if (enemyAI.isTargetInSight())
            enemyAI.setState(EnemyState.STATE_ATTACKING);

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag != "Player") return;
        if (enemyAI.CurState != EnemyState.STATE_IDLE) return;
        if (enemyAI.isTargetInSight())
        {
            enemyAI.setState(EnemyState.STATE_ATTACKING);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player") return;

        if (enemyAI.CurState != EnemyState.STATE_ATTACKING) return;
        
        enemyAI.setState(EnemyState.STATE_FOLLOWING);
      
    }
}
