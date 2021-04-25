using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;

namespace Panda
{
    public class EnemyAIPandaBT : MonoBehaviour
    {
        // Start is called before the first frame update

        EnemyAIScript self;

        GameObject player;

        [SerializeField] float sightAngle = 120.0f;
        [SerializeField] float attackingDist = 10.0f;
        [SerializeField] float followingDist = 20.0f;

        //공격 쿨타임
        float attackCoolTime;
        float attackTime;

        //어드 정도 따라가다가 못 따라 잡으면 포기
        float followLimitTime;
        float followTime;



        void Start()
        {
            self = gameObject.GetComponent<EnemyAIScript>();
            if(self == null)
            {
                Debug.LogError("There's no EnemyAIScript in this " + this.gameObject.name.ToString());
                return;
            }

            player = GameObject.FindWithTag("Player");
            if(player == null)
            {
                Debug.LogError("Can not find Player!");
                return;
            }

            //공격 쿨타임 초기화
            attackCoolTime = self.AttackCoolTime;
            //처음에 들어 왔을 때 공격 바로 할 수 있게 attackCoolTime으로 초기화
            attackTime = attackCoolTime;

            followLimitTime = self.FollowLimitTime;
        }

        #region die
        [Task]
        bool isDied()
        {
            return self.checkHp();
        }

        [Task]
        void died()
        {
            self.died();
            if (self.IsDied) Task.current.Complete(true); //Complete
            GetComponent<PandaBehaviour>().enabled = false;
            //Running
        }
        #endregion die

        #region attack
        /// <summary>
        /// Check whether player is close depending on a given attacking distance and in enemy's sight.
        /// </summary>
        /// <returns></returns>
        [Task]
         bool isPlayerInAttackingSight()
        {
            return Panda.Conditions.isTargetInSight(gameObject, player, attackingDist, sightAngle);
        }

        //쿨타임 끝났으면 true
        [Task]
        bool isTimeToAttack()
        {
            if (attackCoolTime > attackTime)
            {
                return false;
            }
            return true;
        }

        [Task]
        void takeAttackCoolTime()
        {
            
            //쿨타임 업데이트
            attackTime += Time.deltaTime;
            //플레이어 따라가기
            self.NvAgent.SetDestination(player.transform.position);
            if (attackCoolTime <= attackTime) Task.current.Succeed();
        }

        //앞에 장애물 있는지
        [Task]
        void isThereTarget()
        {
            if (self.isTarget(player.transform)) // 앞에 장애물 없을 때
                Task.current.Succeed();
        }

        [Task]
        void attack()
        {
            //초기값
            if (self.CurState == EnemyState.STATE_NONE) self.setState(EnemyState.STATE_ATTACKING);

            //다른 상태 => Attack으로 변화
            if (self.CurState != EnemyState.STATE_ATTACKING)
            {
                //attackTime = 0;
                self.setState(EnemyState.STATE_ATTACKING);
                //Task.current.Succeed();
               // return;
            }

           
            //플레이어 위치 업데이트
            self.NvAgent.SetDestination(player.transform.position);


            //self.Animator.SetTrigger("Throw");
            //준비 애니메이션 켜주기
            if(self.Animator.GetBool("IsReadyToThrow") == false)
            {
                //나중에 던지는 애니메이션으로 넘어가게 하는 장치 혹은 던지기 취소되었을 때 빠져나가는 장치
                self.Animator.SetBool("IsReadyToThrow", true);
                self.Animator.SetTrigger("ReadyToThrow");
                return;
            }
            else
            {
                self.Animator.SetTrigger("Throw");
                attackTime = 0.0f;
            }
            
            Task.current.Succeed();
        }

        #endregion attack

        /// <summary>
        /// Check whether player is close depending on a given following distance and in enemy's sight.
        /// </summary>
        [Task]
        bool isPlayerInFollowingSight()
        {
            return Panda.Conditions.isTargetInSight(gameObject, player, followingDist, sightAngle);
        }


        [Task]
        void follow()
        {
            //초기값
            if (self.CurState == EnemyState.STATE_NONE) self.setState(EnemyState.STATE_FOLLOWING);
            if (self.CurState != EnemyState.STATE_FOLLOWING) self.setState(EnemyState.STATE_FOLLOWING);

            if (isFollowingTimeOver())
            {
                Task.current.Succeed();
                return;
            }

            //따라다니는 시간
            followTime += Time.deltaTime;

            self.NvAgent.SetDestination(player.transform.position);
            Task.current.Succeed();

            return;
        }

        bool isFollowingTimeOver()
        {
            if(followTime > followLimitTime)
            {
                followTime = 0;
                return true;
            }
            return false;
        }

        [Task]
        void idle()
        {
            //초기값
            if (self.CurState == EnemyState.STATE_NONE) self.setState(EnemyState.STATE_IDLE);

            //다른 상태에서 idle로 바뀌었을 경우
            if(self.CurState != EnemyState.STATE_IDLE)
            {
                self.otherToIdle();
                if (self.CurState == EnemyState.STATE_ATTACKING)
                    self.attackToOther();

                self.setState(EnemyState.STATE_IDLE);
            }
            self.idle();
            Task.current.Succeed();
            return;
        }
    }
}
