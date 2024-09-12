using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScene : MonoBehaviour
{
    public GameObject fade;

    private void Start()
    {
        Invoke("FadeOut", 3f);
    }

    void FadeOut()
    {
        fade.SetActive(true);
        fade.GetComponent<Animator>().SetTrigger("FadeOut");
        Invoke("MoveStartScene", 1.5f);
    }

    void MoveStartScene()
    {
        SceneManager.LoadScene("StartScene");
    }
}
