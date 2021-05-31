using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Stage2Story : MonoBehaviour
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
                tmp.text = "저 이상한 사람들을 쫓다보니\n어느새 여기까지 와버렸어...";
                if(time > 3.0f)
                {
                    time = 0.0f;
                    curIndex++;
                }
                break;
                
            case 1:
                tmp.text = "여긴 저 이상한 사람들이\n훨씬 더 많잖아..";
                if(time > 3.0f)
                {
                    time = 0.0f;
                    curIndex++;
                }
                break;
                
            case 2:
                tmp.text = "저기 저 사람이 이상한 사람들을\n끌어들이는 거 같아. 처치하자!";
                if(time > 3.0f)
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
