using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class StartScene : MonoBehaviour
{

    public void Start()
    {
        ClickNormal();
    }
    public void retry()
    {
        Time.timeScale = 1.0f;
        SoundManager.inst.ESound(AudioType.Click);
        Invoke("LoadGameScene", 0.3f);
        ballAnimator.SetTrigger("MoveUp");
        
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }


    public void gameQuit()
    {
        SoundManager.inst.ESound(AudioType.Click);
        Application.Quit();
    }

    public GameObject NormalBtn;
    public GameObject HardBtn;
    public GameObject CrazyBtn;
    public GameObject HiddenBtn;

    public GameObject MainBall;
    public GameObject MainBallHard;
    public GameObject MainBallCrazy;

    public Animator ballAnimator;


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
        Debug.Log("Hidden");
       

        GameManager.SelectMode(GameModeType.Hidden);

    }

}
