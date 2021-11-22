using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHP : EnemyHPScript
{
    bool isRendered;
    TextMeshProUGUI hpText;

    protected override void Awake()
    {
        base.Awake();
        isRendered = false;
        hpText = this.transform.Find("Text").GetComponent<TextMeshProUGUI>();
        ConsoleDebug.IsNull(this.name, "hpText", hpText);

    }
    protected override void LateUpdate()
    {
    }


    public override void SetEnemyHPSlider(float hp)
    {
        if (!isRendered)
        {
            setHPRender(true);
            isRendered = true;
        }
        hpSlider.value = hp;
        hpText.text = string.Format("{0}/{1} ({2:0.##}%)", hp, hpSlider.maxValue, (hp/hpSlider.maxValue)*100.0f);
    }


}
