using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackingTestDurability : MonoBehaviour
{
    GameObject sliderGO = null;
    Slider slider = null;

    Transform player;
    
    Canvas canvas;
    Camera hpCamera;
    
    RectTransform rectParent;
    RectTransform rectHp;

    float time = 0.0f;
    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.Find("Player").transform;
        if(player == null)
        {
            Debug.LogError(string.Format("[{0}:{1}] Gan not find Player.", this.gameObject.name.ToString(), this.name.ToString()));
            return;
        }


        UIManager ui = GameObject.Find("UIManager").GetComponent<UIManager>();
        sliderGO = GameObject.Instantiate(ui.EnemyHPPrefab);
        if(sliderGO == null)
        {
            Debug.LogError("[AttackingTestDurability]Can not instantiate Durability Slider.");
            return;
        }

        sliderGO.transform.parent = ui.HpCanvas.transform;


        slider = sliderGO.GetComponent<Slider>();
        if (slider == null)
        {
            Debug.LogError(string.Format("[{0}:{1}]Can not find slider.", this.gameObject.name.ToString(), this.name.ToString()));
            return;
        }

        canvas = ui.HpCanvas;
        hpCamera = canvas.worldCamera;

        rectParent = canvas.GetComponent<RectTransform>();
        rectHp = sliderGO.GetComponent<RectTransform>();
            }
   
    public void Init(float maxValue)
    {
        slider.maxValue = maxValue;
        sliderGO.SetActive(false);
    }

    private void Update()
    {
        if (sliderGO.activeSelf)
        {
            time += Time.deltaTime;
            if(time > 3.0f)
            {
                sliderGO.SetActive(false);
            }
        }

    }

    void LateUpdate()
    {
        if (!sliderGO.activeSelf) return;
        setUIScale();
    }

    void setUIScale()
    {
       // MaskableGraphic[] ui = slider.GetComponentsInChildren<MaskableGraphic>();
        //if (ui[0].enabled)
        //{
            Vector3 screenPos = Camera.main.WorldToScreenPoint(this.transform.position + new Vector3(0, 2.0f, 0));

            if (screenPos.z < 0.0f)
            {
                screenPos *= -1.0f;
            }

            if (Vector3.Distance(player.position, this.transform.position) > 10.0f)
            {
                rectHp.localScale = rectHp.localScale * 0.0f; //안보이게 하기 위함
            }
            else
            {
            rectHp.localScale = Vector3.one / Vector3.Distance(player.position, this.transform.position) * 5.0f;
            }

            Vector2 localPos = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, screenPos, hpCamera, out localPos); //스크린 좌표를 체력바 UI 캔버스 좌표로 변환
            rectHp.localPosition = localPos;
       // }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (slider.value <= 0.0f) return;
        if (other.gameObject.tag == "SnowBall")
        {
            if (!sliderGO.activeSelf) sliderGO.SetActive(true);
            time = 0.0f;
        }
    }
    public void SetDurability(float value)
    {
        slider.value = value;
        if(value <= 0.0f)
        {
            sliderGO.SetActive(false);
        }
    }

}
