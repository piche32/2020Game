using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using BBUnity.Actions;

[Action("Enemy/Died")]
[Help("Enemy Died")]

public class Died : GOAction
{
    private EnemyAIScript enemyAISc;

    public override void OnStart()
    {
        enemyAISc = gameObject.GetComponent<EnemyAIScript>();

        base.OnStart();
    }

    public override TaskStatus OnUpdate()
    {
        enemyAISc.died();
        return base.OnUpdate();
    }
}
