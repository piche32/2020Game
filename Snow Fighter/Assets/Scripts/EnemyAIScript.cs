using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

enum EnemyState
{
    STATE_IDLE,
    STATE_FOLLOWING,
    STATE_ATTACKING,
    STATE_DODGING
}

public class EnemyAIScript : MonoBehaviour
{
    [SerializeField] GameObject snowball = null;
    GameObject player;

    [SerializeField] float followingDist = 10.0f;
    [SerializeField] float attackingDist = 5.0f;

    [SerializeField] float speed = 1.0f;
    [SerializeField] float attackCoolTime = 10.0f;
    float time;

    Vector3 startPoint;
    EnemyState state;

    float distBtwPlayer; //플레이어까지 거리

    NavMeshAgent nvAgent;

    RaycastHit ray;
    float rayDist; //Ray의 길이
    GameObject snowStart;
    Vector3 snowStartPt;

    [SerializeField] LayerMask snowballLM;
    [SerializeField]float dodgingDist = 20.0f;

    [SerializeField] LayerMask playerLM;
    [SerializeField] float sightAngle = 60.0f;

    [SerializeField] LayerMask snowStartLM;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) {
            Debug.Log("There's no Player.");
            return;
        }

        state = EnemyState.STATE_IDLE;
        startPoint = gameObject.transform.position; //실행 시, 현재 서 있는 지점을 StartPoint로 두기
        distBtwPlayer = Vector3.Distance(transform.position, player.transform.position);
        time = 0.0f;

        nvAgent = GetComponent<NavMeshAgent>();

        nvAgent.enabled = false;

        rayDist = distBtwPlayer;
        snowStart = GameObject.Find("SnowStart");
        if(snowStartPt == null)
        {
            Debug.Log("Can not Find SnowStartPt");
            return;
        }
        snowStartPt = snowStart.transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        distBtwPlayer = Vector3.Distance(transform.position, player.transform.position);

        checkState();
        
        /*
        else if(state == EnemyState.STATE_DODGING) //다시: 피할 확률 집어넣기
        {

        }*/

        Debug.Log("Enemy state: " + state);
    } //Update 함수 괄호 삭제X
    
    bool isTargetInSight()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, followingDist, playerLM);

        if(cols.Length > 0)
        {
            Transform tfPlayer = cols[0].transform;

            Vector3 dir = (tfPlayer.position - transform.position).normalized;
            float angle = Vector3.Angle(dir, transform.forward);

            if(angle < sightAngle * 0.5f)
            {
                if(isTarget("Player", dir, distBtwPlayer)) //Player와 Enemy 사이에 장애물 확인
                { return true; }
            }
        }
        return false;
    }
    
    void idle()
    {

        if(isTargetInSight())
        {
            state = EnemyState.STATE_FOLLOWING;
            return;
        }

        //정해진 위치로 돌아가기
        if (Vector3.Distance(transform.position, startPoint) > 2)
        {
            nvAgent.enabled = true;
            nvAgent.SetDestination(startPoint);
            nvAgent.stoppingDistance = 2;
            nvAgent.speed = speed;
        }

        else //장애물을 등지고 경계
        {

        }
        return;
        
    }

    void follow()
    {
        transform.LookAt(player.transform.position);

        if (distBtwPlayer > followingDist) {
            state = EnemyState.STATE_IDLE;
            return;
        }

        if(distBtwPlayer < attackingDist 
            && isTarget("Player", transform.forward, distBtwPlayer) // enemy와 player 사이에 다른 장애물이 존재하는지 여부
            && time > attackCoolTime)
        {
            state = EnemyState.STATE_ATTACKING;
            return;
        }

        nvAgent.enabled = true;
        nvAgent.SetDestination(player.transform.position);
        nvAgent.stoppingDistance = 2;
        nvAgent.speed = speed;

        return;
    }

    bool isTarget(string targetTag, Vector3 rayDir, float maxDist) //enemy와 target 사이에 다른 물체가 있는지
    {
        Debug.DrawRay(transform.position, transform.forward * rayDist, Color.blue, 0.1f);
        if(Physics.Raycast(transform.position, rayDir, out ray, maxDist, -1 -snowballLM -snowStartLM)){
            if (ray.transform.CompareTag(targetTag)) { return true; }
            else return false;
        }

        Debug.Log("Raycast error");
        return false;
    }

    void attack()
    {
        if(distBtwPlayer > attackingDist || time < attackCoolTime)
        {
            state = EnemyState.STATE_FOLLOWING;
        }

        nvAgent.enabled = false;
        gameObject.transform.LookAt(player.transform);
        
        if (!isTarget("Player", transform.forward, distBtwPlayer)) //Player와 Enemy 사이에 장애물 있는지
        {
            state = EnemyState.STATE_FOLLOWING;
            return;
        }

        snowStartPt = snowStart.transform.position;

        Vector3 snowballPos = snowStartPt;
        snowballPos += (transform.rotation * Vector3.forward);

        Instantiate(snowball, snowballPos, transform.rotation);
        time = 0.0f;
        
    }

    void checkState()
    {
        switch (state)
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
    
} //스크립트 클래스 괄호 삭제X
