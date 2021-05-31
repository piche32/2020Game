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
    Start = 0,
    GameOver = 1,
    Success = 2,
    Tutorial,
    Stage1,
    Stage2,
}

public class GameManagerScript : Singleton<GameManagerScript>
{
    protected GameManagerScript() { }
    
    int stage = 3;
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
        stage = DataController.Instance.gameData.Stage;
        preStage = DataController.Instance.gameData.PreStage;
        if(stage >= (int)StageNum.Tutorial )
        {
            init();
        }
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

        DataController.Instance.gameData.ChangeStage(stage);
        SceneManager.LoadScene(stage);
    }
    public void Restart()
    {
        int swap = preStage;
        preStage = stage;
        stage = preStage;

        DataController.Instance.gameData.ChangeStage(stage);
        SceneManager.LoadScene(stage);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(stage);
    }

    void init()
    {
        score = 0;
        totalEnemyCount = 0;
        enemyCount = 0;
        attackedCount = 0;
        runningTime = 0.0f;
    }
}
