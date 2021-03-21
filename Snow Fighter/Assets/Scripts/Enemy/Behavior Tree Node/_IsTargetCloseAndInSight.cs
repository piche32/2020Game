using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pada1.BBCore;
using BBUnity.Conditions;

[Condition("Perception/_IsTargetCloseAndInSight")]
[Help("This is custom condition node. Using tag, checks whether a target is close depending on a given distance and in sight")]
public class _IsTargetCloseAndInSight : GOCondition
{
    [InParam("target")]
    [Help("Target to check the distance with tag")]
    public GameObject target;

    [InParam("closeDistance")]
    [Help("The maximum distance to consider that the target is close")]
    public float closeDistance;

    [InParam("angle")]
    [Help("The angle in degree to consider that the target is in sight")]
    public float sightAngle;

    // Start is called before the first frame update
    public override bool Check()
    {
        Vector3 dir = (target.transform.position - gameObject.transform.position);
        if (dir.sqrMagnitude > closeDistance * closeDistance)
            return false;

        float angle = Vector3.Angle(dir, gameObject.transform.forward);
        RaycastHit ray;
        if(angle < sightAngle * 0.5f)
        {
            if(Physics.Raycast(gameObject.transform.position + gameObject.transform.up * 0.5f, dir, out ray, dir.magnitude))
            {
                return ray.transform.CompareTag(target.tag);
            }
        }
        return false;
    }
}
