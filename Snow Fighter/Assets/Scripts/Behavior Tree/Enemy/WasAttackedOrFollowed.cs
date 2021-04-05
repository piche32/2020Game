using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pada1.BBCore;
using BBUnity.Conditions;

[Condition("Enemy/WasAttackedOrFollowed")]
[Help("Check enemy's previous state. If Previous state is Attack or Follow, return true.")]

public class WasAttackedOrFollowed : GOCondition
{
    private EnemyAIScript enemyAISc;
    public override bool Check()
    {
        enemyAISc = gameObject.GetComponent<EnemyAIScript>();
        if (enemyAISc.PreState == EnemyState.STATE_ATTACKING || enemyAISc.PreState == EnemyState.STATE_FOLLOWING)
            return true;
        else return false;
        //throw new System.NotImplementedException();
    }
}
