using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialUI : MonoBehaviour
{
    TextMeshProUGUI tmp;

    int curIndex;
    bool isDone;

    GameObject player;

    float time;

    public bool isObjDestroyed;

    public bool isHamburgerDestroyed;

    // Start is called before the first frame update
    void Start()
    {
        tmp = GameObject.Find("TutorialText").GetComponent<TextMeshProUGUI>();

        player = GameObject.Find("Player");

        curIndex = 0;
        isDone = false;
        time = 0.0f;
        isObjDestroyed = false;
        isHamburgerDestroyed = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch (curIndex)
        {
            case 0:
                tmp.text = "왼쪽의 조이스틱을 이용해서 \n움직일 수 있습니다. \n조이스틱을 조작해보세요.";
                if (player.GetComponent<PlayerScript>().IsMoving)
                {//어떻게 해야 객체지향적 프로그래밍일까...
                    isDone = true;
                }
                if(isDone)
                {
                    tmp.text = "잘했습니다.";
                    time += Time.deltaTime;
                    if (time > 3.0f) {
                        curIndex++;
                        time = 0.0f;
                        isDone = false;
                    }
                }
                break;
            case 1:
                tmp.text = "다음은 화면 회전입니다. \n오른쪽 화면을 드래그해보세요.";
                if (player.GetComponentInChildren<PlayerSightScript>().IsCameraRotating)
                {
                    isDone = true;
                }
                if(isDone) { 
                    tmp.text = "잘했습니다.";
                    time += Time.deltaTime;
                    if (time > 3.0f) {
                        curIndex++;
                        time = 0.0f;
                        isDone = false;
                    }
                }
                break;
            case 2:
                time += Time.deltaTime;
                if(time < 3.0f) {
                tmp.text = "점프 버튼을 누르면 \n점프할 수 있습니다.";
                }
                else
                {
                    tmp.text = "공격 버튼으로 공격을 해보세요.";
                }
                if (player.GetComponent<PlayerScript>().IsThrowing)
                {
                    isDone = true;
                    time = 0.0f;
                }
                if (isDone)
                {
                    time += Time.deltaTime;
                    tmp.text = "잘했습니다.";
                    if (time > 3.0f)
                    {
                        curIndex++;
                        isDone = false;
                        time = 0.0f;
                    }
                }
                break;
            case 3:
                tmp.text = "상자를 맞혀 \n내구도를 0으로 만들어 보세요.";
                if (isObjDestroyed) isDone = true;
                if (isDone)
                {
                    tmp.text = "잘했습니다.";
                    time += Time.deltaTime;
                    if(time > 3.0f)
                    {
                        curIndex++;
                        isDone = false;
                        time = 0.0f;
                    }
                }
                //내구도 확인 -> 잘했습니다. 튜토리얼이 모두 끝났습니다. -> success 화면 -> stage 1
                break;
            case 4:
                tmp.text = "햄버거를 먹으면 체력이 회복됩니다.";
                if (isHamburgerDestroyed) isDone = true;
                if (isDone)
                {
                    tmp.text = "잘했습니다.";
                    time += Time.deltaTime;
                    if (time > 3.0f)
                    {
                        curIndex++;
                        isDone = false;
                        time = 0.0f;
                    }
                }
                //내구도 확인 -> 잘했습니다. 튜토리얼이 모두 끝났습니다. -> success 화면 -> stage 1
                break;
            case 5:
                tmp.text = "튜토리얼이 모두 끝났습니다.";
                time += Time.deltaTime;
                if(time > 3.0f)
                {
                    GameManagerScript.Instance.Success();
                }
                break;

        }
    }
}
