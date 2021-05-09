using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 스테이지 정보
 - 총 Enemy 수
 - 남은 Enemy 수
 - 진행 시간
 - 눈 던진 횟수
*/

public class StageManager : Singleton<StageManager>
{

    [SerializeField] int totalEnemyCount = 0;
    public int TotalEnemyCount { get { return totalEnemyCount; } }

    int enemyCount;
    public int EnemyCount { get { return enemyCount; } set { enemyCount = value; } }

    float runningTime;
    public float RunningTime { get { return runningTime; } }

    int attackedCount;
    public int AttackedCount { get { return attackedCount; } }

    int score;
    public int Score { get { return score; } }
    
    // Start is called before the first frame update
    void Awake()
    {
        init();
    }

    private void FixedUpdate()
    {
        runningTime += Time.deltaTime;
    }

    public void init()
    {
        totalEnemyCount = 0;
        GameObject enemys = GameObject.Find("Enemys");
        int childIdx = enemys.transform.childCount;
        for(int i = 0; i < childIdx; i++)
            if (enemys.transform.GetChild(i).gameObject.activeSelf) totalEnemyCount++;
        enemyCount = totalEnemyCount;
        runningTime = 0.0f;
        attackedCount = 0;
        score = 0;
    }


    public void setAttackedCount()
    {
        attackedCount++;
    }


    public void setScore()
    {
        if (totalEnemyCount == 0) totalEnemyCount = 1;
        score = totalEnemyCount * 10;

        //공 던진 횟수에 따른 점수 계산
        int rate = attackedCount / totalEnemyCount; 
        if (rate < 3)
            score += rate * 100;
        else if (rate < 10)
            score += rate * 25;
        else if (rate < 50)
            score += rate * 3;
        else
            score += rate;

        //클리어 시간에 따른 점수 계산
        rate = (int)runningTime / totalEnemyCount;
        if (rate < 30)
            score += rate * 100;
        else if (rate < 60)
            score += rate * 40;
        else if (rate < 100)
            score += rate * 20;
        else
        {
            while(rate / 10 < 1000)
                rate /= 10;
            score += rate;
        }
    }
}
