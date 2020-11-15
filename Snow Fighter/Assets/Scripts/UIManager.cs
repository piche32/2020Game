using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    protected UIManager() { }

    [SerializeField] Slider playerHP = null;
    [SerializeField] Slider power = null;
    // Start is called before the first frame update

    private void Start()
    {
        PlayerScript player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        playerHP.maxValue = player.MaxHP;
        playerHP.value = playerHP.maxValue;

        power.maxValue = player.MaxPower;
        power.minValue = player.InitPower;
        power.value = player.InitPower;
    }
    public void SetPlayerHPSlider(float hp)
    {
        playerHP.value = hp;
    }

    public void SetPlayerPowerSlider(float power)
    {
        this.power.value = power;
    }
}
