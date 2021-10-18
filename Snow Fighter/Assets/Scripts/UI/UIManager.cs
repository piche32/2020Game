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
    private Vector3 reticleDefaultPosition;
    GameObject enemyHPPrefab = null;
    public GameObject EnemyHPPrefab { get { return enemyHPPrefab; } }

    Canvas hpCanvas = null;
    public Canvas HpCanvas { get { return hpCanvas; } }

    TextMeshProUGUI enemyCount = null;
    public TextMeshProUGUI EnemyCount { get { return enemyCount; } set { enemyCount = value; } }

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        if (player == null)
        {
            Debug.LogError(string.Format("[{0}:{1}] Can not find Player.", this.gameObject.name.ToString(), this.name.ToString()));
            return;
        }
        PlayerScript playerScirpt = player.GetComponent<PlayerScript>();

        playerHP = player.Find("Sight Camera/Canvas/HP").GetComponent<Slider>();
        if (playerHP == null)
        {
            Debug.LogError(string.Format("[{0}:{1}] Gan not find Player.", this.gameObject.name.ToString(), this.name.ToString()));
            return;

        }
        playerHP.maxValue = playerScirpt.MaxHP;
        playerHP.value = playerHP.maxValue;

        reticle = GameObject.Find("Reticle").GetComponent<Image>();
        if (ConsoleDebug.Instance.IsNull(this.name.ToString(), reticle.name.ToString(), (Object)reticle)) return;
        
            reticle.color = Color.gray;
            reticleDefaultPosition = reticle.transform.position;
        

        enemyHPPrefab = GameObject.Find("EnemyHP");
        if (ConsoleDebug.Instance.IsNull(this.name.ToString(), enemyHPPrefab.name.ToString(), (Object)enemyHPPrefab)) return;

        hpCanvas = GameObject.Find("HPCanvas").GetComponent<Canvas>();
        if (ConsoleDebug.Instance.IsNull(this.name.ToString(), hpCanvas.name.ToString(), (Object)hpCanvas)) return;

        enemyCount = GameObject.Find("EnemyCount").GetComponent<TextMeshProUGUI>();
        if (ConsoleDebug.Instance.IsNull(this.name.ToString(), enemyCount.name.ToString(), (Object)enemyCount)) return;

        SetEnemyCountText();
        // enemyCount.text = StageManager.Instance.EnemyCount + " / " + StageManager.Instance.TotalEnemyCount;

    }

    public void SetTarget(Transform target)
    {
        if (reticle == null) return;
        if (target != null)
        {
            reticle.color = Color.red;
            reticle.transform.position = Camera.main.WorldToScreenPoint(target.position + target.up);
        }
        else
        {
            reticle.color = Color.gray;
            reticle.transform.position = reticleDefaultPosition;
        }
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
