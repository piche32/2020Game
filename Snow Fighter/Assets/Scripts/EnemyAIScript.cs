using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    STATE_IDLE,
    STATE_FOLLOWING,
    STATE_ATTACKING
}

public class EnemyAIScript : MonoBehaviour
{
    EnemyState curState;
    EnemyState preState;


    Transform playerTrans;
    [SerializeField]Transform snowStartTrans = null;

    [SerializeField] GameObject snowball = null;

    [SerializeField] LayerMask snowballLM;
    [SerializeField] LayerMask playerLM;
    [SerializeField] LayerMask snowStartLM;
    [SerializeField] LayerMask enemyLM;


    [SerializeField] float followingDist = 10.0f;
    [SerializeField] float attackingDist = 5.0f;
    [SerializeField] float alertingDist = 20.0f;
//  [SerializeField] float dodgingDist = 20.0f;

    [SerializeField] float speed = 1.0f;
    [SerializeField] float rotateSpeed = 1.0f;
    [SerializeField] float sightAngle = 60.0f;
    [SerializeField] float attackCoolTime = 10.0f;
    [SerializeField] float followLimitTime = 30.0f;
    [SerializeField] float alertLimitTime = 3.0f;
    
    float attackTime;
    float followTime;
    float alertTime;

    Vector3 startPoint;
    Vector3 snowStartPt;
    
    NavMeshAgent nvAgent;

    RaycastHit ray;
    
    bool wasObstacle;


    [SerializeField] List<Transform> wayPoints = new List<Transform>();
    Transform targetWayPoint;
    int targetWayPointIndex;

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        curState = EnemyState.STATE_IDLE;
        preState = curState;

        playerTrans = GameObject.FindWithTag("Player").transform;
        if(playerTrans == null)
        {
            Debug.Log("ERROR_WITH_PLAYER");
            return;
        }

        attackTime = 0.0f;
        followTime = 0.0f;
        alertTime = 0.0f;

        startPoint = gameObject.transform.position; //실행 시, 현재 서 있는 지점을 StartPoint로 두기
        snowStartPt = snowStartTrans.position;

        targetWayPointIndex = 0;
        targetWayPoint = wayPoints[targetWayPointIndex];

        nvAgent = GetComponent<NavMeshAgent>();
        nvAgent.enabled = true;
        nvAgent.stoppingDistance = 2;
        nvAgent.speed = speed;
        nvAgent.SetDestination(targetWayPoint.position);

        wasObstacle = false;

        animator = GetComponent<Animator>();
        animator.SetBool("isAlerting", false);
        animator.applyRootMotion = false;
    }

    // Update is called once per frame
    void Update()
    {
        checkState();
        attackTime += Time.deltaTime; //attack state가 아닐 때도 시간을 계산(attack cool time을 state 변화로 초기화 시켜 무한히 공격하는 것을 막기 위함)
    } //Update 함수 괄호 삭제X

    void checkState()
    {
        switch (curState)
        {
            case EnemyState.STATE_IDLE:
                idle();
                break;

            case EnemyState.STATE_FOLLOWING:
                follow();
                break;

            case EnemyState.STATE_ATTACKING:
                attack();
                break;
        }
    }

    public bool isTargetInSight()
    {
        Vector3 dir = (playerTrans.position - transform.position).normalized;
        float angle = Vector3.Angle(dir, transform.forward);

        if (angle < sightAngle * 0.5f)
        {
            if (isTarget(playerTrans)) //Player와 Enemy 사이에 장애물 확인
            { return true; }
        }
        return false;
    }
    bool isTarget(Transform targetTrans) //enemy와 target 사이에 다른 물체가 있는지
    {
        Vector3 dir = (targetTrans.position - transform.position).normalized;
        float maxDist = Vector3.Distance(targetTrans.position, transform.position);
        Debug.DrawRay(transform.position + transform.up * 0.5f, transform.forward * (maxDist + 1.0f), Color.blue, 0.1f);
        if (Physics.Raycast(transform.position + transform.up * 0.5f, dir, out ray, maxDist + 1.0f, -1 - snowballLM - snowStartLM - enemyLM))
        {
            return ray.transform.CompareTag(targetTrans.tag);
        }

        Debug.Log("Raycast error");
        return false;
    }
    void goBackToStartPt()
    {
        nvAgent.enabled = true;
        nvAgent.SetDestination(startPoint);
        nvAgent.stoppingDistance = 2;
        nvAgent.speed = speed;
    }
    void alert()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, alertingDist, -1 - snowballLM - snowStartLM);

        if (cols.Length == 0) return;
        Debug.DrawRay(transform.position, transform.forward * alertingDist, Color.blue, 0.1f);
        if (Physics.Raycast(transform.position, transform.forward, out ray, alertingDist, -1 - snowballLM - snowStartLM))
        {
            if (!wasObstacle)
            {
                rotateSpeed = -rotateSpeed;
            }
            wasObstacle = true;
        }
        else wasObstacle = false;
        animator.SetFloat("RotateY", Time.deltaTime*rotateSpeed/90.0f); //RotateY의 Max: 1, rotateSpeed의 단위 degree
        //transform.Rotate(0.0f, Time.deltaTime * rotateSpeed, 0.0f);

        if(alertTime > alertLimitTime)
        {
            animator.SetBool("isAlerting", false);
            animator.applyRootMotion = false;

        }
    }

    void patrol()
    {
        
        if (Vector3.Distance(transform.position, targetWayPoint.position) <= nvAgent.stoppingDistance+0.8f)
        {
            if (alertTime == 0.0f) // 목표지점 도착 직후
            {
                animator.SetBool("isAlerting", true); //경계모드 활성화
                animator.SetTrigger("Alert");
                animator.applyRootMotion = true;

                alertTime += Time.deltaTime;
                return;
            }
            if (animator.GetBool("isAlerting"))
            {
                alert();
                alertTime += Time.deltaTime;
                return;
            }
            
            targetWayPointIndex++;
            if (targetWayPointIndex >= wayPoints.Count)
            {
                targetWayPointIndex = 0;
            }
            targetWayPoint = wayPoints[targetWayPointIndex];
            nvAgent.SetDestination(targetWayPoint.position);
            alertTime = 0.0f;
        }

        return;
    }
    void idle()
    {
        if (preState == EnemyState.STATE_FOLLOWING)
        { //follow -> idle fn
            followToIdle();
            return;
        }

        patrol();
    }
    void follow()
    {
        if(preState == EnemyState.STATE_IDLE) //idle -> follow fn
        {
            idleToFollow();
            return;
        }

        if (isFollowingTimeOver()) //일정시간동안 공격범위에 들어가지 못하고 따라다니기만 했을 경우 Idle 상태로 돌아가기
        {
            setState(EnemyState.STATE_IDLE);
            return;
        }

        followTime += Time.deltaTime;

        nvAgent.SetDestination(playerTrans.position);
    }
    void attack()
    {
        nvAgent.SetDestination(playerTrans.position);

        if (attackCoolTime > attackTime) return; //쿨타임 남음

        if (!isTarget(playerTrans)) return; //장애물 유무 확인

        animator.SetTrigger("Throw");
        snowStartPt = snowStartTrans.position;

        Vector3 snowballPos = snowStartPt;
        snowballPos += (transform.rotation * Vector3.forward);

        Instantiate(snowball, snowballPos, transform.rotation);
        attackTime = 0.0f;
    }
    void followToIdle()
    {
        nvAgent.SetDestination(targetWayPoint.position);
        alertTime = 0.0f;
        preState = curState;
    }
    void idleToFollow()
    {
        followTime = 0.0f;
        preState = curState;
    }
    void attackToFollow()
    {
        preState = curState;
    }
    void followToAttack()
    {
        preState = curState;
    }

    Vector3 DirFromAngle(float angleInDegrees)
    {
        angleInDegrees += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 leftBoundary = DirFromAngle(-sightAngle / 2);
        Vector3 rightBoundary = DirFromAngle(sightAngle / 2);
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary * followingDist);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary * followingDist);
        Gizmos.DrawWireSphere(transform.position, followingDist);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackingDist);
    }

    public void setState(EnemyState state)
    {
        preState = curState;
        curState = state;
    }
    public EnemyState getCurState()
    {
        return curState;
    }
    public bool isFollowingTimeOver()
    {
        if (followTime > followLimitTime) return true;
        return false;
    }

} //스크립트 클래스 괄호 삭제X
