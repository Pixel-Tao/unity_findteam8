using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    
    //====================================
    Rigidbody2D rb = null;
    float Tick = 0;
    float PhysicsTick;
    //=======================feature_lms==

    void Start()
    {
        InitVelocity(1.5f, 1.5f);//아규먼츠로 Float값을 입력하면 그 시간만큼 물리를 적용합니다.
    }

    private void FixedUpdate()
    {
        ZeroVelocity();
    }//물리처리.

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

    //============================orgin/feature_lms=======
    void InitVelocity(float val, float F = 1)
    {
        rb = GetComponent<Rigidbody2D>();
        float force = F;
        float PosX = Random.Range(-1f, 1f);
        float PosY = Random.Range(-1f, 1f);
        rb.AddForce(new Vector2(PosX, PosY) * force, ForceMode2D.Impulse);
        PhysicsTick = val; // 각 볼 객체의 물리 적용 시간을 수정합니다.
    }
    void ZeroVelocity()
    {
        Tick += Time.deltaTime;
        if (Tick > PhysicsTick)
        {
            Rigidbody2D isRigid = GetComponent<Rigidbody2D>();
            isRigid.angularVelocity = 0;
            isRigid.velocity = Vector2.zero;
            isRigid.isKinematic = true;
        }
    }
}
