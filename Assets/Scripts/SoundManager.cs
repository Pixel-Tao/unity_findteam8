using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioType
{
    None,
    Flip,
    ButtonClick,
    file01,
    file02,
    file03,
    file04,

    BGM,
    BGM0,
    BGM1,
    BGM2,
    end,
}
public class SoundManager : MonoBehaviour
{
    public static SoundManager inst;
    AudioSource _sound;
    
    [Header ("ListofFiles")]
    List<AudioClip> _clips          = new List<AudioClip>();

    [Header("BackGorundMusic")]
    public AudioClip _BGM           = null;
    public AudioClip _BGM0          = null;
    public AudioClip _BGM1          = null;
    public AudioClip _BGM2          = null;

    [Header("SoundFile")]
    public AudioClip _Flip          = null;
    public AudioClip _ButtonClick   = null;
    public AudioClip _file01        = null;
    public AudioClip _file02        = null;
    public AudioClip _file03        = null;
    public AudioClip _file04        = null;

    AudioType _type                 = AudioType.None;

    private void Awake()
    {
        inst = this;
        DontDestroyOnLoad(this);
        
        _sound = GetComponent<AudioSource>();
        _clips = new List<AudioClip>(new AudioClip[(int)AudioType.end]);
        Init();

    }
    void Init()
    {
        _clips[(int)AudioType.Flip] = _Flip;
        _clips[(int)AudioType.ButtonClick] = _ButtonClick;
        _clips[(int)AudioType.file01] = _file01;
        _clips[(int)AudioType.file02] = _file02;
        _clips[(int)AudioType.file03] = _file03;
        _clips[(int)AudioType.file04] = _file04;
        _clips[(int)AudioType.BGM] = _BGM;
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

