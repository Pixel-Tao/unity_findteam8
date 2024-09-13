using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 시작 화면 씬 컴포넌트
/// </summary>
public class StartScene : MonoBehaviour
{
    #region 변수 선언부
    /// <summary>
    /// FadeIn, FadeOut 애니메이션을 가지고 있는 오브젝트
    /// </summary>
    public GameObject fade;
    /// <summary>
    /// 마우스 커서 오브젝트
    /// </summary>
    public GameObject cursor;
    /// <summary>
    /// 게임 제목 Text 오브젝트
    /// </summary>
    public Text teamName;

    /// <summary>
    /// 노멀 난이도 버튼
    /// </summary>
    public GameObject NormalBtn;
    /// <summary>
    /// 하드 난이도 버튼
    /// </summary>
    public GameObject HardBtn;
    /// <summary>
    /// 헬 난이도 버튼
    /// </summary>
    public GameObject CrazyBtn;
    /// <summary>
    /// 히든1 난이도 버튼
    /// </summary>
    public GameObject HiddenBtn;
    /// <summary>
    /// 히든2 난이도 버튼
    /// </summary>
    public GameObject Hidden2Btn;

    /// <summary>
    /// 게임 시작 공
    /// </summary>
    public GameObject MainBall;
    /// <summary>
    /// 게임 시작 하드공
    /// </summary>
    public GameObject MainBallHard;
    /// <summary>
    /// 게임 시작 헬공
    /// </summary>
    public GameObject MainBallCrazy;

    /// <summary>
    /// 시작 공 애니메이터
    /// </summary>
    public Animator ballAnimator;

    /// <summary>
    /// 버전 텍스트
    /// </summary>
    public Text versionText;
    #endregion

    #region 내부 함수 선언부 (private)
    /// <summary>
    /// StartScene이 시작 할때 한번 실행되는 함수
    /// </summary>
    private void Start()
    {
        // 버전을 표시해줍니다.
        versionText.text = "v" + Application.version;
        // StartScene이 시작될때 디버그 모드가 켜져 있다면 Italic으로 표시합니다. 아니면 Bold로 표시합니다.
        teamName.fontStyle = GameManager.DebugMode ? FontStyle.Italic : FontStyle.Bold;
        // StartScene이 시작될때 기본적으로 난이도가 노멀 난이도로 선택 되도록 해줍니다.
        ClickNormal();
        // StartScene이 시작될때 FadeIn 애니메이션을 실행하여 자연스러운 화면 전환을 해줍니다.
        FadeIn();
        // StartScene이 시작될때 마우스 커서를 활성화 시켜줍니다.
        cursor.SetActive(true);
    }

    /// <summary>
    /// FadeIn 애니메이션을 실행하는 함수
    /// 화면이 점점 밝아지는 효과
    /// </summary>
    private void FadeIn()
    {
        // fade 오브젝트를 활성화 시켜줍니다.
        fade.SetActive(true);
        // fade 애니메이터의 트리거를 FadeIn으로 설정하면 검은 화면에서 서서히 밝아지는 효과를 실행해줍니다.
        fade.GetComponent<Animator>().SetTrigger("FadeIn");
        // 1초 뒤에 fade 오브젝트를 비활성화 하여 다르 UI와 인터렉션 가능 하도록 해줍니다.
        Invoke("HideFade", 1.0f);
    }

    /// <summary>
    /// fade 오브젝트를 비활성화 시켜주는 함수
    /// </summary>
    private void HideFade()
    {
        fade.SetActive(false);
    }

    /// <summary>
    /// 화면이 다른 씬으로 넘어갈때 fade 애니메이션을 실행하고 다른 씬으로 넘어가는 함수
    /// </summary>
    /// <param name="method"></param>
    private void FadeOut(string method)
    {
        // 비활성화 되어 있는 fade 오브젝트를 활성화 시켜줍니다.
        fade.SetActive(true);
        // fade 애니메이터의 트리거를 FadeOut으로 설정하면 밝아진 화면에서 검은 화면으로 서서히 어두워지는 효과를 실행해줍니다.
        fade.GetComponent<Animator>().SetTrigger("FadeOut");
        // 1.2초 뒤에 method 이름을 가진 함수를 실행해줍니다.
        Invoke(method, 1.2f);
    }

    /// <summary>
    /// Invok에서 실행하는 함수로 GameScene으로 넘어가는 함수입니다.
    /// </summary>
    private void MoveGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }
    #endregion

    #region ButtonClickEvent
    // 해당 함수들은 Button 클릭 이벤트 함수들로 ButtonClickEvent region으로 묶어서 관리합니다.
    /// <summary>
    /// 게임 시작할때 실행되는 이벤트 함수 입니다
    /// </summary>
    public void GamePlay()
    {
        // 공을 치는 사운드를 실행하고
        SoundManager.Instance.ESound(AudioType.Ball01);
        // 게임 시작시 공이 위로 올라가는 애니메이션을 실행해줍니다.
        ballAnimator.SetTrigger("MoveUp");
        // FadeOut 함수를 실행하여 FadeOut 애니메이션을 실행하고 1.2초 뒤에 MoveGameScene 함수를 실행하여 GameScene으로 넘어가게 해줍니다.
        FadeOut("MoveGameScene");
    }
    /// <summary>
    /// 노멀모드 버튼을 클릭 했을때 실행되는 이벤트 함수
    /// </summary>
    public void ClickNormal()
    {
        Debug.Log("Normal");
        // 난이도 선택에 따른 시작 버튼 표시
        MainBall.SetActive(true);
        MainBallHard.SetActive(false);
        MainBallCrazy.SetActive(false);
        // 게임 매니저에 노멀모드를 선택하도록 해줍니다.
        GameManager.SetGameMode(GameModeType.Normal);
        // 선택된 공의 애니메이터를 가져와서 ballAnimator에 저장해줍니다.
        ballAnimator = MainBall.GetComponent<Animator>();
    }
    /// <summary>
    /// 하드모드 버튼을 클릭 했을때 실행되는 이벤트 함수
    /// </summary>
    public void ClickHard()
    {
        Debug.Log("Hard");
        // 난이도 선택에 따른 시작 버튼 표시
        MainBall.SetActive(false);
        MainBallHard.SetActive(true);
        MainBallCrazy.SetActive(false);
        // 게임 매니저에 하드모드를 선택하도록 해줍니다.
        GameManager.SetGameMode(GameModeType.Hard);
        // 선택된 공의 애니메이터를 가져와서 ballAnimator에 저장해줍니다.
        ballAnimator = MainBallHard.GetComponent<Animator>();
    }
    /// <summary>
    /// 헬 모드 버튼을 클릭 했을때 실행되는 이벤트 함수
    /// </summary>
    public void ClickCrazy()
    {
        Debug.Log("Crazy");
        // 난이도 선택에 따른 시작 버튼 표시
        MainBall.SetActive(false);
        MainBallHard.SetActive(false);
        MainBallCrazy.SetActive(true);
        // 게임 매니저에 헬모드를 선택하도록 해줍니다.
        GameManager.SetGameMode(GameModeType.Crazy);
        // 선택된 공의 애니메이터를 가져와서 ballAnimator에 저장해줍니다.
        ballAnimator = MainBallCrazy.GetComponent<Animator>();
    }
    /// <summary>
    /// 왼쪽 상단 초크 버튼클릭 했을때 함수
    /// </summary>
    public void ClickHidden()
    {
        Debug.Log("Hidden Stage 1");
        // 게임 매니저에 히든모드를 선택하도록 해줍니다.
        GameManager.SetGameMode(GameModeType.Hidden);
        // 히든모드 버튼을 클릭했을때 흔들리는 애니메이션을 실행해줍니다.
        HiddenBtn.GetComponent<Animator>().SetTrigger("Shake");
        // 클릭 사운드를 실행해줍니다.
        SoundManager.Instance.ESound(AudioType.Click);
        // FadeOut 함수를 실행하여 FadeOut 애니메이션을 실행하고 1.2초 뒤에 MoveGameScene 함수를 실행하여 GameScene으로 넘어가게 해줍니다.
        FadeOut("MoveGameScene");
    }
    /// <summary>
    /// 오른쪽 밑에 초크 버튼 클릭 했을때 함수
    /// </summary>
    public void ClickHidden2()
    {
        Debug.Log("Hidden Stage 2");
        // 게임 매니저에 히든2모드를 선택하도록 해줍니다.
        GameManager.SetGameMode(GameModeType.Hidden2);
        // 히든2모드 버튼을 클릭했을때 흔들리는 애니메이션을 실행해줍니다.
        Hidden2Btn.GetComponent<Animator>().SetTrigger("Shake");
        // 클릭 사운드를 실행해줍니다.
        SoundManager.Instance.ESound(AudioType.Click);
        // FadeOut 함수를 실행하여 FadeOut 애니메이션을 실행하고 1.2초 뒤에 MoveGameScene 함수를 실행하여 GameScene으로 넘어가게 해줍니다.
        FadeOut("MoveGameScene");
    }
    /// <summary>
    /// 제목 텍스트를 클릭 했을때 실행되는 이벤트 함수
    /// </summary>
    public void DebugMode()
    {
        // 디버그 모드를 토글로 변경해줍니다.
        GameManager.SetDebugMode(!GameManager.DebugMode);
        // 디버그 모드가 켜져 있다면 Italic으로 표시합니다. 아니면 Bold로 표시합니다.
        teamName.fontStyle = GameManager.DebugMode ? FontStyle.Italic : FontStyle.Bold;

    }
    #endregion
}
