using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;

public class NormalEnemyAIBT : MonoBehaviour
{
    PlayerScript player = null;
    Animator animator = null;

    [SerializeField] float attackDelayTime = 1.2f;
     float damage = 10.0f;


    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerScript>();
        if (player == null)
            Debug.LogError("[Enemy.Ver2.EnemyAIBT.cs]Can't Find PlayerScript.");

        animator = GetComponentInChildren<Animator>();
        damage = GetComponent<Enemy.Ver2.Enemy>().Damage;

    }

    /// <summary>
    /// 타겟 쪽으로 살짝 몸을 트는 함수
    /// 타겟이 가까이 있을 땐 집요하게 몸을 튼다.
    /// </summary>
    [Task]
    public void AimAt_Target()
    {
        if (player != null)
        {
            var targetDekta = (player.transform.position - this.transform.position);

            Vector3 dir = Vector3.zero; //Enemy와 Player 간의 방향 벡터
            dir.x = player.transform.position.x - this.transform.position.x;
            dir.z = player.transform.position.z - this.transform.position.z;
            dir.y = this.transform.position.y;
            dir = dir.normalized;

            //Enemy와 Player 간의 각도
            float angle = Vector3.Angle(dir, new Vector3(transform.forward.x, dir.y, transform.forward.z));

            if (targetDekta.magnitude > 3f) //일정 거리만큼 멀리 있으면 player쪽으로 회전한다.
            {
                if (angle > 120f || angle < -120.0f) { Task.current.Fail(); return; }
                if (angle > 10.0f || angle < -10.0f)
                {
                    Vector3 look = Vector3.Slerp(this.transform.forward, dir, Time.deltaTime * 3.0f);
                    this.transform.rotation = Quaternion.LookRotation(look, Vector3.up);
                }
                else
                    Task.current.Succeed();

            }
            else
            {
                if (angle > 30.0f || angle < -30.0f)
                {
                    Vector3 look = Vector3.Slerp(this.transform.forward, dir, Time.deltaTime * 2.0f);
                    this.transform.rotation = Quaternion.LookRotation(look, Vector3.up);
                }
                else
                    Task.current.Succeed();
            }
            if (Task.isInspected)
                Task.current.debugInfo = string.Format("angle={0}", Vector3.Angle(dir, this.transform.forward));

        }


    }

    [Task]
    public void Attack()
    {
        if (animator.GetBool("isAttacking"))
        {
            animator.SetBool("isAttacking", false);

            if (!animator.GetCurrentAnimatorStateInfo(1).IsName("Attack")
                || (animator.GetCurrentAnimatorStateInfo(1).IsName("Attack")
                && animator.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.9f))
            {
                Task.current.Succeed();
            }
        }
        else
        {//공격 중이 아닐 때,
            if (!animator.GetCurrentAnimatorStateInfo(1).IsName("Attack")) // 공격 애니메이션 실행 아닐 때
            {
                animator.SetBool("isAttacking", true);
                Invoke("HitPlayer", attackDelayTime);
                animator.SetTrigger("Attack");
            }
        }
    }

    void HitPlayer()
    {
        if (player != null && this.gameObject.activeInHierarchy) //플레이어도 안죽고, 이 Enemy도 안 죽었을 때,
        {
            if (Vector3.Distance(this.transform.position, player.transform.position) < 2.0f)
            {
                player.damaged(-damage);
//                player.setHP(-damage);
            }
        }
    }



}
