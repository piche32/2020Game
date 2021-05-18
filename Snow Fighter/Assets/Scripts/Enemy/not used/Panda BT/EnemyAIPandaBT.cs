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

        float sightAngle;
         float attackingDist;
        float followingDist;

        //공격 쿨타임
        float attackCoolTime;
        float attackTime;

        //어느 정도 따라가다가 못 따라 잡으면 포기
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

            sightAngle = self.SightAngle;
            attackingDist = self.AttackingDist;
            followingDist = self.FollowingDist;
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

        [Task]
        bool isAttacking()
        {
            AnimatorStateInfo aniInfo = self.Animator.GetCurrentAnimatorStateInfo(1);
            if(aniInfo.fullPathHash == Animator.StringToHash("Upper Layer.Throw"))
            {
                if(aniInfo.normalizedTime <= 1.0f) {
                    Task.current.Succeed();
                    return true;
                }
            }
            Task.current.Fail();
            return false;
        }

        /// <summary>
        /// Check whether player is close depending on a given attacking distance and in enemy's sight.
        /// </summary>
        /// <returns></returns>
        [Task]
         bool isPlayerInAttackingSight()
        {
            return Panda.Conditions.isTargetInSight(gameObject, player, attackingDist, sightAngle);
        }

        [Task]
        void attackInit()
        {
            self.Animator.SetBool("isAlerting", false);
            self.NvAgent.stoppingDistance = 7.0f;
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


            Task.current.Succeed();
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
            else
                Task.current.Fail();
        }

        //지금 던지는 중인지
        [Task]
        bool isThrowing()
        {
            AnimatorStateInfo aniInfo = self.Animator.GetCurrentAnimatorStateInfo(1);
            if (aniInfo.fullPathHash == Animator.StringToHash("Upper Layer.Throw"))
            {
                if (aniInfo.normalizedTime >= 1.0f) {
                    //던지는 애니메이션 거의 끝나갈 때,
                    self.Animator.SetBool("isThrowing", false);
                    return false; 
                }
                return true;
            }
            else {
                self.Animator.SetBool("isThrowing", false);
                return false;
            }
        }

        [Task]
        void readyToThrow()
        {
            attackTime = 0;
            if (self.Animator.GetBool("IsReadyToThrow") == true) return;
            if (self.PreState == EnemyState.STATE_NONE || self.CurState != EnemyState.STATE_ATTACKING) //처음 들어왔을 때
            {
                self.setState(EnemyState.STATE_ATTACKING);
            }
            
            if(self.CurState != EnemyState.STATE_ATTACKING)
            {
                self.setState(EnemyState.STATE_ATTACKING);
            }

            self.Animator.SetBool("IsReadyToThrow", true);
            self.Animator.SetTrigger("Throw");

            Task.current.Succeed();
        }

        [Task]
        void createSnow()
        {            
            self.createSnow();
            Task.current.Succeed();
        }

        [Task]
        void throwSnow()
        {
            self.throwSnow();
            Task.current.Succeed();
        }

        [Task]
        void throwEnemySnow()
        {
            self.throwEnemySnow();
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

            if (!self.Animator.GetBool("isThrowing"))
            {
                self.Animator.SetTrigger("Throw");
                self.Animator.SetBool("isThrowing", true);
                attackTime = 0.0f;
            }
            else
                return;


            //준비 애니메이션 켜주기
            //if(self.Animator.GetBool("IsReadyToThrow") == false)
            //{
            //    //나중에 던지는 애니메이션으로 넘어가게 하는 장치 혹은 던지기 취소되었을 때 빠져나가는 장치
            //    self.Animator.SetBool("IsReadyToThrow", true);
            //    self.Animator.SetTrigger("ReadyToThrow");
            //    return;
            //}
            //else
            //{
            //    /*if (!self.Animator.GetCurrentAnimatorStateInfo(1).IsName("ThrowReady")) //이미 던지는 중..
            //    {
            //        Task.current.Succeed();
            //    }*/
            //    self.Animator.SetTrigger("Throw");
            //    attackTime = 0.0f;
            //}*/
            
            Task.current.Succeed();
        }


        #endregion attack

        #region follow


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
            if (self.CurState == EnemyState.STATE_NONE) self.setState(EnemyState.STATE_FOLLOWING);
            if (self.CurState != EnemyState.STATE_FOLLOWING) {
                if (self.CurState == EnemyState.STATE_ATTACKING) self.attackToOther();
                if (self.CurState == EnemyState.STATE_IDLE) self.IdleToOther();
                self.setState(EnemyState.STATE_FOLLOWING); 
            }

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
            if (followTime > followLimitTime)
            {
                followTime = 0;
                return true;
            }
            return false;
        }

        #endregion follow

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
