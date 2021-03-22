using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pada1.BBCore;
using BBUnity.Conditions;

[Condition("Enemy/CheckHP")]
[Help("Check HP")]

public class CheckHP : GOCondition
{
    private EnemyAIScript enemyAISc;

    public override bool Check()
    {
        enemyAISc = gameObject.GetComponent<EnemyAIScript>();
        return enemyAISc.checkHp();
       // throw new System.NotImplementedException();
    }
}
