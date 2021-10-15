using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name; //곡의 이름
    public AudioClip clip; //곡
}

public class SoundManager : MonoBehaviour
{
    // 싱글턴
    static public SoundManager instance;
    #region Singleton
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(this.gameObject);
    }
    #endregion

    public AudioSource[] audioSourceEffects;
    public AudioSource audioSourceBgm;

    public string[] playSoundName;
    
    public Sound[] effects;
    public Sound[] bgmSounds;

    private void Start()
    {
        playSoundName = new string[audioSourceEffects.Length];
    }

    public void PlaySE(string _name)
    {
        for (int i = 0; i < effects.Length; i++)
        {
            if (_name == effects[i].name)
            {
                for (int j = 0; j < audioSourceEffects.Length; j++)
                {
                    if (!audioSourceEffects[j].isPlaying)
                    {
                        playSoundName[j] = effects[i].name;
                        audioSourceEffects[j].clip = effects[i].clip;
                        audioSourceEffects[j].Play();
                        return;
                    }
                }
                Debug.Log("모든 가용 오디오 소스 사용중");
                return;
            }
        }
        Debug.Log(_name + " 사운드가 사운드 매니저에 등록 되지 않았습니다");
    }

    public void StopAllSE()
    {
        for (int i = 0; i < audioSourceEffects.Length; i++)
        {
            audioSourceEffects[i].Stop();
        }
    }

    public void StopSE(string _name)
    {
        for (int i = 0; i < audioSourceEffects.Length; i++)
        {
            if (playSoundName[i] == _name) audioSourceEffects[i].Stop();
            return;
        }
        Debug.Log("재생 중인" + _name + "사운드가 없습니다");
    }
}
