using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using BBUnity.Actions;

[Action("Enemy/Attack")]
[Help("Attack player")]

public class Attack : GOAction
{
    [InParam("player")]
    public GameObject player;

    private EnemyAIScript enemyAISc;

    public override void OnStart()
    {
        if(player == null)
        { 
            player = GameObject.FindGameObjectWithTag("Player");
            if (player == null) Debug.LogWarning("Player not specified. Attack will not work for" + gameObject.name);
        }

        enemyAISc = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyAIScript>();

        base.OnStart();
    }

    public override TaskStatus OnUpdate()
    {
        if (player == null) return TaskStatus.FAILED;
        enemyAISc.attack();
        
        return base.OnUpdate();
    }
}
