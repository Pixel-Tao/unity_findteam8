using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class StartScene : MonoBehaviour
{
    public GameObject fade;

    public void retry()
    {
        SoundManager.inst.ESound(AudioType.Click);
        fade.SetActive(true);
        fade.GetComponent<Animator>().SetTrigger("FadeOut");
        Invoke("MoveGameScene", 1.2f);
    }

    void MoveGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void gameQuit()
    {
        SoundManager.inst.ESound(AudioType.Click);
        Application.Quit();
    }
}
