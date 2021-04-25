using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using BBUnity.Actions;

[Action("Enemy/Attack")]
[Help("Attack player")]

public class Attack : GOAction
{
    [InParam("player")]
    public GameObject player;

    EnemyAIScript enemyAISc;

    float attackCoolTime;
    float attackTime;

    //Player 따라가는 거리
    [InParam("StoppingDist")]
    public float stoppingDist;

    public override void OnStart()
    {
        if(player == null)
        { 
            player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
                Debug.LogWarning("Player not specified. Attack will not work for" + gameObject.name);
        }
        enemyAISc = gameObject.GetComponent<EnemyAIScript>();


        //공격 쿨타임 초기화
        attackCoolTime = enemyAISc.AttackCoolTime;
        //처음에 들어 왔을 때 공격 한 번 할 수 있도록 하기 
        attackTime = attackCoolTime;

        //StoppingDist 다시 정해주기
        if (stoppingDist == 0) stoppingDist = 10.0f;
        enemyAISc.NvAgent.stoppingDistance = stoppingDist;

        base.OnStart();
    }

    public override TaskStatus OnUpdate()
    {
        if (player == null) return TaskStatus.FAILED;


        //다른 상태에서 Attack으로 변화 했을 때,
        if (enemyAISc.CurState != EnemyState.STATE_ATTACKING)
        {
            attackTime = attackCoolTime;

            //Enemy 상태 업데이트
            enemyAISc.setState(EnemyState.STATE_ATTACKING);
           // return TaskStatus.RUNNING;
        }

        /*
        if (enemyAISc.PreState == EnemyState.STATE_FOLLOWING) { 
            enemyAISc.followToAttack();
            attackTime = 0f;
            return TaskStatus.RUNNING;
        }
        if (enemyAISc.PreState == EnemyState.STATE_IDLE) { //Idle=>Attack
            enemyAISc.idleToAttack();
            attackTime = 0f;
            return TaskStatus.RUNNING;
        }
        */

        //쿨타임 증가
        attackTime += Time.deltaTime;

        //플레이어 위치 업데이트 해주기
        enemyAISc.NvAgent.SetDestination(player.transform.position);

        if (attackCoolTime > attackTime)
            return TaskStatus.RUNNING; //쿨타임 남음


        //쿨타임 끝! 공격!!
        if (!enemyAISc.isTarget(player.transform)) // 앞에 장애물이 있다
            return TaskStatus.RUNNING;

        //준비 애니메이션 켜주기
        if (enemyAISc.Animator.GetBool("IsReadyToThrow") == false) {
            //나중에 던지는 애니메이션으로 넘어가게 하는 장치 혹은 던지기 취소 되었을 때 빠져나가는 장시
            enemyAISc.Animator.SetBool("IsReadyToThrow", true);
            enemyAISc.Animator.SetTrigger("ReadyToThrow");
        }

        else {
            enemyAISc.Animator.SetTrigger("Throw");
            attackTime = 0.0f;
        }
        //enemyAISc.attack();
        return TaskStatus.RUNNING;


    }
}
