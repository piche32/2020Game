using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;


public class SoundController : MonoBehaviour
{
    AudioSource bgmSource = null;
    [SerializeField] Slider bgmSlider = null;

    List<AudioSource> sfxSources = null;
    Dictionary<String, AudioClip> sfxClips = null;
    [SerializeField] Slider sfxSlider = null;

    // Start is called before the first frame update
    void Start()
    {
        bgmSource = transform.Find("BGM").GetComponent<AudioSource>();

        Transform sfx = transform.Find("SFX");
        sfxSources = new List<AudioSource>();
        foreach (AudioSource audio in sfx.GetComponents<AudioSource>())
        {
            sfxSources.Add(audio);
        }

        bgmSource.volume = DataController.Instance.gameData._BGMVolume;
        foreach (AudioSource audio in sfxSources)
        {
            audio.volume = DataController.Instance.gameData._SFXVolume;
        }

        if (bgmSlider == null && sfxSlider == null)
        {
            Transform setting = GameObject.Find("Player/Sight Camera/Canvas/Setting").transform;
            bgmSlider = setting.Find("BGM").GetChild(0).GetComponent<Slider>();
            sfxSlider = setting.Find("SFX").GetChild(0).GetComponent<Slider>();
        }

        if (bgmSlider == null)
        {
            Debug.LogError("[SoundController]bgmSilder is missing");
        }
        if (sfxSlider == null)
        {
            Debug.LogError("[SoundController]sfxSilder is missing");
        }
        bgmSlider.value = bgmSource.volume;
        sfxSlider.value = DataController.Instance.gameData._SFXVolume;

        sfxClips = new Dictionary<string, AudioClip>();
        AudioClip[] temp = Resources.LoadAll<AudioClip>("Sounds/SFX");
        foreach (AudioClip clip in temp)
        {
            sfxClips.Add(clip.name, clip);
        }
        //Debug.Log("[SoundController]sfxClips.Count: " + sfxClips.Count);
        if (sfxClips.Count == 0)
            Debug.LogError("[SoundController]sfxClips is empty");

        #region eventSetting
        EventContainer.Instance.Events["OnPlayerAttack"].AddListener(()=> {
            PlaySFX("playerAttack", 0.8f, 1.8f);
        });
        
        EventContainer.Instance.Events["OnPlayerDamaged"].AddListener(()=> {
            PlaySFX("playerAttacked", 6.5f, 7.5f);
        });
        

        #endregion eventSetting
    }

    public void SetBGMVolume()
    {
        bgmSource.volume = bgmSlider.value;
        DataController.Instance.gameData.ChangeVolume("BGM", bgmSlider.value);
    }
    public void SetSFXVolume()
    {
        foreach (AudioSource audio in sfxSources)
            audio.volume = sfxSlider.value;
        DataController.Instance.gameData.ChangeVolume("BGM", sfxSlider.value);
    }

    public void PlaySFX(String name, float startTime, float endTime = 0.0f)
    {
        AudioSource source = null;
        foreach (AudioSource audio in sfxSources)
        {
            if (audio.isPlaying) continue;
            source = audio;
            break;
        }
        if (source == null)
        {
            source = transform.Find("SFX").gameObject.AddComponent<AudioSource>();
            sfxSources.Add(source);
        }

        source.clip = sfxClips[name];
        if (source.clip == null || source.clip.name != name)
        {
            Debug.LogError("[SoundController]Sound Clip \"" + name + "\" can not find");
            return;
        }
        source.time = startTime;
        source.Play();
        if (endTime > 0.0f)
        {
            source.SetScheduledEndTime(AudioSettings.dspTime + (endTime - startTime));
        }

    }
    public void PlaySFX(String name)
    {
        AudioSource source = null;
        foreach (AudioSource audio in sfxSources)
        {
            if (audio.isPlaying) continue;
            source = audio;
            break;
        }
        if (source == null)
        {
            source = transform.Find("SFX").gameObject.AddComponent<AudioSource>();
            sfxSources.Add(source);
        }
        source.time = 0.0f;
        source.clip = sfxClips[name];
        if (source.clip == null || source.clip.name != name)
        {
            Debug.LogError("[SoundController]Sound Clip \"" + name + "\" can not find");
            return;
        }
        source.Play();
    }
}
