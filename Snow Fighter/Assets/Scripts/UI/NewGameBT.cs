using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGameBT : MonoBehaviour
{
    public void NewGame()
    {
        DataController.Instance.MakeNewData();
        DataController.Instance.SaveGameData();

    }
}
