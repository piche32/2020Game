using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
namespace Enemy.Ver2
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(RagdollChanger))]
    public class Enemy : MonoBehaviour
    {
        NavMeshAgent nvAgent = null;
        PlayerScript player = null;
        Animator animator = null;

        float maxHP = 1.0f;
        float hp = 1.0f;
        public float HP { get { return hp; } }

        //HPBar
        Slider hpSlider;
        public Slider HpSlider { get { return hpSlider; } set { hpSlider = value; } }


        // Start is called before the first frame update
        void Start()
        {
            player = GameObject.Find("Player").GetComponent<PlayerScript>();
            if (player == null)
                Debug.LogError("[Enemy.cs]Can't Find PlayerScript.");
            nvAgent = GetComponent<NavMeshAgent>();
            animator = GetComponentInChildren<Animator>();

            hp = maxHP;
            hpSlider = GameObject.Instantiate(GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>().EnemyHPPrefab.GetComponent<Slider>());
            hpSlider.GetComponent<EnemyHPScript>().enabled = true;
            hpSlider.GetComponent<EnemyHPScript>().InitEnemyHPSlider(this.transform, maxHP);

        }

        // Update is called once per frame
        void Update()
        {
        //    if(nvAgent.enabled)
        //        nvAgent.destination = player.transform.position;
            animator.SetFloat("Speed", nvAgent.velocity.magnitude);
        }

        public bool checkHp()
        {
            return hp <= 0;
        }
        public void Hit(float damage)
        {
            hp -= damage;
            //animator.Play("Hit", 1);
            //animator.Play("Hit", 0);
            //animator.SetTrigger("Hit");
            hpSlider.GetComponent<EnemyHPScript>().SetEnemyHPSlider(hp);
        }

        public void Die()
        {
            if (this.GetComponentInChildren<SnowBallScript>())
                SnowBallPoolingScript.Instance.ReturnObject(this.GetComponentInChildren<SnowBallScript>());

            this.GetComponentInChildren<Renderer>().enabled = false;
            --StageManager.Instance.EnemyCount;
            GameObject.Find("UIManager").GetComponent<UIManager>().SetEnemyCountText();
            if (StageManager.Instance.EnemyCount == 0)
            {
                GameManagerScript.Instance.Success();
            }

         

            this.gameObject.SetActive(false);
           // GetComponent<Panda.PandaBehaviour>().enabled = false;

        }
    }
}