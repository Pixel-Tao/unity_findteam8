using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScene : MonoBehaviour
{
    /// <summary>
    /// FadeIn, FadeOut 애니메이션을 가지고 있는 오브젝트
    /// </summary>
    public GameObject fade;

    /// <summary>
    /// 시작 시 FadeOut 애니메이션을 실행합니다.
    /// </summary>
    private void Start()
    {
        // 3초 후 FadeOut 애니메이션 실행
        Invoke("FadeOut", 3f);
    }
    /// <summary>
    /// FadeOut 애니메이션을 실행합니다.
    /// </summary>
    private void FadeOut()
    {
        // fade 오브젝트 활성화
        fade.SetActive(true);
        // FadeOut 애니메이션 실행
        fade.GetComponent<Animator>().SetTrigger("FadeOut");
        // 1.5초 후 MoveStartScene 함수 실행
        Invoke("MoveStartScene", 1.5f);
    }
    /// <summary>
    /// StartScene으로 이동합니다.
    /// </summary>
    private void MoveStartScene()
    {
        // StartScene으로 이동
        SceneManager.LoadScene("StartScene");
    }
}
