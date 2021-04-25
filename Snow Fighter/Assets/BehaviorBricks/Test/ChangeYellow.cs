using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using BBUnity.Actions;

[Action("ChangeColor/yellow")]
public class ChangeYellow : GOAction
{

    [InParam("wasRed")]
    public bool wasRed;

    [InParam("wasBlue")]
    public bool wasBlue;

    [InParam("wasYellow")]
    public bool wasYellow;

    Material yellow;
    // Start is called before the first frame update
    public override void OnStart()
    {
        yellow = Resources.Load<Material>("Materials/Enemy");
    }

    // Update is called once per frame
    public override TaskStatus OnUpdate()
    {
        wasYellow = true;
        wasRed = true;
        this.gameObject.GetComponent<MeshRenderer>().material = yellow;
        return TaskStatus.RUNNING;
    }
}
