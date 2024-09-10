using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;

public class ButtonAnimation : MonoBehaviour
{
    public Button playButton;  
    public Animator animator;  

    void Start()
    {
        // 버튼에 클릭 이벤트 리스너 추가
        playButton.onClick.AddListener(OnPlayButtonClick);
    }

    void OnPlayButtonClick()
    {
        // 버튼이 클릭될 때 트리거를 설정하여 애니메이션 재생
        animator.SetTrigger("Click");
    }
}
