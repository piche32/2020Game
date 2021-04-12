using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SuccessStage : MonoBehaviour
{
    TextMeshProUGUI enemyCount = null;
    TextMeshProUGUI attackedCount = null;
    TextMeshProUGUI time = null;
    TextMeshProUGUI score = null;
    // Start is called before the first frame update
    void Start()
    {
        enemyCount = gameObject.transform.Find("EnemyCount").GetComponent<TextMeshProUGUI>();
        attackedCount = gameObject.transform.Find("AttackedCount").GetComponent<TextMeshProUGUI>();
        time = gameObject.transform.Find("Time").GetComponent<TextMeshProUGUI>();
        score = gameObject.transform.Find("Score").GetComponent<TextMeshProUGUI>();

        enemyCount.text = "Kill: " + GameManagerScript.Instance.TotalEnemyCount;
        attackedCount.text = "Attack: " + GameManagerScript.Instance.AttackedCount;
        time.text = "Time: " + (int)GameManagerScript.Instance.RunningTime;
        score.text = GameManagerScript.Instance.Score.ToString();
    }

}
