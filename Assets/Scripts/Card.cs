using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int idx = 0;
    public int idx2 = 0;

    public GameObject front; // ī�� �ո�
    public GameObject back;  // ī�� �޸�

    public Animator Anim; // �ִϸ��̼� ����

    AudioSource audioSource; // ����� �ҽ� ���� 
    public AudioClip clip;

    public SpriteRenderer frontImage; // ���� ����
    public SpriteRenderer backImage;  // �޸�


    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // ������ҽ� ������Ʈ ����
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

        // �̹����� ����� �ε���� ������ �α׸� ����
        if (frontImage.sprite == null)
            Debug.LogError($"Failed to load front sprite for rtan{frontNumber}");

        if (backImage.sprite == null)
            Debug.LogError($"Failed to load back sprite for backColor{backNumber}");


    }

    public void OpenCard()
    {
        audioSource.PlayOneShot(clip); // ����� Ŭ���� �� �� ���
        Anim.SetBool("isOpen", true);
        front.SetActive(true);
        back.SetActive(false);



    }


}
