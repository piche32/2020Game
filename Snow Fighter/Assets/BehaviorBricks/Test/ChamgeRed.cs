using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using BBUnity.Actions;

[Action("ChangeColor/red")]

public class ChamgeRed : GOAction
{
    [InParam("wasRed")]
    public bool wasRed;

    [InParam("wasBlue")]
    public bool wasBlue;

    [InParam("wasYellow")]
    public bool wasYellow;

    Material red;
    // Start is called before the first frame update
    public override void OnStart()
    {
        red = Resources.Load<Material>("Materials/Enemy");
    }

    // Update is called once per frame
    public override TaskStatus OnUpdate()
    {
        wasRed = true;
        wasBlue = false;
        this.gameObject.GetComponent<MeshRenderer>().material = red;
        return TaskStatus.RUNNING;

    }
}
