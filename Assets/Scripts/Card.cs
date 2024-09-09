using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int idx = 0;
    public int idx2 = 0;

    public GameObject front; // 카드 앞면
    public GameObject back;  // 카드 뒷면

    public Animator Anim; // 애니메이션 변수

    AudioSource audioSource; // 오디오 소스 변수 
    public AudioClip clip;

    public SpriteRenderer frontImage; // 팀원 사진
    public SpriteRenderer backImage;  // 뒷면


    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // 오디오소스 컴포넌트 실행
    }


    void Update()
    {

    }

    public void Setting(int frontNumber, int backNumber)

    {
        idx = frontNumber;
        idx2 = backNumber;
        frontImage.sprite = Resources.Load<Sprite>($"rtan{frontNumber}");
        backImage.sprite = Resources.Load<Sprite>($"backColor{backNumber}");

        Debug.Log($"Setting frontNumber: {frontNumber}, backNumber: {backNumber}");

        frontImage.sprite = Resources.Load<Sprite>($"rtan{frontNumber}");
        backImage.sprite = Resources.Load<Sprite>($"backColor{backNumber}");

        // 이미지가 제대로 로드되지 않으면 로그를 남김
        if (frontImage.sprite == null)
            Debug.LogError($"Failed to load front sprite for rtan{frontNumber}");

        if (backImage.sprite == null)
            Debug.LogError($"Failed to load back sprite for backColor{backNumber}");


    }

    public void OpenCard()
    {
        audioSource.PlayOneShot(clip); // 오디오 클립이 한 번 재생
        Anim.SetBool("isOpen", true);
        front.SetActive(true);
        back.SetActive(false);



    }


}
