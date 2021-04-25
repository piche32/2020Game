using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using BBUnity.Actions;

[Action("Enemy/Follow")]
[Help("Follow player")]
public class Follow : GOAction
{
    [InParam("player")]
    public GameObject player;

    private EnemyAIScript enemyAISc;

    float followLimitTime;
    float followTime;
    

    public override void OnStart()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player == null) Debug.LogWarning("Player not specified. Attack will not work for" + gameObject.name);
        }

        enemyAISc = gameObject.GetComponent<EnemyAIScript>();
        


        followLimitTime = enemyAISc.FollowLimitTime;
        
        base.OnStart();
    }

    public override TaskStatus OnUpdate()
    {
        if (player == null) return TaskStatus.FAILED;
        //enemyAISc.follow();

        if (enemyAISc.CurState != EnemyState.STATE_FOLLOWING)
        {
            followTime = 0.0f;
            if (enemyAISc.CurState == EnemyState.STATE_ATTACKING)
                enemyAISc.attackToOther();

            if (enemyAISc.CurState == EnemyState.STATE_IDLE)
                enemyAISc.IdleToOther();

            enemyAISc.setState(EnemyState.STATE_FOLLOWING); 
            return TaskStatus.RUNNING;

        }

        /*
        if (enemyAISc.PreState == EnemyState.STATE_IDLE) { //idle -> follow fn
            enemyAISc.idleToFollow();
            return TaskStatus.RUNNING;

        }
        if (enemyAISc.PreState == EnemyState.STATE_ATTACKING) {
            enemyAISc.attackToFollow();
            return TaskStatus.RUNNING;
        }
        */

        if (isFollowingTimeOver()) { //일정 시간 동안 공격범위에 들어가지 못하고 따라다니기만 했을 경우 Idle 상태로 돌아가기
            return TaskStatus.RUNNING;

        }

        //따라다니는 시간
        followTime += Time.deltaTime;

        enemyAISc.NvAgent.SetDestination(player.transform.position);

        return TaskStatus.RUNNING;

    }

    bool isFollowingTimeOver()
    {
        if (followTime > followLimitTime) {
            followTime = 0;
            return true;
        }
        return false;
    }
}
