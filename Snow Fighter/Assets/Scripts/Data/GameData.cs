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

    //private float BGMVolume = 1.0f;
    public float _BGMVolume = 1.0f;// { get { return BGMVolume; } }
    //private float SFXVolume = 1.0f;
    public float _SFXVolume = 1.0f; //{ get { return SFXVolume; } }

    public void ChangeStage(int nextStage)
    {
        PreStage = Stage;
        Stage = nextStage;
    }

    public void ChangeVolume(String sound, float volume)
    {
        if (sound == null) return;
        if(sound == "BGM")
        {
            _BGMVolume = volume;
        }
        else if(sound == "SFX")
        {
            _SFXVolume = volume;
        }
    }
}
