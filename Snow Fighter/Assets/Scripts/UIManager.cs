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
    [SerializeField] float enemyHPAniTime = 20.0f;

    float time = 0.0f;
    Transform player;
    bool enemyHPAni;
    bool isBlinking;
    Slider enemyHP;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        PlayerScript playerScirpt = player.GetComponent<PlayerScript>();
        playerHP.maxValue = playerScirpt.MaxHP;
        playerHP.value = playerHP.maxValue;

        power.maxValue = playerScirpt.MaxPower;
        power.minValue = playerScirpt.InitPower;
        power.value = playerScirpt.InitPower;

        enemyHPAni = false;
        isBlinking = false;
    }

    public void Update()
    {
        enemyHPUpdate();
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
        enemyHPAni = true;
        
        enemyHP.value = hp;
        setHPRender(true);
        time = 0.0f;
        StopBlinking();
        isBlinking = false;
    }

    public void InitEnemyHPSlider(Transform enemy, float maxHP)
    {
        enemyHP = enemy.GetComponentInChildren<Slider>(); //enemyCanvas.GetComponentInChildren<Slider>();
        enemyHP.maxValue = maxHP;
        enemyHP.value = enemyHP.maxValue;

        setHPRender(false);
    }

    public void SetTarget(bool isTarget)
    {
        if (isTarget)
            reticle.color = Color.red;
        else
            reticle.color = Color.white;
    }

    void setHPRender(bool isOn)
    {
        MaskableGraphic[] sliderUI = enemyHP.GetComponentsInChildren<MaskableGraphic>();
            foreach(MaskableGraphic ui in sliderUI)
            {
                ui.enabled = isOn;
            }
    }

    void blinkHP()
    {
            MaskableGraphic[] sliderUI = enemyHP.GetComponentsInChildren<MaskableGraphic>();
            foreach (MaskableGraphic ui in sliderUI)
            {
                ui.enabled = !ui.enabled;
            }
    }

    void enemyHPUpdate()
    {
        if (enemyHPAni)
        {
            enemyCanvas.transform.rotation = player.GetComponent<PlayerScript>().SightCamTrans.rotation;
            if (enemyCanvas.transform.gameObject.GetComponentInParent<EnemyAIScript>().Hp > 20)
            {
                time += Time.deltaTime;
                if (time > enemyHPAniTime)
                {
                    if (isBlinking)
                    {
                        isBlinking = false;
                        StopBlinking();
                    }
                    setHPRender(false);
                    enemyHPAni = false;
                }
                else if (time > enemyHPAniTime - 3)
                {
                    if (isBlinking) return;
                    isBlinking = true;
                    StartBlinking();
                }
            }
            else
            {
                enemyHP.fillRect.transform.GetComponent<Image>().color = Color.red;
            }
        }
    }
    IEnumerator Blink()
    {
        while (true)
        {
            blinkHP();
            yield return new WaitForSeconds(0.2f);
        }
    }

    void StartBlinking()
    {
        StopAllCoroutines();
        StartCoroutine("Blink");
    }

    void StopBlinking()
    {
        StopAllCoroutines();
    }
}
