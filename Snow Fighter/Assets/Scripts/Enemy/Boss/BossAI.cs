using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;

namespace Enemy.Ver2
{

    enum KindOfAttacks
    {
        Roll = 0,
        Throw,
        Punch,
        Num
    }

    public class BossAI : EnemyAIBT
    {
        [Task]
        public bool isRoll;
        [Task]
        public bool isThrow;
        [Task]
        public bool isPunch;

        [Range(0.0f, 1.0f)] public float probabilityOfRoll;
        [Range(0.0f, 1.0f)] public float probabilityOfThrow;
        [Range(0.0f, 1.0f)] public float probabilityOfPunch;

        protected override void Start()
        {
            base.Start();    
        }

        void ResetActionBool()
        {
            isRoll = false;
            isThrow = false;
            isPunch = false;
        }

        [Task]
        void DecideHowToAttack()
        {
            ResetActionBool();
            float number = Random.Range(0.0f, 1.0f);
            if(number <= probabilityOfRoll)
            {
                isRoll = true;
            }
            else if(number <= probabilityOfRoll + probabilityOfThrow)
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
        void Roll()
        {
            if (!animator.GetBool("isRoll"))
            {
                animator.SetBool("isRoll", true);
              //  Task.current.Succeed();
                return;
            }

            if(animator.GetCurrentAnimatorStateInfo(1).IsName("Roll") && animator.GetCurrentAnimatorStateInfo(1).normalizedTime >= 0.9f)
            {
                animator.SetBool("isRoll", false);
                isRoll = false;
                Task.current.Succeed();
            }
        }
        
         [Task]
        void Throw()
        {
            if (!animator.GetBool("isThrow"))
            {
                animator.SetBool("isThrow", true);
              //  Task.current.Succeed();
                return;
            }

            if(animator.GetCurrentAnimatorStateInfo(1).IsName("Throw") && animator.GetCurrentAnimatorStateInfo(1).normalizedTime >= 0.9f)
            {
                animator.SetBool("isThrow", false);
                isThrow = false;
                Task.current.Succeed();
            }
        }

        [Task]
        void Punch()
        {
            if (!animator.GetBool("isPunch"))
            {
                animator.SetBool("isPunch", true);
            //    Task.current.Succeed();
                return;
            }

            if(animator.GetCurrentAnimatorStateInfo(1).IsName("Punch") && animator.GetCurrentAnimatorStateInfo(1).normalizedTime >= 0.9f)
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
    }
}
