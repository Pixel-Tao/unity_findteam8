using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int idx = 0;

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

    public void Setting(int number)

    {
        idx = number;
        frontImage.sprite = Resources.Load<Sprite>($"rtan{idx}"); 

    }

    public void OpenCard()
    {
        audioSource.PlayOneShot(clip); // ����� Ŭ���� �� �� ���
        Anim.SetBool("isOpen", true);
        front.SetActive(true);
        back.SetActive(false);



    }


}
