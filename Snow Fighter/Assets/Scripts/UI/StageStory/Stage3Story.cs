using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Stage3Story : MonoBehaviour
{
    TextMeshProUGUI tmp;

    int curIndex;
    float time;
    // Start is called before the first frame update
    void Start()
    {
        tmp = GameObject.Find("TutorialText").GetComponent<TextMeshProUGUI>();

        curIndex = 0;
        time = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        switch (curIndex)
        {
            case 0:
                tmp.text = "아까 잡은 마법사가\n여기 위에 보스가 있다고 했어.";
                if (time > 4.0f)
                {
                    time = 0.0f;
                    curIndex++;
                }
                break;

            case 1:
                tmp.text = "보스를 처치하러 가자!";
                if (time > 3.0f)
                {
                    time = 0.0f;
                    curIndex++;
                }
                break;

            case 2:
                tmp.transform.parent.gameObject.SetActive(false);
                break;

        }
    }
}
