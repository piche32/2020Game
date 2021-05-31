using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Stage1Story : MonoBehaviour
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
                tmp.text = "1년 내내 따뜻했던 우리 마을에\n언젠가부터 눈이 내리기 시작했다.";
                if (time > 3.0f)
                {
                    time = 0.0f;
                    curIndex++;
                }
                break;

            case 1:
                tmp.text = "눈이 내리면서 마을 곳곳에\n검은 사람들이 나타났다.";
                if (time > 3.0f)
                {
                    time = 0.0f;
                    curIndex++;
                }
                break;

            case 2:
                tmp.text = "다시 마을을 따뜻하게 만들기 위해\n저 사람들을 처치하자.";
                if (time > 3.0f)
                {
                    time = 0.0f;
                    curIndex++;
                }
                break;

            case 3:
                tmp.transform.parent.gameObject.SetActive(false);
                break;


        }
    }
}
