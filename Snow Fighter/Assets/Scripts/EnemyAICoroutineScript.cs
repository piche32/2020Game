using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

enum EnemyCoroutineState
{
    STATE_IDLE,
    STATE_FOLLOWING,
    STATE_ATTACKING
}

public class EnemyAICoroutineScript : MonoBehaviour
{
    EnemyCoroutineState state;

    IEnumerator alertCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        state = EnemyCoroutineState.STATE_IDLE;
        alertCoroutine = alert();

    }

    // Update is called once per frame
    void Update()
    {
        checkState();
    }

    void checkState()
    {

    }

    IEnumerator alert()
    {
        Debug.Log("first alert");
        yield return new WaitForSeconds(2);
        Debug.Log("Second alert");
        yield return null;
    }
    
}
