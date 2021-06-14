using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartBT : MonoBehaviour
{
    public void GameStart()
    {
        GameManagerScript.Instance.Stage = DataController.Instance.gameData.Stage;
        GameManagerScript.Instance.LoadGame();
    }
}
