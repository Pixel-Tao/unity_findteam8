using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScene : MonoBehaviour
{
    /// <summary>
    /// FadeIn, FadeOut 애니메이션을 가지고 있는 오브젝트
    /// </summary>
    public GameObject fade;

    #region 내부 함수 선언부 (private)
    /// <summary>
    /// GameScene이 시작될때 실행
    /// </summary>
    private void Start()
    {
        GameStart();
    }
    /// <summary>
    /// 게임 시작
    /// </summary>
    private void GameStart()
    {
        // fade 오브젝트 활성화
        fade.SetActive(true);
        // FadeIn 애니메이션 실행
        fade.GetComponent<Animator>().SetTrigger("FadeIn");
        // 1초 후 HideFade 함수 실행
        Invoke("HideFade", 1.0f);
    }
    /// <summary>
    /// 매 프레임 마다 함수 실행
    /// </summary>
    private void Update()
    {
        // Q키를 누르면 게임오버
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameManager.Instance.GameOver();
        }
        // W키를 누르면 게임클리어
        else if (Input.GetKeyDown(KeyCode.W))
        {
            GameManager.Instance.GameClear();
        }
    }
    /// <summary>
    /// fade 오브젝트를 숨기는 함수
    /// </summary>
    private void HideFade()
    {
        // fade 오브젝트 비활성화
        fade.SetActive(false);
        // 게임 시작
        GameManager.Instance.GameStart();
    }
    /// <summary>
    /// MoveStartScene 함수 실행
    /// </summary>
    private void MoveStartScene()
    {
        // StartScene으로 이동
        SceneManager.LoadScene("StartScene");
    }
    #endregion

    #region 버튼 이벤트
    /// <summary>
    /// 게임 재시작 버튼 클릭시 이벤트 함수
    /// </summary>
    public void Restart()
    {
        // 클릭 사운드 실행
        SoundManager.Instance.ESound(AudioType.Click);
        // GameScene으로 이동
        SceneManager.LoadScene("GameScene");
    }
    /// <summary>
    /// 스테이지 선택 버튼 클릭시 이벤트 함수
    /// </summary>
    public void SelectMode()
    {
        // fade 오브젝트 활성화
        fade.SetActive(true);
        // FadeOut 애니메이션 실행
        fade.GetComponent<Animator>().SetTrigger("FadeOut");
        // 클릭 사운드 실행
        SoundManager.Instance.ESound(AudioType.Click);
        // 1.2초 후 MoveStartScene 함수 실행
        Invoke("MoveStartScene", 1.2f);
    }
    #endregion
}