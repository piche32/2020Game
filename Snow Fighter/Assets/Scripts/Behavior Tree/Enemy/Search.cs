using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using BBUnity.Actions;

[Action("Enemy/Search")]
[Help("Search Player")]

public class Search : GOAction
{
    private EnemyAIScript enemyAISc;
    int countRotate;
    LayerMask ignoreLayerMask;
    LayerMask layerMask;
    RaycastHit ray;

    Animator animator;
    
   public override void OnStart()
    {
        ignoreLayerMask = 0;
        enemyAISc = gameObject.GetComponent<EnemyAIScript>();
        foreach(LayerMask LM in enemyAISc.IgnoreLM)
        {
            ignoreLayerMask |= 1 << LM;
        }
        layerMask = -1 - ignoreLayerMask;
        animator = gameObject.GetComponent<Animator>();

        countRotate = 0;

        animator.SetBool("isAlerting", true);
        animator.SetBool("isMoving", false);
        animator.SetTrigger("Alert");
        animator.applyRootMotion = true;

        if (enemyAISc.PreState == EnemyState.STATE_ATTACKING)
        {
            //enemyAISc.attackToIdle();
        }
        else if (enemyAISc.PreState == EnemyState.STATE_FOLLOWING)
        {
           //enemyAISc.followToIdle();
        }

        base.OnStart();
    }

    public override TaskStatus OnUpdate()
    {
        //한 바퀴 제자리에서 돌았는데.. 플레이어가 없으면 Idle로 가기

        Collider[] cols = Physics.OverlapSphere(gameObject.transform.position, enemyAISc.AlertingDist);

        if (cols.Length == 0) return TaskStatus.RUNNING;
        Debug.DrawRay(gameObject.transform.position, gameObject.transform.forward * enemyAISc.AlertingDist, Color.blue, 0.1f);
        if (Physics.Raycast(gameObject.transform.position, gameObject.transform.forward, out ray, enemyAISc.AlertingDist)) {
            if (!enemyAISc.WasObstacle) {
                enemyAISc.RotateSpeed = -enemyAISc.RotateSpeed;
            }
            enemyAISc.WasObstacle = true;
        }

        else enemyAISc.WasObstacle = false;

        if (Time.deltaTime * enemyAISc.RotateSpeed > 90.0f)
        {
            countRotate++;
            if(countRotate == 4) //한바퀴 다 돈 경우,
            {
                animator.SetBool("isAlerting", false);
                animator.SetBool("isMoving", true);
                animator.applyRootMotion = false;
                enemyAISc.setState(EnemyState.STATE_IDLE);
            }

           
        }
        animator.SetFloat("RotateY", Time.deltaTime * enemyAISc.RotateSpeed / 90.0f); //RotateY의 Max: 1, rotateSpeed의 단위 degree, 90도마다 초기화

        return TaskStatus.RUNNING;
        //return base.OnUpdate();
    }
}