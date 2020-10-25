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
    void Update()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            enemyAI.setState(EnemyState.STATE_ATTACKING);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            enemyAI.setState(EnemyState.STATE_FOLLOWING);
        }
    }
}
