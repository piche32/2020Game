using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using BBUnity.Actions;

[Action("ChangeColor/Blue")]

public class ChangeBlue : GOAction
{

    [InParam("wasRed")]
    public bool wasRed;

    [InParam("wasBlue")]
    public bool wasBlue;

    [InParam("wasYellow")]
    public bool wasYellow;


    Material blue;
    // Start is called before the first frame update
    public override void OnStart()
    {
        blue = Resources.Load<Material>("Materials/Player");
    }

    // Update is called once per frame
    public override TaskStatus OnUpdate()
    {
        wasBlue = true;
        wasYellow = false;
        this.gameObject.GetComponent<MeshRenderer>().material = blue;
        return TaskStatus.COMPLETED;
    }
}
