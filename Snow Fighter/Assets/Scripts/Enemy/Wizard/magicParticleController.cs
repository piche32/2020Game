using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class magicParticleController : MonoBehaviour
{
    ParticleSystem ps;
    List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();

    PlayerScript player = null;

     float damage = 5.0f;
    [SerializeField] float coolTime = 1.0f;
    float lateAttackTime = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        player = GameObject.Find("Player").GetComponent<PlayerScript>();
        damage = GetComponentInParent<Enemy.Ver2.Enemy>().Damage;
    }

    private void OnParticleTrigger()
    {
        if (Time.time - lateAttackTime < coolTime) return; //쿨타임
        if (player == null || !player.gameObject.activeInHierarchy) return; //플레이어 죽으면 리턴

        int numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
        if (numEnter > 5)
        {
            lateAttackTime = Time.time;
            player.damaged(-damage);
            // EventContainer.Instance.Events["OnPlayerDamaged"].Invoke();
            // player.setHP(-damage);
            for (int i = 0; i < numEnter; i++)
            {
                ParticleSystem.Particle temp = enter[i];
                // temp.startColor = new Color32(0, 0, 0, 0);
                enter[i] = temp;
            }
        }

    }

   
}
