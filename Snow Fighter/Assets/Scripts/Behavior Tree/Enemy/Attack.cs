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

    private EnemyAIScript enemyAISc;

    [InParam("attackCoolTime")]
    public float attackCoolTime;

    private float attackTime;


    public override void OnStart()
    {
        attackTime = attackCoolTime;
        if(player == null)
        { 
            player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
                Debug.LogWarning("Player not specified. Attack will not work for" + gameObject.name);
        }

        enemyAISc = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyAIScript>();

        base.OnStart();
    }

    public override TaskStatus OnUpdate()
    {
        if (player == null) return TaskStatus.FAILED;

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

        attackTime += Time.deltaTime;
        enemyAISc.NvAgent.SetDestination(player.transform.position); //플레이어 위치 업데이트 해주기

        if (enemyAISc.Animator.GetBool("IsReadyToThrow") == false) {
            enemyAISc.Animator.SetBool("IsReadyToThrow", true);
        }
        else {
            if (attackCoolTime > attackTime)
                return TaskStatus.RUNNING; //쿨타임 남음
            if (!enemyAISc.isTarget(player.transform))
                return TaskStatus.RUNNING; //장애물 유무 확인
            enemyAISc.Animator.SetTrigger("Throw");
            attackTime = 0.0f;
        }
        //enemyAISc.attack();
        return TaskStatus.RUNNING;

    } 
}
