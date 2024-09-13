using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// 오디오 타입
/// </summary>
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
/// <summary>
/// 사운드를 실행할 수 있는 사운드 매니저 
/// </summary>
public class SoundManager : MonoBehaviour
{
    #region 변수 선언부
    /// <summary>
    /// 싱글톤 정적 변수
    /// 접근시 SoundManager.inst로 접근 가능
    /// </summary>
    public static SoundManager Instance { get; private set; }
    /// <summary>
    /// 오디오 소스 컴포넌트
    /// </summary>
    AudioSource _sound;
    
    [Header("ListofFiles")]
    /// <summary>
    /// 사운드 파일 리스트
    /// </summary>
    List<AudioClip> _clips = new List<AudioClip>();

    /// <summary>
    /// 백그라운드 사운드
    /// </summary>
    [Header("BackGorundMusic")]
    public AudioClip _BGM = null;
    public AudioClip _BGM0 = null;
    public AudioClip _BGM1 = null;
    public AudioClip _BGM2 = null;

    /// <summary>
    /// 이펙트 사운드
    /// </summary>
    [Header("SoundFile")]
    public AudioClip _Click = null;
    public AudioClip _Ball01 = null;
    public AudioClip _Ball02 = null;
    public AudioClip _Ball03 = null;
    public AudioClip _Win = null;
    public AudioClip _Defeat = null;
    public AudioClip _Dodge = null;
    #endregion

    #region 내부 함수 구현부
    /// <summary>
    /// SoundManager가 처음 시작될때(렌더링 전) 실행되는 함수
    /// Start보다 우선으로 실행됨.
    /// </summary>
    private void Awake()
    {
        // Singleton을 사용할때 필요한 선언부
        // 객체화 하여 정적변수 Instance 에 할당해준다.
        Instance = this;
        // DontDestroyOnLoad에 오브젝트를 등록해주면 씬이 변경되어도 삭제되지 않는다.
        // 씬마다 새롭게 생성된다면 생성이 안되도록 변경해야함.
        // 예를 들면 Instance가 null일 때만 현재 객체가 생성되는 를 정적 변수에 담아주면 여러번 SoundManager를 생성하더라도 정적 변수는 하나만 생성되게 된다.
        //if (Instance == null)
        //{
        //    Instance = this;
        //    DontDestroyOnLoad(this.gameObject);
        //}
        

        // 오디오 소스 컴포넌트를 가져온다.
        _sound = GetComponent<AudioSource>();
        // 사운드 파일 리스트를 초기화한다. 오디오 타입의 end == 타입 전체 수(12) 
        // 즉 AudioClip 리스트의 크기를 12개 만큼 생성
        _clips = new List<AudioClip>(new AudioClip[(int)AudioType.end]);

        Init();

        // PlayerSettings에서 플랫폼 별로 Resolution항목에서 윈도우 크기에 맞게 조절할 수 있습니다.
        //int setWidth    = 760; 
        //int setHeight   = 1280;
        //Screen.SetResolution(setWidth, setHeight, false);
    }
    /// <summary>
    /// AudioClip 리스트에 사운드 파일을 초기화한다.
    /// </summary>
    private void Init()
    {
        // 설정된 오디오 클립 오브젝트를 리스트에 AudioType 순서에 맞게 저장한다.
        _clips[(int)AudioType.Click] = _Click;
        _clips[(int)AudioType.Ball01] = _Ball01;
        _clips[(int)AudioType.Ball02] = _Ball02;
        _clips[(int)AudioType.Ball03] = _Ball03;
        _clips[(int)AudioType.Win] = _Win;
        _clips[(int)AudioType.Defeat] = _Defeat;
        _clips[(int)AudioType.Dodge] = _Dodge;

        _clips[(int)AudioType.BGM] = _BGM;
        _clips[(int)AudioType.BGM0] = _BGM0;
        _clips[(int)AudioType.BGM1] = _BGM1;
        _clips[(int)AudioType.BGM2] = _BGM2;

    }
    #endregion

    #region 공개 함수 구현부
    /// <summary>
    /// AudioType에 해당하는 사운드를 반복해서 재생한다.
    /// </summary>
    /// <param name="Sound"></param>
    public void BSound(AudioType type)
    {
        if (_sound.isPlaying)
            _sound.Stop();

        _sound.clip = _clips[(int)type];
        _sound.volume = 0.3f;
        _sound.Play();
    }

    /// <summary>
    /// AudioType에 해당하는 사운드를 한번만 재생한다.
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

    public void Stop()
    {
        _sound.clip = null;
    }
    #endregion

}

