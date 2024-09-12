using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScene : MonoBehaviour
{
    public GameObject fade;

    void Start()
    {
        GameStart();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameManager.Instance.GameOver();
        }
        else if(Input.GetKeyDown(KeyCode.W))
        {
            GameManager.Instance.GameClear();
        }
    }

    public void GameStart()
    {
        fade.SetActive(true);
        fade.GetComponent<Animator>().SetTrigger("FadeIn");
        Invoke("HideFade", 1.0f);
    }

    void HideFade()
    {
        fade.SetActive(false);
        GameManager.Instance.GameStart();
    }

    public void Restart()
    {
        Time.timeScale = 1.0f;
        SoundManager.inst.ESound(AudioType.Click);
        SceneManager.LoadScene("GameScene");
    }

    public void SelectMode()
    {
        Time.timeScale = 1.0f;
        fade.SetActive(true);
        fade.GetComponent<Animator>().SetTrigger("FadeOut");
        SoundManager.inst.ESound(AudioType.Click);
        Invoke("MoveStartScene", 1.2f);
    }

    void MoveStartScene()
    {
        SceneManager.LoadScene("StartScene");
    }


}
