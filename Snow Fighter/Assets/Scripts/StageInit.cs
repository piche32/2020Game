using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageInit : MonoBehaviour
{
    [SerializeField] int stageNum = (int)StageNum.None;
    // Start is called before the first frame update
    void Start()
    {
        if (stageNum != (int)StageNum.None)
            GameManagerScript.Instance.init(stageNum);
        else GameManagerScript.Instance.init();
        StageManager.Instance.init();
        GameObject.Find("UIManager").GetComponent<UIManager>().SetEnemyCountText();
    }

}
