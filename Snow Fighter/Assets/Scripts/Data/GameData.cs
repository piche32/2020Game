using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class GameData
{
    private int stage = (int)StageNum.Tutorial;
    public int Stage { get { return stage; } }

    private int preStage = (int)StageNum.Start;
    public int PreStage { get { return stage; } }

    public float BGMVolume = 1.0f;
    public float SFXVolume = 1.0f;

    public void ChangeStage(int nextStage)
    {
        preStage = stage;
        stage = nextStage;
    }
}
