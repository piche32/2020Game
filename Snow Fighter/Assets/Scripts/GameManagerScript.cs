using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 - 씬 전환
 - 씬 전환 시 넘겨줄 데이터
 */

public enum StageNum
{
    None = -1,
    Start = 0,
    GameOver = 1,
    Success = 2,
    Tutorial,
    Stage1,
    Stage2,
    Num,
}

public class GameManagerScript : Singleton<GameManagerScript>
{
    protected GameManagerScript() { }
    
    int stage = (int)StageNum.Tutorial;
    public int Stage {
        get { return stage; }
        set {
            preStage = stage;
            stage = value;
        }
    }
    int preStage = 3;
    public int PreStage { get { return preStage; } }
    

    int totalEnemyCount;
    public int TotalEnemyCount { get { return totalEnemyCount; } }
    int enemyCount;
    public int EnemyCount { get { return enemyCount; } }
    float runningTime;
    public float RunningTime { get { return runningTime; } }
    int attackedCount;
    public int AttackedCount { get { return attackedCount; } }
    int score;
    public int Score { get { return score; } }

    public void Awake()
    {
        init();
    }

    public void Gameover()
    {
        preStage = stage;
        stage = 1;

        DataController.Instance.gameData.ChangeStage(stage);
        SceneManager.LoadScene(stage);
    }

    public void Success()
    {
        StageManager s = GameObject.Find("(singleton) StageManager").GetComponent<StageManager>();
        s.setScore();

        score = s.Score;
        totalEnemyCount = s.TotalEnemyCount;
        enemyCount = s.EnemyCount;
        attackedCount = s.AttackedCount;
        runningTime = s.RunningTime;

        preStage = stage;
        stage = 2;

        DataController.Instance.gameData.ChangeStage(preStage+1);
        SceneManager.LoadScene(stage);
    }
    public void Restart()
    {
        int swap = preStage;
        preStage = stage;
        stage = swap;

        DataController.Instance.gameData.ChangeStage(stage);
        SceneManager.LoadScene(stage);
    }

    public void LoadGame()
    {
        if (stage >= (int)StageNum.Num)
        {
            preStage = (int)StageNum.None;
            stage--;
            DataController.Instance.gameData.ChangeStage(stage);
            DataController.Instance.SaveGameData();
            SceneManager.LoadScene((int)StageNum.Start);
            return;
        }
        SceneManager.LoadScene(stage);
    }

    public void init()
    {
        stage = DataController.Instance.gameData.Stage;
        preStage = DataController.Instance.gameData.PreStage;
        if (stage >= (int)StageNum.Tutorial)
        {
            score = 0;
            totalEnemyCount = 0;
            totalEnemyCount = 0;
            enemyCount = 0;
            attackedCount = 0;
            runningTime = 0.0f;
        }
    }

    public void init(int stageNum)
    {
        DataController.Instance.gameData.ChangeStage(stageNum);
        init();
    }

    public void GoToNext()
    {
        int swap = preStage;
        preStage = stage;
        stage = swap + 1;

        //마지막까지 다 깼을 경우
        if(stage >= (int)StageNum.Num)
        {
            preStage = (int)StageNum.None;
            stage--;
            DataController.Instance.gameData.ChangeStage(stage);
            DataController.Instance.SaveGameData();
            SceneManager.LoadScene((int)StageNum.Start);
            return;
        } 
        DataController.Instance.gameData.ChangeStage(stage);
        DataController.Instance.SaveGameData();
        SceneManager.LoadScene(stage);
    }

    public void GoToMain()
    {
        SceneManager.LoadScene((int)StageNum.Start);
    }
}
