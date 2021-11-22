using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSnowballEffect : MonoBehaviour
{
    public float damage;
    bool canAttack;

    private void OnEnable()
    {
        canAttack = true;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag != "Player") return;
        if (!canAttack) return;
        canAttack = false;
        PlayerScript player = other.transform.GetComponent<PlayerScript>();
        player.damaged(-damage);
        EventContainer.Instance.Events["OnPlayerIced"].Invoke();
    }
}
