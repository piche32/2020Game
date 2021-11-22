using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;

namespace Enemy.Ver2
{

    public class BossAI : EnemyAIBT
    {
        [Task]
        public bool isRoll;
        [Task]
        public bool isThrow;
        [Task]
        public bool isPunch;
        [Task]
        bool isAttack;
        [SerializeField] [Range(0.0f, 1.0f)] float probabilityOfRoll;
        [SerializeField] [Range(0.0f, 1.0f)] float probabilityOfThrow;
        [SerializeField] [Range(0.0f, 1.0f)] float probabilityOfPunch;

        [SerializeField] float rollDamage = 50.0f;
        [SerializeField] float throwDamage = 30.0f;
        [SerializeField] float punchDamage = 10.0f;
        [SerializeField] float normalDamage = 5.0f;


        public float ThrowStoppingDist;
        public float RollStoppingDist;

        float time;
        public float restTime = 5.0f;
        public float followTime = 5.0f;
        [Task]
        public bool isRest;

        public BossSnowBallPooling snowballs;
        BossSnowBall snowball;
        public Transform snowStart;

        public float rollPower;
        public float throwPower;
        public float punchPower;
       // public Transform sight;
        protected override void Start()
        {
            base.Start();
            isAttack = false;
            //ConsoleDebug.IsNull(this.name, "snowball", snowball);
        }

        void ResetActionBool()
        {
            isRoll = false;
            isThrow = false;
            isPunch = false;
            isRest = false;

            animator.SetBool("isRest", false);
        }

        [Task]
        void CheckAttack()
        {
            if (isAttack)
            {
                Task.current.Succeed();
                return;
            }

            isAttack = IsPlayerInSight("Attack");
            Task.current.Complete(isAttack);
        }

        [Task]
        void DecideHowToAttack()
        {
            ResetActionBool();
            float number = Random.Range(0.0f, 1.0f);
            if (number <= probabilityOfRoll)
            {
                isRoll = true;
            }
            else if (number <= probabilityOfRoll + probabilityOfThrow)
            {
                isThrow = true;
            }
            else
            {
                isPunch = true;
            }
            Task.current.Succeed();
        }

        [Task]
        void MakeASnowBall(string attackType)
        {
            snowball = snowballs.GetObject().GetComponent<BossSnowBall>();
            if(snowball == null)
            {
                Task.current.Fail();
                return;
            }
            if(attackType == "Roll")
            //손 위치에 만들기
                snowball.init(attackType, snowStart, rollDamage);
            if (attackType == "Throw")
                snowball.init(attackType, snowStart, throwDamage);
            Task.current.Succeed();
        }


        bool isRollSnow = false;
        [Task]
        void Roll()
        {
            //애니메이션 실행
            if (!animator.GetBool("isRoll"))
            {
               // if (!animator.GetCurrentAnimatorStateInfo(1).IsName("Roll")
              //      || (animator.GetCurrentAnimatorStateInfo(1).IsName("Roll") && animator.GetCurrentAnimatorStateInfo(1).normalizedTime < 1.0f))
               // {
                    animator.SetBool("isRoll", true);
                //    animator.SetTrigger("Roll");
                    //animator.applyRootMotion = false;
                    isRollSnow = false;
                    return;
              //  }
            }

            if (!isRollSnow && animator.GetCurrentAnimatorStateInfo(1).IsName("Roll")
                && animator.GetCurrentAnimatorStateInfo(1).normalizedTime >= 0.3f)
            {
                snowball.Roll(this.transform.forward, rollPower, destination);
                snowball = null;
                isRollSnow = true;
            }

            if (animator.GetBool("isRoll") && animator.GetCurrentAnimatorStateInfo(1).IsName("Roll") && animator.GetCurrentAnimatorStateInfo(1).normalizedTime >= 1.0f)
            {
                animator.SetBool("isRoll", false);
                //animator.applyRootMotion = true;

                isRoll = false;
                Task.current.Succeed();
                return;
            }
        }

        bool isThrowSnow = false;
        [Task]
        void Throw()
        {
            if (!animator.GetBool("isThrow"))
            {
                animator.SetBool("isThrow", true);
                isThrowSnow = false;
                //  Task.current.Succeed();
                return;
            }

            if(!isThrowSnow && animator.GetCurrentAnimatorStateInfo(1).IsName("Throw") &&
                animator.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.55f)
            {
                snowball.Throw(this.transform.forward, throwPower);
                snowball = null;
                isThrowSnow = true;
            }

            if (animator.GetCurrentAnimatorStateInfo(1).IsName("Throw") && animator.GetCurrentAnimatorStateInfo(1).normalizedTime >= 0.9f)
            {
                animator.SetBool("isThrow", false);
                isThrow = false;
                Task.current.Succeed();
                return;
            }
        }

        bool canPunch = false;
        [Task]
        void Punch()
        {
            if (!animator.GetBool("isPunch"))
            {
                animator.SetBool("isPunch", true);
                canPunch = true;
                //    Task.current.Succeed();
                return;
            }
            if (canPunch && animator.GetCurrentAnimatorStateInfo(1).IsName("Punch") &&
                animator.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.30f)
            {
                canPunch = false;
                if (Vector3.Distance(player.transform.position, this.transform.position) <= attackStoppingDist + 0.5f)
                {
                    player.damaged(-punchDamage);
                    //플레이어 밀려나기
                    Vector3 dir = player.transform.position - this.transform.position;
                    //dir.y = player.transform.position.y;
                    dir = dir.normalized;
                    player.punched(dir, punchPower);
                }
            }
            if (animator.GetCurrentAnimatorStateInfo(1).IsName("Punch") && animator.GetCurrentAnimatorStateInfo(1).normalizedTime >= 0.9f)
            {
                animator.SetBool("isPunch", false);
                isPunch = false;
                Task.current.Succeed();
            }
        }


        [Task]
        void SetTargetPosition()
        {
            nvAgent.isStopped = false;
            nvAgent.stoppingDistance = defaultStoppingDist;
            SetDestination(player.transform.position);
        }


        /// <summary>
        /// 타겟 쪽으로 살짝 몸을 트는 함수
        /// 타겟이 가까이 있을 땐 집요하게 몸을 튼다.
        /// </summary>
        [Task]
        public override void AimAt_Target()
        {
            if (player != null)
            {
                transform.LookAt(player.transform.position);
                Task.current.Succeed();
            }
        }

        [Task]
        void TakeARest()
        {
            if (!animator.GetBool("isRest"))
            {
                isRest = true;
                nvAgent.isStopped = true;
                animator.SetBool("isRest", true);
                Task.current.Succeed();
                return;
            }
            if (animator.GetCurrentAnimatorStateInfo(1).IsName("Rest") && animator.GetCurrentAnimatorStateInfo(1).normalizedTime >= 0.9f)
            {
                isRest = false;
                nvAgent.isStopped = false;
                NextWaypoint();
                animator.SetBool("isRest", false);
                Task.current.Succeed();
                return;
            }
            Task.current.Succeed();
        }

        [Task]
        public bool SetDestination_Player(string AttackType)
        {
            nvAgent.isStopped = false;
            bool ret = false;

            switch (AttackType)
            {
                case "Roll":
                    nvAgent.stoppingDistance = RollStoppingDist;
                    break;

                case "Throw":
                    nvAgent.stoppingDistance = ThrowStoppingDist;
                    break;
                default:
                    nvAgent.stoppingDistance = attackStoppingDist;
                    break;
            }

            ret = SetDestination(player.transform.position);
            return ret;
        }

        bool isFollow = false;
        [Task]
        void FollowPlayer()
        {
            SetDestination_Player();
            if (isArrived())
            {
                isFollow = false;
                Task.current.Succeed();
                return;
            }
            if (!isFollow)
            {
                time = 0.0f;
                isFollow = true;
                return;
            }
            time += Time.deltaTime;
            if(time > followTime)
            {
                isFollow = false;
                Task.current.Succeed();
                return;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.tag == "Player")
            {
                if(!isPunch)
                    player.damaged(-normalDamage);
                isAttack = true;
            }
            if(collision.gameObject.layer == LayerMask.NameToLayer("PlayerSnowBall"))
            {
                isAttack = true;
            }

        }
    }
}
