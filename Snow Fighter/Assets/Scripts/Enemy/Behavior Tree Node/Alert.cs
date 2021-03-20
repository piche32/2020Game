using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using BBUnity.Actions;

[Action("Enemy/Alert")]
[Help("alerting waypoints")]

public class Alert : GOAction
{

    private EnemyAIScript enemyAISc;
    public override void OnStart()
    {
        enemyAISc = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyAIScript>();
        base.OnStart();
    }

    // Update is called once per frame
    public override TaskStatus OnUpdate()
    {
        enemyAISc.alert();
        return base.OnUpdate();
    }
}
