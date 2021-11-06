using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;
using UnityEngine.AI;

namespace Enemy.Ver2
{
    public class EnemyAIBT : MonoBehaviour
    {
        Enemy self = null;
        protected PlayerScript player = null;

        NavMeshAgent nvAgent = null;
        // Animator animator = null;

        public WaypointPath waypointPath;
        int waypointIndex;
        Vector3 destination;

        [SerializeField] float defaultStoppingDist = 1.5f;
        [SerializeField] float attackStoppingDist = 1.5f;

        [SerializeField] protected float attackDist = 5.0f;
        //IsPlayerInEnemySight enemyAttackSight = null;
        IsPlayerInEnemySight enemyFollowSight = null;
        protected virtual void Start()
        {

            player = GameObject.Find("Player").GetComponent<PlayerScript>();
            if (player == null)
                Debug.LogError("[Enemy.Ver2.EnemyAIBT.cs]Can't Find PlayerScript.");

            nvAgent = GetComponent<NavMeshAgent>();

            self = GetComponent<Enemy>();

            waypointIndex = 0;

            //enemyAttackSight = this.transform.Find("AttackColl").GetComponent<IsPlayerInEnemySight>();
            enemyFollowSight = this.transform.Find("FollowColl").GetComponent<IsPlayerInEnemySight>();

        }

        /// <summary>
        /// 공격할 수 있는 범위로 Player가 들어왔는지 확인
        /// 가까이 있을 땐 180도 정도, 멀 땐 120도까지 시야 범위
        /// </summary>
        /// <returns></returns>
        [Task]
        protected bool IsPlayerInSight(string type)
        {
            bool ret = false;
            if (player != null && player.gameObject != null)
            {
                if (type == "Attack")
                {
                    Vector3 dir = player.transform.position - this.transform.position;
                    dir.y = 0.0f;
                    if (dir.magnitude < attackDist)
                        ret = true;
                }
                else if (type == "Follow")
                { ret = enemyFollowSight._IsPlayerInEnemySight; }
            }
            return ret;
        }

        [Task]
        public bool SetDestination_Player()
        {
            bool ret = false;
            nvAgent.stoppingDistance = attackStoppingDist;
            ret = SetDestination(player.transform.position);
            return ret;
        }

        /// <summary>
        /// NaviMesh Stop
        /// </summary>
        /// <returns></returns>
        /// 

        [Task]
        public bool Stop()
        {
            nvAgent.isStopped = true;
            return true;
        }

        [Task]
        public void StartNvAgent()
        {
            nvAgent.isStopped = false;
            Task.current.Succeed();

        }

        int waypointArrayIndex
        {
            get
            {
                int i = 0;
                if (waypointPath.loop) //루프일 때
                {
                    i = waypointIndex % waypointPath.Waypoints.Length;
                }
                else //루프 아닐 때(12345->4321 이런 식으로 움직임)
                {
                    int n = waypointPath.Waypoints.Length; //waypointPath 최대 길이
                    i = waypointIndex % (n * 2);

                    if (i > n - 1)
                    {
                        i = (n - 1) - (i % n);
                    }
                }
                return i;
            }
        }

        [Task]
        public bool SetDestination(Vector3 p)
        {
            destination = p;
            if (nvAgent == null || !nvAgent.isActiveAndEnabled) return false;
            nvAgent.SetDestination(destination);

            if (Task.isInspected)
                Task.current.debugInfo = string.Format("({0}. {1})", destination.x, destination.y);

            return true;
        }

        [Task]
        public bool SetDestination_Waypoint()
        {
            bool isSet = false;
            if (nvAgent == null || !nvAgent.isActiveAndEnabled) return isSet;
            nvAgent.isStopped = false;
            nvAgent.stoppingDistance = defaultStoppingDist;
            if (waypointPath != null)
            {
                var i = waypointArrayIndex;
                var p = waypointPath.Waypoints[i].position;
                isSet = SetDestination(p);
            }
            return isSet;
        }

        [Task]
        public bool isArrived()
        {
            bool ret = false;
            Vector3 dest = this.transform.position;
            dest.y = this.transform.position.y;
            float d = Vector3.Distance(dest, destination);
            //if (d < 5.0f) 원본
            if (d <= nvAgent.stoppingDistance + 1.0f) //1.0f은 offset
                ret = true;

            if (Task.isInspected)
                Task.current.debugInfo = string.Format("d-{0:0.00}", d);
            return ret;
        }

        [Task]
        bool NextWaypoint()
        {
            if (waypointPath != null)
            {
                waypointIndex = (++waypointIndex) % (waypointPath.Waypoints.Length * 2);
                if (Task.isInspected)
                    Task.current.debugInfo = string.Format("i = {0}", waypointArrayIndex);
            }
            return true;
        }

        /// <summary>
        /// CheckHP
        /// If HP <= 0 return true
        /// </summary>
        /// <returns></returns>
        [Task]
        protected bool CheckHP()
        {
            return self.checkHp();
        }

        [Task]
        protected void MakeRagdoll()
        {
            GetComponent<Collider>().isTrigger = false;
            //GetComponent<Collider>().enabled = false;
            nvAgent.enabled = false;
            //animator.enabled = false;
            GetComponent<RagdollChanger>().ChangeRagdoll();
            GetComponent<Collider>().isTrigger = true;
            Task.current.Succeed();
        }

        [Task]
        protected void Blink()
        {
            this.GetComponentInChildren<Renderer>().enabled = !this.GetComponentInChildren<Renderer>().enabled;
            Task.current.Succeed();
        }

        [Task]
        protected void Die()
        {
            self.Die();
            Task.current.Succeed();
        }

    }

}