using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

namespace Enemy.Ver2
{
    public class Boss : Enemy
    {
        // Start is called before the first frame update
        protected override void Start()
        {
            player = GameObject.Find("Player").GetComponent<PlayerScript>();
            ConsoleDebug.IsNull(this.name, "player", player);
            nvAgent = GetComponent<NavMeshAgent>();
            animator = this.GetComponent<Animator>();
            if (animator == null)
                animator = GetComponentInChildren<Animator>();
            ConsoleDebug.IsNull(this.name, "animator", animator);

            hp = MaxHP;
            HpSlider = GameObject.Find("BossHP").GetComponent<Slider>();
            HpSlider.GetComponent<EnemyHPScript>().enabled = true;
            HpSlider.GetComponent<EnemyHPScript>().InitEnemyHPSlider(this.transform, MaxHP);
        }

    }
}
