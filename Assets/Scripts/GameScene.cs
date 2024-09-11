using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScene : MonoBehaviour
{
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
