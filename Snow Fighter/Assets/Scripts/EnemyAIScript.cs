using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

enum EnemyState
{
    STATE_STANDING,
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

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) {
            Debug.Log("There's no Player.");
            return;
        }

        state = EnemyState.STATE_STANDING;
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

        
        checkState();

        if (state == EnemyState.STATE_STANDING) //정해진 위치에 서있기, 돌아가기
        {
            nvAgent.enabled = true;
            nvAgent.SetDestination(startPoint);
            nvAgent.stoppingDistance = 2;
            nvAgent.speed = speed;

            /*
            if (Vector3.Distance(transform.position, startPoint) > 2)
            {
                gameObject.transform.LookAt(startPoint);
                gameObject.transform.Translate(Vector3.forward * speed);
            }
            */
        }

        else if(state == EnemyState.STATE_FOLLOWING) //이미 플레이어를 인식한 상태 -> 플레이어 따라가기, 다시: 장애물 인식 처리 X, 공격 받았을 때 처리 X
        {
            nvAgent.enabled = true;
            nvAgent.SetDestination(player.transform.position);
            nvAgent.stoppingDistance = 2;
            nvAgent.speed = speed;

            //gameObject.transform.LookAt(player.transform);
            //gameObject.transform.Translate(Vector3.forward * speed);
        }

        else if(state == EnemyState.STATE_ATTACKING)
        { 
            //이미 시야에 장애물이 없다는 것을 판정함
            //쏘기만 하면 됨.
            nvAgent.enabled = false;
            gameObject.transform.LookAt(player.transform);

            
            snowStartPt = snowStart.transform.position;
            Vector3 snowballPos = snowStartPt;
            snowballPos += (transform.rotation * Vector3.forward);

            Instantiate(snowball, snowballPos, transform.rotation, transform);
            time = 0.0f;
            
        }

        /*else if(state == EnemyState.STATE_DODGING) //다시: 피할 확률 집어넣기
        {

        }*/

        Debug.Log("Enemy state: " + state);

    } //Update 함수 괄호 삭제X
    
    void checkState()
    {
        distBtwPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (player == null || distBtwPlayer > followingDist) //player가 멀어지거나 없으면 제자리
        {
            state = EnemyState.STATE_STANDING;
        }

        
        if (distBtwPlayer < followingDist) 
        {
            state = EnemyState.STATE_FOLLOWING;
        }
        
        if (distBtwPlayer < attackingDist)
        {
            if (time > attackCoolTime)
            {
                gameObject.transform.LookAt(player.transform);
                snowStartPt = snowStart.transform.position;

                rayDist = distBtwPlayer + 1.0f;
                Debug.DrawRay(snowStartPt, transform.forward * rayDist, Color.blue, 0.1f);
                if (Physics.Raycast(snowStartPt, transform.forward, out ray, rayDist))
                {
                    if (ray.transform.CompareTag("Player"))
                    {
                        state = EnemyState.STATE_ATTACKING;
                    }
                    else
                    {
                        state = EnemyState.STATE_FOLLOWING; //장애물 있으면 플레이어 쫓아가기
                    }
                }
            }
            else
            {
                state = EnemyState.STATE_FOLLOWING; //쿨타임 남으면 플레이어 쫓아가기
            }
        }

        //플레이어가 공격 시 피하기...(?)
        /*Collider[] t_cols = Physics.OverlapSphere(transform.position, dodgingDist);//, snowballLM);
        if(t_cols.Length > 0)
        {
            for(int i = 0; i < t_cols.Length; i++)
            {
                if(t_cols[i].transform.IsChildOf(player.transform))
                {
                    state = EnemyState.STATE_DODGING;
                    Debug.Log("Enemy state: " + state);

                }
            }
        }*/
    }

} //스크립트 클래스 괄호 삭제X
