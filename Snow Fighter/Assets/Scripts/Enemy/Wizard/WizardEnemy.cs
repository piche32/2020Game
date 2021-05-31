using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardEnemy : MonoBehaviour
{
    Enemy.Ver2.Enemy self;

    private void Start()
    {
        self = GetComponent<Enemy.Ver2.Enemy>();
    }
    private void Update()
    {
        if (self.checkHp())
        {
            GameObject enemys = GameObject.Find("Enemys");
            int childIdx = enemys.transform.childCount;
            for(int i = 0; i < childIdx; i++)
            {
                GameObject enemy = enemys.transform.GetChild(i).gameObject;
                if (enemy != null && enemy.activeSelf)
                {
                    if (enemy.name == "WizardEnemy") continue;
                    Enemy.Ver2.Enemy enemyScript = enemy.GetComponent<Enemy.Ver2.Enemy>();
                    if (enemyScript != null)
                    {
                        enemyScript.Hit(enemyScript.HP + 1.0f);
                    }
                }
            }
        }
    }
}
