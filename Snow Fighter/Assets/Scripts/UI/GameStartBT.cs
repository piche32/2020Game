using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartBT : MonoBehaviour
{
    public void GameStart()
    {
        GameManagerScript.Instance.LoadGame();
    }
}
