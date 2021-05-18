using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy.Ver2
{
    public class WaypointPath : MonoBehaviour
    {
        public bool loop;
        private Transform[] waypoints;
        public Transform[] Waypoints { get { return waypoints; } }

        // Use this for initialization
        void Start()
        {

            var wp = new List<Transform>();
            foreach (Transform c in this.transform)
            {
                wp.Add(c);
            }
            waypoints = wp.ToArray();
        }

    }
}