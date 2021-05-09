using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    protected UIManager() { }
    
    Transform player;

    Slider playerHP = null;
    Image reticle = null;
     GameObject enemyHPPrefab = null;
    public GameObject EnemyHPPrefab { get { return enemyHPPrefab; } }

     Canvas hpCanvas = null;
    public Canvas HpCanvas { get { return hpCanvas; } }

    TextMeshProUGUI enemyCount = null;
    public TextMeshProUGUI EnemyCount { get { return enemyCount; } set { enemyCount = value; } }
    
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        PlayerScript playerScirpt = player.GetComponent<PlayerScript>();

        playerHP = GameObject.Find("HP").GetComponent<Slider>();
        playerHP.maxValue = playerScirpt.MaxHP;
        playerHP.value = playerHP.maxValue;

        reticle = GameObject.Find("Reticle").GetComponent<Image>();
        reticle.color = Color.gray;

        enemyHPPrefab = GameObject.Find("EnemyHP");
        hpCanvas = GameObject.Find("HPCanvas").GetComponent<Canvas>();
        EnemyCount = GameObject.Find("EnemyCount").GetComponent<TextMeshProUGUI>();


        enemyCount.text = StageManager.Instance.EnemyCount + " / " + StageManager.Instance.TotalEnemyCount;

    }

    public void SetTarget(bool isTarget)
    {
        if (isTarget)
            reticle.color = Color.red;
        else
            reticle.color = Color.gray;
    }

    public void SetPlayerHPSlider(float hp)
    {
        playerHP.value = hp;
    }

    public void SetEnemyCountText()
    {
        enemyCount.text = (StageManager.Instance.TotalEnemyCount - StageManager.Instance.EnemyCount) + " / " + StageManager.Instance.TotalEnemyCount;
    }

    //public void SetEnemyHPSlider(Slider enemyHP, float hp)
    //{
    //    enemyHPAni = true;
        
    //    enemyHP.value = hp;
    //    setHPRender(true);
    //    time = 0.0f;
    //    StopBlinking();
    //    isBlinking = false;
    //}

    //public void InitEnemyHPSlider(Slider enemyHP, float maxHP)
    //{
    //    //enemyHP = enemy.GetComponentInChildren<Slider>(); //enemyCanvas.GetComponentInChildren<Slider>();
    //    enemyHP.maxValue = maxHP;
    //    enemyHP.value = enemyHP.maxValue;

    //    setHPRender(false);
    //}

   

    //void setHPRender(Slider enemyHP, bool isOn)
    //{
    //    MaskableGraphic[] sliderUI = enemyHP.GetComponentsInChildren<MaskableGraphic>();
    //        foreach(MaskableGraphic ui in sliderUI)
    //        {
    //            ui.enabled = isOn;
    //        }
    //}

    //void blinkHP(Slider enemyHP)
    //{
    //        MaskableGraphic[] sliderUI = enemyHP.GetComponentsInChildren<MaskableGraphic>();
    //        foreach (MaskableGraphic ui in sliderUI)
    //        {
    //            ui.enabled = !ui.enabled;
    //        }
    //}

    //void enemyHPUpdate()
    //{
    //    if (enemyHPAni)
    //    {
    //        enemyCanvas.transform.rotation = player.GetComponent<PlayerScript>().SightCamTrans.rotation;
    //        if (enemyCanvas.transform.gameObject.GetComponentInParent<EnemyAIScript>().Hp > 20)
    //        {
    //            time += Time.deltaTime;
    //            if (time > enemyHPAniTime)
    //            {
    //                if (isBlinking)
    //                {
    //                    isBlinking = false;
    //                    StopBlinking();
    //                }
    //                setHPRender(false);
    //                enemyHPAni = false;
    //            }
    //            else if (time > enemyHPAniTime - 3)
    //            {
    //                if (isBlinking) return;
    //                isBlinking = true;
    //                StartBlinking();
    //            }
    //        }
    //        else
    //        {
    //            enemyHP.fillRect.transform.GetComponent<Image>().color = Color.red;
    //        }
    //    }
    //}
    //IEnumerator Blink()
    //{
    //    while (true)
    //    {
    //        blinkHP();
    //        yield return new WaitForSeconds(0.2f);
    //    }
    //}

    //void StartBlinking()
    //{
    //    StopAllCoroutines();
    //    StartCoroutine("Blink");
    //}

    //void StopBlinking()
    //{
    //    StopAllCoroutines();
    //}
}
