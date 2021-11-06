using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class IsPlayerInEnemySight : MonoBehaviour
{
    [SerializeField] float DIST = 10.0f; //Enemy와 Player의 사이 최대 거리
    [SerializeField] float REALIZEDDIST = 10.0f; //Enemy가 Player를 알아차릴 수 있는 거리
    [SerializeField] float SIGHTANGLE = 120.0f; //Enemy가 볼 수 있는 각도
    bool isPlayerInEnemySight = false;
    public bool _IsPlayerInEnemySight
    { get { return isPlayerInEnemySight; } }

    PlayerScript player = null;

    SphereCollider coll = null;


    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<SphereCollider>();
        coll.radius = DIST;
        player = GameObject.Find("Player").GetComponent<PlayerScript>();
        if (player == null)
            Debug.LogError("[IsPlayerInEnemeySight.cs]There is no player.");

    }


    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            float sightAngle = SIGHTANGLE; //Enemy가 볼 수 있는 각도
            isPlayerInEnemySight = false; //Player가 Enemy의 시야에 들어왔는지

            Vector3 dir = Vector3.zero; //Enemy와 Player 간의 방향 벡터
            dir.x = player.transform.position.x - this.transform.position.x;
            dir.z = player.transform.position.z - this.transform.position.z;
            
            float dist = dir.magnitude; //y축은 거리 계산에서 제외하기

            dir = dir.normalized;

            //Enemy와 Player 간의 각도
            float angle = Vector3.Angle(dir, new Vector3(transform.forward.x, dir.y, transform.forward.z));

            if (dist < REALIZEDDIST) //거리가 일정 이하일 때, 시야 각을 조절해준다.
            {
                sightAngle = 360.0f;
            }

            if (angle < sightAngle / 2) //Player가 Enemy의 시야 안에 들어 왔을 때
            {
                isPlayerInEnemySight = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player" )
        {
            Vector3 dir = player.transform.position - this.transform.position;
            dir.y = 0.0f;
            if (dir.magnitude > coll.radius)
            { 
                isPlayerInEnemySight = false;
            }

            //   Debug.Break();
        }
    }
}
