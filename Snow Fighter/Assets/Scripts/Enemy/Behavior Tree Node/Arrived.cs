using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using Pada1.BBCore.Framework;

[Condition("Enemy/Arrived")]
[Help("Checks whether it arrived destination")]

public class Arrived : ConditionBase
{
    public override bool Check()
    {
        return true;
    }

}
