using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using Pada1.BBCore.Framework;
using BBUnity.Actions;

[Action("Enemy/Patrol")]
[Help("Wandering and alerting waypoints")]

public class Patrol : GOAction
{
    private EnemyAIScript enemyAISc;

    public override void OnStart()
    {
        enemyAISc = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyAIScript>();

        base.OnStart();
    }
    public override TaskStatus OnUpdate()
    {
        enemyAISc.idle();
        return TaskStatus.COMPLETED;
    }
}
