using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    protected UIManager() { }

    [SerializeField] Slider playerHP = null;
    [SerializeField] Canvas enemyCanvas = null;
    [SerializeField] Slider power = null;
    [SerializeField] Image reticle = null;
    // Start is called before the first frame update

    Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        PlayerScript playerScirpt = player.GetComponent<PlayerScript>();
        playerHP.maxValue = playerScirpt.MaxHP;
        playerHP.value = playerHP.maxValue;

        power.maxValue = playerScirpt.MaxPower;
        power.minValue = playerScirpt.InitPower;
        power.value = playerScirpt.InitPower;

    }

    public void Update()
    {
        enemyCanvas.transform.rotation = player.GetComponent<PlayerScript>().SightCamTrans.rotation;
    }
    public void SetPlayerHPSlider(float hp)
    {
        playerHP.value = hp;
    }

    public void SetPlayerPowerSlider(float power)
    {
        if (this.power == null) return;
        this.power.value = power;
    }

    public void SetEnemyHPSlider(Transform enemy, float hp)
    {
        Slider HP = enemy.GetComponent<EnemyAIScript>().HpSlider;
        HP.value = hp;
    }

    public void InitEnemyHPSlider(Transform enemy, float maxHP)
    {
        Slider HP = enemy.GetComponent<EnemyAIScript>().HpSlider;
        HP.maxValue = maxHP;
        HP.value = HP.maxValue;
    }

    public void SetTarget(bool isTarget)
    {
        if (isTarget)
            reticle.color = Color.red;
        else
            reticle.color = Color.white;
    }
}
