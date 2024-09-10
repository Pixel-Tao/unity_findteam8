using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class StartScene : MonoBehaviour
{
    public void retry()
    {
        SoundManager.inst.ESound(AudioType.Click);
        SceneManager.LoadScene("GameScene");
    }

    public void gameQuit()
    {
        SoundManager.inst.ESound(AudioType.Click);
        Application.Quit();
    }
}
