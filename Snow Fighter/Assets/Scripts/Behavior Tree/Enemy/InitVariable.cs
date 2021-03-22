using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using BBUnity.Actions;

[Action("Enemy/InitVariable")]
[Help("Initialize variables in enemy behavior tree")]
public class InitVariable : GOAction
{
    [OutParam("player")]
    public GameObject player;

    [OutParam("attackingDist")]
    float attackingDist;

    [OutParam("attackingAngle")]
    float attackingAngle;

    [OutParam("followingDist")]
    float followingDist;

    [OutParam("followingAngle")]
    float followingAngle;

    EnemyAIScript enemyAISc;
    // Start is called before the first frame update
    public override void OnStart()
    {
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if(player == null) Debug.LogWarning("Player not specified. Attack will not work for" + gameObject.name);
        }

        enemyAISc = gameObject.GetComponent<EnemyAIScript>();
        attackingDist = enemyAISc.AttackingDist;
        attackingAngle = enemyAISc.SightAngle;
        followingAngle = enemyAISc.SightAngle;
        followingDist = enemyAISc.FollowingDist;

        base.OnStart();
    }
}
