using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;

namespace Enemy.Ver2
{
    public class WizardEnemyAIBT : EnemyAIBT
    {
        Animator animator = null;

        [SerializeField] Transform attackPoint = null;
        [SerializeField] GameObject skillParticle = null;
        [SerializeField] Vector3 skillOffset = new Vector3(0.1f, 0f, 0f);

        [SerializeField] float attackDelayTime = 3.0f;

        [SerializeField] float rotateSpeed = 1.0f;
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            if (attackPoint == null)
            {
                Debug.LogWarning("[" + this.ToString() + "]" + "Can not found AttackPoint.");
            }

            //animator = GetComponentInChildren<Animator>();
            animator = GetComponent<Animator>();

            skillParticle.GetComponent<ParticleSystem>().Stop();
            skillParticle.GetComponent<ParticleSystem>().Clear();

            ParticleSystem[] particles;
            particles = skillParticle.GetComponentsInChildren<ParticleSystem>();
            foreach (var particle in particles)
            {
                particle.Clear();
                particle.Stop();
            }


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
                var targetDelta = (player.transform.position - this.transform.position);

                Vector3 dir = Vector3.zero; //Enemy와 Player 간의 방향 벡터
                dir.x = player.transform.position.x - this.transform.position.x;
                dir.z = player.transform.position.z - this.transform.position.z;
                dir.y = this.transform.position.y;
                dir = dir.normalized;

                if (Task.isInspected)
                    Task.current.debugInfo = string.Format("angle={0}", Vector3.Angle(dir, this.transform.forward));

                //Enemy와 Player 간의 각도
                float angle = Vector3.Angle(dir, new Vector3(transform.forward.x, dir.y, transform.forward.z));

                if (targetDelta.magnitude < attackDist/10) {
                    transform.LookAt(player.transform.position);
                    Task.current.Succeed(); 
                }
                if (angle > 0.01f || angle < -0.01f)
                {
                    Vector3 look = Vector3.Slerp(this.transform.forward, dir, Time.deltaTime * 6.0f);
                    this.transform.rotation = Quaternion.LookRotation(look, Vector3.up);
                }
                else
                {
                    Task.current.Succeed(); 
                }
            }
            else
                Task.current.Fail();
        }


        [Task]
        public void Attack()
        {
             if (animator.GetBool("isAttacking"))
            {

                if (!animator.GetCurrentAnimatorStateInfo(1).IsName("Attack")
                    || (animator.GetCurrentAnimatorStateInfo(1).IsName("Attack")
                    && animator.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.9f))
                {
                    animator.SetBool("isAttacking", false);

                    skillParticle.GetComponent<ParticleSystem>().Stop();
                    ParticleSystem[] particles;
                    particles = skillParticle.GetComponentsInChildren<ParticleSystem>();
                    foreach (var particle in particles)
                    {
                        particle.Stop();
                    }

                    Task.current.Succeed();
                }
            }
                else
            { //공격 중이 아닐 때,
                if (!animator.GetCurrentAnimatorStateInfo(1).IsName("Attack")) //공격 애니메이션 실행 아닐 떄
                {
                    animator.SetBool("isAttacking", true);
                    Invoke("HitPlayer", attackDelayTime);
                    animator.SetTrigger("Attack");
                    Task.current.Succeed();
                }

            }
        }

        void HitPlayer()
        {
            if (player != null && this.gameObject.activeInHierarchy) //플레이어 살아있음. 이 Enemy도 살아있음
            {
                skillParticle.transform.position = attackPoint.position + skillOffset;
                skillParticle.GetComponent<ParticleSystem>().Play();
                Vector3 target = player.transform.position;
                target.y += 0.5f;
                Vector3 dir = target - skillParticle.transform.position;

                dir = dir.normalized;
                skillParticle.transform.rotation = Quaternion.LookRotation(dir, skillParticle.transform.up);

                ParticleSystem[] particles;
                particles = skillParticle.GetComponentsInChildren<ParticleSystem>();
                foreach (var particle in particles)
                {
                    particle.Play();
                }

            }
        }

        [Task]
        public bool IsAttacking()
        {
            return animator.GetBool("isAttacking");
        }

        [Task]
        public void StopAttacking()
        {
            skillParticle.GetComponent<ParticleSystem>().Stop();
            skillParticle.GetComponent<ParticleSystem>().Clear();

            ParticleSystem[] particles;
            particles = skillParticle.GetComponentsInChildren<ParticleSystem>();
            foreach (var particle in particles)
            {
                particle.Clear();
                particle.Stop();
            }
            Task.current.Succeed();
        }

        [Task]
        public void Alert()
        {
            if (!animator.GetCurrentAnimatorStateInfo(1).IsName("Alert")) animator.SetTrigger("Alert");
            if (!animator.GetBool("isAlerting")) 
            {
                animator.SetBool("isAlerting", true);
                animator.applyRootMotion = true;
            }

            Task.current.Succeed();
        }

        [Task]
        public void StopAlerting()
        {
            animator.SetBool("isAlerting", false);
            animator.applyRootMotion = false;
            Task.current.Succeed();
        }

        [Task]
        public bool canFollow()
        {
            bool ret = false;
            if (player != null && player.gameObject != null)
            {
                IsPlayerInEnemySight self = this.transform.Find("FollowColl").GetComponent<IsPlayerInEnemySight>();
                ret = self._IsPlayerInEnemySight;
            }
            return ret;
        }

        [Task]
        public void StopFollowing()
        {
            Task.current.Succeed();
        }

    }
}