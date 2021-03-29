﻿using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public enum EnemyState
{
    STATE_IDLE,
    STATE_FOLLOWING,
    STATE_ATTACKING,
    STATE_DIE,
}

public class EnemyAIScript : MonoBehaviour
{
    EnemyState curState;
    EnemyState preState;
    public EnemyState PreState { get { return preState; } }


    Transform playerTrans;
    [SerializeField] Transform snowStartTrans = null;
    SnowBallScript snow;

    [SerializeField] LayerMask snowballLM;
    [SerializeField] LayerMask playerLM;
    [SerializeField] LayerMask snowStartLM;
    [SerializeField] LayerMask enemyLM;

    [SerializeField] float followingDist = 10.0f;
    public float FollowingDist { get { return followingDist; } }
    [SerializeField] float attackingDist = 5.0f;
    public float AttackingDist { get { return attackingDist; } }
    [SerializeField] float alertingDist = 20.0f;
    //  [SerializeField] float dodgingDist = 20.0f;

    [SerializeField] float speed = 1.0f;
    [SerializeField] float rotateSpeed = 1.0f;
    [SerializeField] float sightAngle = 60.0f;
    public float SightAngle { get { return sightAngle; } }
    [SerializeField] float attackCoolTime = 10.0f;
    [SerializeField] float followLimitTime = 30.0f;
    [SerializeField] float alertLimitTime = 3.0f;
    [SerializeField] float power = 20.0f;
    [SerializeField] float maxHP = 100.0f;

    private float hp;
    public float Hp { get { return hp; } set { hp = value; } }
    [SerializeField]
    Slider hpSlider;
    public Slider HpSlider { get { return hpSlider; } set { hpSlider = value; } }



    float attackTime;
    float followTime;
    public float FollowTime { get { return followTime; } set { followTime = value; } }
    float alertTime;

    NavMeshAgent nvAgent;
    public NavMeshAgent NvAgent{get{return nvAgent; }}

    RaycastHit ray;
    
    bool wasObstacle;


    [SerializeField] List<Transform> wayPoints = new List<Transform>();
    Transform targetWayPoint;
    int targetWayPointIndex;

    Animator animator;
    public Animator Animator { get { return animator; } }

    int blinkCount;
    bool isDied;
    public bool IsDied { get { return isDied; } }

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


        targetWayPointIndex = 0;
        targetWayPoint = wayPoints[targetWayPointIndex];
        nvAgent = GetComponent<NavMeshAgent>();
        nvAgent.enabled = true;
        nvAgent.stoppingDistance = 4;
        nvAgent.speed = speed;
        nvAgent.SetDestination(targetWayPoint.position);

        wasObstacle = false;

        animator = GetComponent<Animator>();
        animator.SetBool("isAlerting", false);
        animator.SetBool("isMoving", true);
        animator.applyRootMotion = false;
        animator.SetBool("IsReadyToThrow", false);

        snow = null;

        hp = maxHP;

        hpSlider.GetComponent<EnemyHPScript>().InitEnemyHPSlider(this.transform, maxHP);
        blinkCount = 0;

        isDied = false;

    }

    // Update is called once per frame
    void Update()
    {
        //checkState();
        attackTime += Time.deltaTime; //attack state가 아닐 때도 시간을 계산(attack cool time을 state 변화로 초기화 시켜 무한히 공격하는 것을 막기 위함)
    } //Update 함수 괄호 삭제X

    void checkState()
    {
       /* switch (curState)
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

            case EnemyState.STATE_DIE:
                died();
                break;
        }*/
    }

    public void died()
    {
        if (preState != EnemyState.STATE_DIE)
        {
            preState = EnemyState.STATE_DIE;
            //animator.SetBool("IsReadyToThrow", false);
            //animator.SetBool("isMoving", false);
            //animator.SetBool("isAlerting", false);
            animator.SetTrigger("Dying");
            animator.SetBool("IsDying", true);
            //nvAgent.enabled = false;
            Collider[] cors = this.GetComponentsInChildren<Collider>();
            foreach (Collider cor in cors)
            {
                cor.enabled = false;
            }
        }
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f)
        {
            //if (this.enabled == true)
           // {
                //this.enabled = false;
                //GetComponent<BehaviorExecutor>().enabled = false;
                StartCoroutine("Blink");
          //  }

        }
        

    }

    IEnumerator Blink()
    {
        while (blinkCount < 30)
        {
            blinkCount += 1;
            this.GetComponentInChildren<Renderer>().enabled = !this.GetComponentInChildren<Renderer>().enabled;
            yield return new WaitForSeconds(0.1f);
        }
        if (blinkCount >= 30) {
            if (this.GetComponentInChildren<SnowBallScript>())
                SnowBallPoolingScript.Instance.ReturnObject(this.GetComponentInChildren<SnowBallScript>());
            this.GetComponentInChildren<Renderer>().enabled = false;
            GetComponent<BehaviorExecutor>().enabled = false;
            isDied = true;
            //  this.enabled = false;

        }
        yield return null;


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
    public bool isTarget(Transform targetTrans) //enemy와 target 사이에 다른 물체가 있는지
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
    
    public void alert()
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
            animator.SetBool("isMoving", true);
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
                animator.SetBool("isMoving", false);
                animator.SetTrigger("Alert");
               // Debug.Log("Alert");
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
    public void idle()
    {
         if (preState == EnemyState.STATE_ATTACKING)
        {
            attackToIdle();
            return;
        }
        if (preState == EnemyState.STATE_FOLLOWING)
        { //follow -> idle fn
            followToIdle();
            return;
        }

        if (this.GetComponentInChildren<SnowBallScript>())
            SnowBallPoolingScript.Instance.ReturnObject(this.GetComponentInChildren<SnowBallScript>());


        patrol();
    }
    public void follow()
    {
        if(preState == EnemyState.STATE_IDLE) //idle -> follow fn
        {
            idleToFollow();
            return;
        }
        if (preState == EnemyState.STATE_ATTACKING)
        {
            attackToFollow();
            return;
        }
        if (isFollowingTimeOver()) //일정시간동안 공격범위에 들어가지 못하고 따라다니기만 했을 경우 Idle 상태로 돌아가기
        {
            setState(EnemyState.STATE_IDLE);
            
        }

        followTime += Time.deltaTime;

        nvAgent.SetDestination(playerTrans.position);
    }

    public void createSnow()
    {
       snow = SnowBallPoolingScript.Instance.GetObject();
       snow.gameObject.transform.SetParent(snowStartTrans);
        snow.gameObject.transform.position = snowStartTrans.position;
        snow.gameObject.transform.rotation = snowStartTrans.rotation;
    }

    public void throwSnow()
    {
        snow = GetComponentInChildren <SnowBallScript>();
        if (snow == null) return;
        snow.gameObject.transform.SetParent(null);
        snow.GetComponent<SnowBallScript>().IsFired = true;
        snow.GetComponent<SnowBallScript>().Initialize( power, snowStartTrans.position, snowStartTrans.rotation, transform , playerTrans);
        animator.SetBool("IsReadyToThrow", false);
    }

    public void attack()
    {
        if (preState == EnemyState.STATE_FOLLOWING)
        {
            followToAttack();
            return;
        }
        if(preState == EnemyState.STATE_IDLE) //Idle=>Attack
        {
            preState = EnemyState.STATE_ATTACKING;
            return;
        //    preState = curState;
         //   curState = EnemyState.STATE_ATTACKING;
        }

        nvAgent.SetDestination(playerTrans.position);
        if (animator.GetBool("IsReadyToThrow") == false)
        {
            animator.SetBool("IsReadyToThrow", true);
         //   if (attackCoolTime > attackTime) return; //쿨타임 남음

          //  if (!isTarget(playerTrans)) return; //장애물 유무 확인


            //if (Vector3.Distance(transform.position, targetWayPoint.position) <= nvAgent.stoppingDistance)
            //{
            //    animator.SetBool("isMoving", false);
            //}
            //else
            //{
            //    if (!animator.GetBool("isMoving"))
            //        animator.SetBool("isMoving", true);
            //}

            //animator.SetTrigger("ReadyToThrow");
            /*Vector3 snowballPos = snowStartTrans.position;
            snowballPos += (snowStartTrans.rotation * Vector3.forward);

            SnowBallPoolingScript.Instance.GetObject().Initialize("Enemy", power, snowballPos, snowStartTrans.rotation);*/
            //Instantiate(snowball, snowballPos, transform.rotation);
            //attackTime = 0.0f;
        }
        else
        {
            if (attackCoolTime > attackTime) return; //쿨타임 남음
            if (!isTarget(playerTrans)) return; //장애물 유무 확인
            attackTime = 0.0f;
            animator.SetTrigger("Throw");
        }
    }
    void followToIdle()
    {
        nvAgent.SetDestination(targetWayPoint.position);
        alertTime = 0.0f;
        preState = EnemyState.STATE_IDLE;
    }
    void attackToIdle()
    {
        animator.SetBool("IsReadyToThrow", false);
        preState = EnemyState.STATE_IDLE;
        //SnowBallScript snowball = GetComponentInChildren<SnowBallScript>();
        //if (snowball)
        //{
        //    SnowBallPoolingScript.Instance.ReturnObject(snowball);
        //}

        snow = GetComponentInChildren<SnowBallScript>();
        if (snow)
        {
            SnowBallPoolingScript.Instance.ReturnObject(snow);
            snow = null;
        }

        nvAgent.SetDestination(targetWayPoint.position);
        alertTime = 0.0f;
        preState = EnemyState.STATE_IDLE;
    }


    public void idleToFollow()
    {
        if (animator.GetBool("isAlerting"))
        {
            animator.SetBool("isAlerting", false);
        }
        followTime = 0.0f;
        preState = EnemyState.STATE_FOLLOWING;
       // preState = curState;
    }
    public void attackToFollow()
    {
        animator.SetBool("IsReadyToThrow", false);
        // preState = curState;
        preState = EnemyState.STATE_FOLLOWING;
        //SnowBallScript snowball = GetComponentInChildren<SnowBallScript>();
        //if (snowball)
        //{
        //    SnowBallPoolingScript.Instance.ReturnObject(snowball);

        //}

        snow = GetComponentInChildren<SnowBallScript>();
        if (snow)
        {
            SnowBallPoolingScript.Instance.ReturnObject(snow);
            snow = null;
        }
    }
    public void followToAttack()
    {
       preState = EnemyState.STATE_ATTACKING;
        //preState = curState;
    }

    public void idleToAttack()
    {
        preState = EnemyState.STATE_ATTACKING;
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

    public bool checkHp()
    {
        return hp <= 0;
    }

    public void Hit(float damage)
    {
        hp -= damage;
        checkHp();
        hpSlider.GetComponent<EnemyHPScript>().SetEnemyHPSlider(hp);
        animator.SetTrigger("Hit");
    }
} //스크립트 클래스 괄호 삭제X
