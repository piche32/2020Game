using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDamaged : MonoBehaviour
{
    Image damageImage = null;
    [SerializeField] Color flashColor = new Color(1f, 0f, 0f, 0.5f);
    [SerializeField] float flashSpeed = 5.0f;
    [SerializeField] float flashTime = 0.5f;

    float time = 0.0f;

    void Start()
    {
        damageImage = this.transform.Find("Sight Camera").transform.Find("Canvas").transform.Find("DamageImage").GetComponent<Image>();
    }

    public void OnDamagedImage()
    {
        StartCoroutine(Damaged());
    }

    IEnumerator Damaged()
    {
        time = 0.0f;
        if (damageImage == null) yield break;
        damageImage.color = flashColor;
        yield return null;

        while (time < flashTime) {
            time += Time.deltaTime;
            if (damageImage == null) { yield break; }
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
