using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class DataController : Singleton<DataController>
{
    public string GameDataFileName = ".json"; //이름 변경 절대 X

    private GameData _gameData;
    public GameData gameData
    {
        get
        {
            if (_gameData == null)
            {
                LoadGameData();
                SaveGameData();
            }
            return _gameData;
        }
    }

    public void LoadGameData()
    {
        string filePath = Application.persistentDataPath + GameDataFileName;

        if (File.Exists(filePath))
        {
            Debug.Log("불러오기 성공!");
            string FromJsonData = File.ReadAllText(filePath);
            _gameData = JsonUtility.FromJson<GameData>(FromJsonData);
        }
        else
        {
            MakeNewData();
        }
    }

    public void SaveGameData()
    {
        string ToJsonData = JsonUtility.ToJson(gameData);
        string filePath = Application.persistentDataPath + GameDataFileName;
        File.WriteAllText(filePath, ToJsonData);
        Debug.Log("저장 완료");
    }

    public void MakeNewData()
    {
        GameData temp = null;
        Debug.Log("새로운 파일 생성");
        if (_gameData != null)
        {
            temp = _gameData;
        }
        _gameData = new GameData();

        if (temp != null) //만약 소리 설정한 게 있다면 유지
        {
            _gameData.BGMVolume = temp.BGMVolume;
            _gameData.SFXVolume = temp.SFXVolume;
        }

    }

    private void OnApplicationQuit()
    {
        SaveGameData();
    }
}
