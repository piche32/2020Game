using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPScript : MonoBehaviour
{
    protected Slider hpSlider;

    float time = 0.0f;
    
    bool isPlayingAni;
    bool isBlinking;

    Transform enemy;
    Transform player;

    [SerializeField] float aniTime = 2.0f;
    [SerializeField] float blinkSpeed = 0.2f;
    Canvas canvas;
    Camera hpCamera;
    RectTransform rectParent;
    RectTransform rectHp;

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        hpSlider = GetComponent<Slider>();
        isPlayingAni = false;
        isBlinking = false;
        enemy = null;
        player = GameObject.FindGameObjectWithTag("Player").transform;

        canvas = GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>().HpCanvas;
        transform.SetParent(canvas.transform);
        hpCamera = canvas.worldCamera;
        rectParent = canvas.GetComponent<RectTransform>();
        rectHp = GetComponent<RectTransform>();
    }


    // Update is called once per frame
    protected virtual void LateUpdate()
    {
        if (enemy == null) return;
        enemyHPUpdate();
        MaskableGraphic[] ui = hpSlider.GetComponentsInChildren<MaskableGraphic>();
        if (ui[0].enabled)
        {

            Vector3 screenPos = Camera.main.WorldToScreenPoint(enemy.transform.position + new Vector3(0, 2.0f, 0));

            if(screenPos.z < 0.0f)
            {
                screenPos *= -1.0f;
            }

            if(Vector3.Distance(player.position, enemy.position) > 10.0f)
            {
                rectHp.localScale = rectHp.localScale * 0.0f;
            }
            else
            {
                rectHp.localScale = Vector3.one * (10.0f - Vector3.Distance(player.position, enemy.position)) / 10.0f;
            }

            Vector2 localPos = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, screenPos, hpCamera, out localPos); //스크린 좌표를 체력바 UI 캔버스 좌표로 변환
            rectHp.localPosition = localPos;
        }
    }
    public virtual void SetEnemyHPSlider(float hp)
    {
        isPlayingAni = true;

        hpSlider.value = hp;
        setHPRender(true);
        time = 0.0f;
        StopBlinking();
        isBlinking = false;
    }
    public void InitEnemyHPSlider(Transform enemyTrans, float maxHP)
    {
        enemy = enemyTrans;
        //enemyHP = enemy.GetComponentInChildren<Slider>(); //enemyCanvas.GetComponentInChildren<Slider>();
        hpSlider.maxValue = maxHP;
        hpSlider.value = hpSlider.maxValue;

        setHPRender(false);
    }

    protected void setHPRender( bool isOn)
    {

        MaskableGraphic[] sliderUI = hpSlider.GetComponentsInChildren<MaskableGraphic>();
        foreach (MaskableGraphic ui in sliderUI)
        {
            ui.enabled = isOn;
        }
    }

    void blinkHP()
    {
        MaskableGraphic[] sliderUI = hpSlider.GetComponentsInChildren<MaskableGraphic>();
        foreach (MaskableGraphic ui in sliderUI)
        {
            ui.enabled = !ui.enabled;
        }
    }

    protected virtual void enemyHPUpdate()
    {
        if (isPlayingAni)
        {
            if (hpSlider.value > 20) //value 값 다시 확인하기
            {
                time += Time.deltaTime;
                if (time > aniTime)
                {
                    //if (isBlinking)
                    //{
                    //    isBlinking = false;
                    //    StopBlinking();
                    //}
                    setHPRender(false);
                    isPlayingAni = false;
                }
                //else if (time > aniTime - 3)
                //{
                //    if (isBlinking) return;
                //    isBlinking = true;
                //    StartBlinking();
                //}
            }
            else
            {
                //warning 사인 그리기
                if (hpSlider.value <= 0)
                { 
                    isBlinking = false;
                    StopBlinking();
                    setHPRender(false);
                    return;
                }

                if (isBlinking) return;
                isBlinking = true;
                StartBlinking();
            }
        }
    }

    IEnumerator Blink()
    {
        while (true)
        {
            blinkHP();
            yield return new WaitForSeconds(blinkSpeed);
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
