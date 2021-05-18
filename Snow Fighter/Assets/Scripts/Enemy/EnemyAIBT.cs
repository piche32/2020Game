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
        PlayerScript player = null;

        NavMeshAgent nvAgent = null;
        Animator animator = null;

        public WaypointPath waypointPath;
        int waypointIndex;
        Vector3 destination;

        [SerializeField] float damage =10.0f;
        [SerializeField] float attackDelayTime = 1.2f;

        private void Start()
        {
            player = GameObject.Find("Player").GetComponent<PlayerScript>();
            if (player == null)
                Debug.LogError("[Enemy.Ver2.EnemyAIBT.cs]Can't Find PlayerScript.");

            nvAgent = GetComponent<NavMeshAgent>();

            self = GetComponent<Enemy>();

            animator = GetComponentInChildren<Animator>();

            waypointIndex = 0;
        }

        /// <summary>
        /// 공격할 수 있는 범위로 Player가 들어왔는지 확인
        /// 가까이 있을 땐 180도 정도, 멀 땐 120도까지 시야 범위
        /// </summary>
        /// <returns></returns>
        [Task]
        bool canAttack()
        {
            bool ret = false;
            if (player != null && player.gameObject != null)
            {
                IsPlayerInEnemySight self = this.transform.Find("AttackColl").GetComponent<IsPlayerInEnemySight>();
                ret = self._IsPlayerInEnemySight;
            }
            return ret;
        }


        [Task]
        public bool SetDestination_Player()
        {
            bool ret = false;
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

        /// <summary>
        /// 타겟 쪽으로 살짝 몸을 트는 함수
        /// 타겟이 가까이 있을 땐 집요하게 몸을 튼다.
        /// </summary>
        [Task]
        public void AimAt_Target()
        {
            if (player != null)
            {
                var targetDekta = (player.transform.position - this.transform.position);

                Vector3 dir = Vector3.zero; //Enemy와 Player 간의 방향 벡터
                dir.x = player.transform.position.x - this.transform.position.x;
                dir.z = player.transform.position.z - this.transform.position.z;
                dir.y = this.transform.position.y;
                dir = dir.normalized;

                //Enemy와 Player 간의 각도
                float angle = Vector3.Angle(dir, new Vector3(transform.forward.x, dir.y, transform.forward.z));

                if (targetDekta.magnitude > 3f) //일정 거리만큼 멀리 있으면 player쪽으로 회전한다.
                {
                    if (angle > 120f || angle < -120.0f) { Task.current.Fail(); return; }
                    if (angle > 10.0f || angle < -10.0f)
                    {
                        Vector3 look = Vector3.Slerp(this.transform.forward, dir, Time.deltaTime * 3.0f);
                        this.transform.rotation = Quaternion.LookRotation(look, Vector3.up);
                    }
                    else
                        Task.current.Succeed();

                }
                else
                {
                    if (angle > 30.0f || angle < -30.0f)
                    {
                        Vector3 look = Vector3.Slerp(this.transform.forward, dir, Time.deltaTime * 2.0f);
                        this.transform.rotation = Quaternion.LookRotation(look, Vector3.up);
                    }
                    else
                        Task.current.Succeed();
                }
                if (Task.isInspected)
                    Task.current.debugInfo = string.Format("angle={0}", Vector3.Angle(dir, this.transform.forward));

            }


        }

        [Task]
        public void Attack()
        {
            if (animator.GetBool("isAttacking")) {
                    animator.SetBool("isAttacking", false);

                if (!animator.GetCurrentAnimatorStateInfo(1).IsName("Attack")
                    || (animator.GetCurrentAnimatorStateInfo(1).IsName("Attack")
                    && animator.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.9f))
                {
                    Task.current.Succeed();
                }
            }
            else
            {//공격 중이 아닐 때,
                if (!animator.GetCurrentAnimatorStateInfo(1).IsName("Attack")) // 공격 애니메이션 실행 아닐 때
                  { 
                    animator.SetBool("isAttacking", true);
                    Invoke("HitPlayer", attackDelayTime);
                    animator.SetTrigger("Attack");
                }
            }
        }

        void HitPlayer()
        {
            if (player != null && this.gameObject.activeInHierarchy)
            {
                if (Vector3.Distance(this.transform.position, player.transform.position) < 2.0f)
                {
                    player.setHP(-damage);
                }
            }
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

                    if(i > n - 1)
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
            nvAgent.SetDestination(destination);

            if (Task.isInspected)
                Task.current.debugInfo = string.Format("({0}. {1})", destination.x, destination.y);

            return true;
        }

        [Task]
        public bool SetDestination_Waypoint()
        {
            bool isSet = false;
            if(waypointPath != null)
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
            float d = Vector3.Distance(this.transform.position, destination);
            if (d < 5.0f)
                ret = true;

            if (Task.isInspected)
                Task.current.debugInfo = string.Format("d-{0:0.00}", d);
            return ret;
        }
        
       [Task]
       bool NextWaypoint()
        {
            if(waypointPath != null)
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
        bool CheckHP()
        {
            return self.checkHp();
        }

        [Task]
        void MakeRagdoll()
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
        void Blink()
        {
            this.GetComponentInChildren<Renderer>().enabled = !this.GetComponentInChildren<Renderer>().enabled;
            Task.current.Succeed();
        }

        [Task]
        void Die()
        {
            //self.Die();
            Task.current.Succeed();
        }

    }

}
/*
tree("Root")
	//Root behaviour for all enemy: Be alive or die.
	parallel
		repeat mute tree("BeAlive")
		repeat mute tree("Die")

tree("BeAlive")
	//This enemy attacks when possible,
	//otherwise it patrols a predefined path.
	fallback
		tree("Attack")
		tree("Patroll")

tree("Patroll")
	//While no enemy is spotted,
	//follow the assigned waypoints
	while
		sequence
			not HasEnemy
			not AcquireEnemy
		repeat
			sequence
				SetDestination_Waypoint
				SetTarget_Destination
				AimAt_Target
				MoveTo_Destination
				Wait(0.3)
				NextWaypoint


tree("Attack")
	//When the enemy is visible, attack it,
	//Otherwise forget about it.
	fallback

		//Attack the enemy if visible.
		repeat
			sequence
				canAttack
				Stop
				Wait(0.5)
				SetTarget_Enemy
				AimAt_Target
				tree("Fire")
		//Otherwise forget about it.
		sequence
			Clear_Enemy
			Fail


tree("Fire")
	//Fire if the unit has ammo.
	sequence
		Wait(0.3)
		Fire

tree("Die")
	//Die by exploding if no more HP.
	Sequence
		IsHealthLessThan(0.1)
		Explode



 */