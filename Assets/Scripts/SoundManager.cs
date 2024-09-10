using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioType
{
    None,
    Click,
    Ball01,
    Ball02,
    Ball03,
    Win,
    Defeat,
    Dodge,

    BGM,
    BGM0,
    BGM1,
    BGM2,
    end,
}
public class SoundManager : MonoBehaviour
{
    public static SoundManager inst { get; private set; }
    AudioSource _sound;
    
    [Header ("ListofFiles")]
    List<AudioClip> _clips          = new List<AudioClip>();

    [Header("BackGorundMusic")]
    public AudioClip _BGM           = null;
    public AudioClip _BGM0          = null;
    public AudioClip _BGM1          = null;
    public AudioClip _BGM2          = null;

    [Header("SoundFile")]
    public AudioClip _Click         = null;
    public AudioClip _Ball01        = null;
    public AudioClip _Ball02        = null;
    public AudioClip _Ball03        = null;
    public AudioClip _Win           = null;
    public AudioClip _Defeat        = null;
    public AudioClip _Dodge         = null;


    private void Awake()
    {
        inst = this;
        
        _sound = GetComponent<AudioSource>();
        _clips = new List<AudioClip>(new AudioClip[(int)AudioType.end]);
        Init();

    }
    void Init()
    {
        _clips[(int)AudioType.Click]  = _Click;
        _clips[(int)AudioType.Ball01] = _Ball01;
        _clips[(int)AudioType.Ball02] = _Ball02;
        _clips[(int)AudioType.Ball03] = _Ball03;
        _clips[(int)AudioType.Win]    = _Win;
        _clips[(int)AudioType.Defeat] = _Defeat;
        _clips[(int)AudioType.Dodge]  = _Dodge;

        _clips[(int)AudioType.BGM]  = _BGM;
        _clips[(int)AudioType.BGM0] = _BGM0;
        _clips[(int)AudioType.BGM1] = _BGM1;
        _clips[(int)AudioType.BGM2] = _BGM2;

    }
    
    /// <summary>
    /// Choose the Backgorund Music and You can Change BGM with AudioType Parameter.
    /// </summary>
    /// <param name="Sound"></param>
    public void BSound(AudioType type)
    {
        if (_sound.isPlaying)
            _sound.Stop();

        _sound.clip = _clips[(int)type];
        _sound.Play();
    }

    /// <summary>
    /// Play Effective Sound, You can Choose the sound with AudioType Parameter.
    /// </summary>
    /// <param name="Sound"></param>
    public void ESound(AudioType type)
    {
        if (!_clips[(int)type])
        {
            Debug.Log("ESound File Void");
            return;
        }
        
        _sound.PlayOneShot(_clips[(int)type]);
    }

}

