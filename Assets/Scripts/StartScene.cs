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
        SoundManager.inst.ESound(AudioType.Click);
        Invoke("LoadGameScene", 0.3f);
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

    public GameObject MainBall;
    public GameObject MainBallHard;
    public GameObject MainBallCrazy;

  

    public void ClickNormal()
    {
        Debug.Log("Normal");
        MainBall.SetActive(true);
        MainBallHard.SetActive(false);
        MainBallCrazy.SetActive(false);

        GameManager.SelectMode(GameModeType.Normal);
       

        
    }

    public void ClickHard()
    {
        Debug.Log("Hard");
        MainBall.SetActive(false);
        MainBallHard.SetActive(true);
        MainBallCrazy.SetActive(false);

        GameManager.SelectMode(GameModeType.Hard);
    }

    public void ClickCrazy()
    {
        Debug.Log("Crazy");
        MainBall.SetActive(false);
        MainBallHard.SetActive(false);
        MainBallCrazy.SetActive(true);

        GameManager.SelectMode(GameModeType.Crazy);
    }

}
