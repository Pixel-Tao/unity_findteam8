using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class StartScene : MonoBehaviour
{
    public GameObject fade;
    public GameObject cursor;
    public Text teamName;

    public GameObject NormalBtn;
    public GameObject HardBtn;
    public GameObject CrazyBtn;
    public GameObject HiddenBtn;
    public GameObject Hidden2Btn;

    public GameObject MainBall;
    public GameObject MainBallHard;
    public GameObject MainBallCrazy;

    public Animator ballAnimator;

    byte hiddenCount = 0;

    void Start()
    {
        Time.timeScale = 1.0f;
        teamName.fontStyle = GameManager.DebugMode ? FontStyle.Italic : FontStyle.Bold;
        ClickNormal();
        FadeIn();
        cursor.SetActive(true);
    }

    void FadeIn()
    {
        fade.SetActive(true);
        fade.GetComponent<Animator>().SetTrigger("FadeIn");
        Invoke("HideFade", 1.0f);
    }
    void HideFade()
    {
        fade.SetActive(false);
    }

    void FadeOut(string method)
    {
        fade.SetActive(true);
        fade.GetComponent<Animator>().SetTrigger("FadeOut");
        Invoke(method, 1.2f);
    }

    void MoveGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    #region ButtonClickEvent

    public void retry()
    {
        SoundManager.inst.ESound(AudioType.Ball01);
        ballAnimator.SetTrigger("MoveUp");
        FadeOut("MoveGameScene");
    }
    public void ClickNormal()
    {
        Debug.Log("Normal");
        MainBall.SetActive(true);
        MainBallHard.SetActive(false);
        MainBallCrazy.SetActive(false);

        GameManager.SelectMode(GameModeType.Normal);

        ballAnimator = MainBall.GetComponent<Animator>();
    }

    public void ClickHard()
    {
        Debug.Log("Hard");
        MainBall.SetActive(false);
        MainBallHard.SetActive(true);
        MainBallCrazy.SetActive(false);

        GameManager.SelectMode(GameModeType.Hard);

        ballAnimator = MainBallHard.GetComponent<Animator>();
    }

    public void ClickCrazy()
    {
        Debug.Log("Crazy");
        MainBall.SetActive(false);
        MainBallHard.SetActive(false);
        MainBallCrazy.SetActive(true);

        GameManager.SelectMode(GameModeType.Crazy);

        ballAnimator = MainBallCrazy.GetComponent<Animator>();
    }

    public void ClickHidden()
    {
        HiddenStage();
        HiddenBtn.GetComponent<Animator>().SetTrigger("Shake");
    }

    public void ClickHidden2()
    {
        // TODO : Hidden Mode 이미지 변경?
        HiddenStage();
        Hidden2Btn.GetComponent<Animator>().SetTrigger("Shake");
    }

    void HiddenStage()
    {
        hiddenCount++;
        if (hiddenCount == 1)
        {
            Debug.Log("Hidden Stage 1");
            GameManager.SelectMode(GameModeType.Hidden);
        }
        else if (hiddenCount == 2)
        {
            Debug.Log("Hidden Stage 2");
            GameManager.SelectMode(GameModeType.Hidden2);
        }
        else
        {
            GameManager.SelectMode(GameModeType.Normal);
            hiddenCount = 0;
        }
    }

    public void DebugMode()
    {
        GameManager.SetDebugMode(!GameManager.DebugMode);
        teamName.fontStyle = GameManager.DebugMode ? FontStyle.Italic : FontStyle.Bold;

    }
    #endregion
}
