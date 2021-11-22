using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDamaged : MonoBehaviour
{
    Image damageImage = null;
    [SerializeField] Color flashColor = new Color(1f, 0f, 0f, 0.5f);
    [SerializeField] Color icedColor = new Color(0f, 0.8f, 1f, 0.5f);
    [SerializeField] float flashSpeed = 5.0f;
    [SerializeField] float flashTime = 0.5f;
    [SerializeField] float stunTime = 1f;

    FrostEffect frostEffect;
    

    void Start()
    {
        damageImage = this.transform.Find("Sight Camera").transform.Find("Canvas").transform.Find("DamageImage").GetComponent<Image>();
        frostEffect = this.GetComponentInChildren<FrostEffect>();
        frostEffect.enabled = false ;
        EventContainer.Instance.Events["OnPlayerDamaged"].AddListener(() => {
            if (damageImage == null) return;
            damageImage.color = flashColor;
           
            StartCoroutine(Damaged());
        });
        EventContainer.Instance.Events["OnPlayerIced"].AddListener(() => {
            StartCoroutine(Iced());

        });
    }

    IEnumerator Iced()
    {
        frostEffect.enabled = true;
        frostEffect.FrostAmount = 0.3f;
        float time = 0.0f;
        yield return null;
        while(time < stunTime)
        {
            if (stunTime * 0.8f > time)
                frostEffect.FrostAmount += 0.05f;
            else
                frostEffect.FrostAmount -= 0.2f;
            yield return new WaitForSeconds(0.1f);

            time += 0.1f;
        }
        frostEffect.enabled = false;
        yield return null;
    }

    public void OnDamagedImage()
    {
        StartCoroutine(Damaged());
    }

    IEnumerator Damaged()
    {
        float time = 0.0f;

        while (time < flashTime) {
            time += Time.deltaTime;
            if (damageImage == null) { yield break; }
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
