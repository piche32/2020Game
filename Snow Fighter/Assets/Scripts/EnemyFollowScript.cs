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
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;

        if (enemyAI.getCurState() == EnemyState.STATE_FOLLOWING) return;
        if (enemyAI.isTargetInSight())
        {
            enemyAI.setState(EnemyState.STATE_FOLLOWING);
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag != "Player") return;

        if ((enemyAI.getCurState() == EnemyState.STATE_FOLLOWING) || (enemyAI.getCurState() == EnemyState.STATE_ATTACKING)) return;

        if (enemyAI.isFollowingTimeOver()) return;
            if (enemyAI.isTargetInSight())
        {
            enemyAI.setState(EnemyState.STATE_FOLLOWING);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player") return;

        if (enemyAI.getCurState() != EnemyState.STATE_FOLLOWING) return;

        enemyAI.setState(EnemyState.STATE_IDLE);
        
    }
}
