using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class GameData
{
    public int Stage = (int)StageNum.Tutorial;
    //public int Stage { get { return stage; } }

    public int PreStage = (int)StageNum.Tutorial;
    //public int PreStage { get { return preStage; } }

    public float BGMVolume = 1.0f;
    public float SFXVolume = 1.0f;

    public void ChangeStage(int nextStage)
    {
        PreStage = Stage;
        Stage = nextStage;
    }
}
