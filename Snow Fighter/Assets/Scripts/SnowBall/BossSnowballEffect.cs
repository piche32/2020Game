using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSnowballEffect : MonoBehaviour
{
    public float damage;
    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag != "Player") return;
        PlayerScript player = other.transform.GetComponent<PlayerScript>();
        //Player.damaged(-damage);
        EventContainer.Instance.Events["OnPlayerIced"].Invoke();
    }
}
