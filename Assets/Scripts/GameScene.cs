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
        SoundManager.inst.ESound(AudioType.Click);
        SceneManager.LoadScene("GameScene");
    }

    public void SelectMode()
    {
        SoundManager.inst.ESound(AudioType.Click);
        SceneManager.LoadScene("StartScene");
    }


}
