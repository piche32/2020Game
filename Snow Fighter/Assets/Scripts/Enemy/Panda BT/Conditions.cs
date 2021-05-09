using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Panda;

namespace Panda
{
    public class Conditions : MonoBehaviour
    {
        /// <summary>
        /// Checks whether target is close depending on a given distance and in self's sight.
        /// </summary>
        /// <param name="self">GameObject to watch the target</param>
        /// <param name="target">GameObject to check the distance with tag </param>
        /// <param name="maxDist">The maximum distance to consider that the target is in sight </param>
        /// <param name="sightAngle">The angle in degree to consider that the target is in sight</param>
        /// <returns></returns>
        public static bool isTargetInSight(GameObject self, GameObject target, float maxDist, float sightAngle)
        {
            Vector3 dir = (target.transform.position - self.transform.position);
            if (dir.sqrMagnitude > maxDist * maxDist) return false; //거리가 maxDist보다 멀면 false

            float angle = Vector3.Angle(dir, self.transform.forward);
            if(angle < sightAngle * 0.5f)
            {
                /*RaycastHit[] hits = Physics.RaycastAll(self.transform.position, self.transform.forward, maxDist);
                if (hits == null)
                {
                    return false; }
                foreach (RaycastHit hit in hits)
                {
                    if (hit.collider.transform.root.tag == target.tag)
                    {
                        return true;
                    }
                }*/
                return true;
            }
            return false;
        }
    }
}

