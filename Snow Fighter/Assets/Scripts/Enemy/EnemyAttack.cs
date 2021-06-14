using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy.Ver2
{
    public class EnemyAttack : MonoBehaviour
    {
        PlayerScript player = null;
        [SerializeField] float damage = 10.0f;
        [SerializeField] float coolTime = 2.0f;
        float lateAttacktime = 0.0f;
        private void Start()
        {
            player = GameObject.Find("Player").GetComponent<PlayerScript>();
        }

        private void OnTriggerStay(Collider other)
        {
            if (Vector3.Distance(player.transform.position, this.transform.position) <= 2.0f)
            {
                if (Time.time - lateAttacktime < coolTime)
                {
                    return;
                }
                if (other.transform.tag == "Player" && other.transform.name != "Sight Camera")
                {
                    lateAttacktime = Time.time;
                    player.damaged(-damage);
           //         player.setHP(-damage);
                }
            }
        }
    }
}