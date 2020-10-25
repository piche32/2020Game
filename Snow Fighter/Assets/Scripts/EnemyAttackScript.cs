using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            this.gameObject.GetComponentInParent<EnemyAIScript>().changeState(EnemyState.STATE_ATTACKING);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            this.gameObject.GetComponentInParent<EnemyAIScript>().changeState(EnemyState.STATE_FOLLOWING);
        }
    }
}
