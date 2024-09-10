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

    public AudioClip clip;

    public SpriteRenderer frontImage; // 팀원 사진
    public SpriteRenderer backImage;  // 뒷면


    void Start()
    {
    }


    void Update()
    {

    }

    public void Setting(int frontNumber, int backNumber)

    {
        idx = frontNumber;
        idx2 = backNumber;
        frontImage.sprite = Resources.Load<Sprite>($"teamPhoto{frontNumber}");
        backImage.sprite = Resources.Load<Sprite>($"ballBack{backNumber}");

    }

    public void RotateFrontFace(float angle)
    {
        if (backImage != null)
        {
            // 앞면만 Z축을 기준으로 회전
            backImage.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    public void OpenCard()
    {
        SoundManager.inst.ESound(AudioType.Ball01); // 오디오 클립이 한 번 재생
        Anim.SetBool("isOpen", true);
        front.SetActive(true);
        back.SetActive(false);

        GameManager.Instance.SelectCard(gameObject);
    }

    public void CloseCard()
    {
        Invoke("CloseCardInvoke", 0.5f);
    }

    void CloseCardInvoke()
    {
        Anim.SetBool("isOpen", false);
        front.SetActive(false);
        back.SetActive(true);
    }

    public void DestroyCard()
    {
        Invoke("DestroyCardInvoke", 0.5f);
    }

    void DestroyCardInvoke()
    {
        Destroy(gameObject);
    }
}
