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
    [InParam("player")]
    public GameObject player;

    // Start is called before the first frame update
    public override void OnStart()
    {
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if(player == null) Debug.LogWarning("Player not specified. Attack will not work for" + gameObject.name);
        }
        base.OnStart();
    }
}
