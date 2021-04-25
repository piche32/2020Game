using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using Pada1.BBCore.Framework;
using BBUnity.Actions;

[Action("Enemy/Idle")]
[Help("Idle")]

public class Idle : GOAction
{
    private EnemyAIScript enemyAISc;

    [InParam("stoppingDist")]
    public float stoppingDist;

    public override void OnStart()
    {
        enemyAISc = gameObject.GetComponent<EnemyAIScript>();

        //StoppingDist 초기화
        if (stoppingDist == 0) stoppingDist = 5.0f;

        base.OnStart();
    }
    public override TaskStatus OnUpdate()
    {
        if (enemyAISc.CurState == EnemyState.STATE_NONE) enemyAISc.setState(EnemyState.STATE_IDLE);


        //다른 상태에서 idle로 바뀌었을 경우
        if (enemyAISc.CurState != EnemyState.STATE_IDLE)
        {
            enemyAISc.NvAgent.stoppingDistance = stoppingDist;
            enemyAISc.otherToIdle();
            if (enemyAISc.CurState == EnemyState.STATE_ATTACKING)
                enemyAISc.attackToOther();

            enemyAISc.setState(EnemyState.STATE_IDLE);
            return TaskStatus.RUNNING;
        }

        enemyAISc.idle();
        return TaskStatus.RUNNING;

    }


}
