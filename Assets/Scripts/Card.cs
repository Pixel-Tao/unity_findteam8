using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public int idx = 0;
    public int idx2 = 0;

    public GameObject front; // 카드 앞면
    public GameObject back;  // 카드 뒷면

    public Animator Anim; // 애니메이션 변수

    public SpriteRenderer frontImage; // 팀원 사진
    public SpriteRenderer backImage;  // 뒷면

    public Text cheatText;

    //====================================
    Rigidbody2D rb = null;
    float Tick = 0;
    float PhysicsTick;
    //=======================feature_lms==

    public bool isOpen { get; private set; }

    void Start()
    {
        Rigidbody2D isRigid = GetComponent<Rigidbody2D>();
        if (GameManager.GameMode == GameModeType.Normal)
        {
            // 물리처리를 하지 않음
            isRigid.simulated = false;
        }
        else if (GameManager.GameMode == GameModeType.Hard)
        {
            // 물리처리를 함.
            isRigid.simulated = true;
        }
        else if (GameManager.GameMode == GameModeType.Crazy)
        {
            // 물리처리를 함.
            isRigid.simulated = true;
        }
        else if (GameManager.GameMode == GameModeType.Hidden)
        {
            frontImage.transform.localScale = new Vector3(0.7f, 0.7f, 1);
        }
        else if (GameManager.GameMode == GameModeType.Hidden)
        {
            isRigid.simulated = true;
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.GameMode == GameModeType.Normal)
            ZeroVelocity();
        if (GameManager.GameMode == GameModeType.Hard)
            ZeroVelocity();
        if (GameManager.GameMode == GameModeType.Crazy)
            ZeroVelocity();
    }//물리처리.

    public void Setting(int frontNumber, int backNumber)
    {
        idx = frontNumber;
        idx2 = backNumber;
        frontImage.sprite = Resources.Load<Sprite>($"teamPhoto{frontNumber}");
        backImage.sprite = Resources.Load<Sprite>($"ballBack{backNumber}");
        cheatText.text = GameManager.DebugMode ? $"{frontNumber}" : "";
        cheatText.raycastTarget = GameManager.GameMode != GameModeType.Hidden2;
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
        isOpen = true;
        Anim.SetBool("isOpen", isOpen);
        front.SetActive(isOpen);
        back.SetActive(!isOpen);
        
        if(GameManager.GameMode == GameModeType.Hidden2)
        {
            GameManager.Instance.SelectCard(gameObject, transform.position);
        }
        else
        {
            Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
            Input.mousePosition.y, 0));
            GameManager.Instance.SelectCard(gameObject, point);
        }
        
    }

    public void CloseCard()
    {
        Invoke("CloseCardInvoke", 0.5f);
    }

    void CloseCardInvoke()
    {
        isOpen = false;
        Anim.SetBool("isOpen", isOpen);
        front.SetActive(isOpen);
        back.SetActive(!isOpen);
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
    //아규먼츠로 Float값을 입력하면 그 시간만큼 물리를 적용합니다.
    public void InitVelocity(float val, bool smooth = false)
    {
        rb = GetComponent<Rigidbody2D>();
        rb.simulated = true;

        if (smooth)
            rb.drag = 0.8f;

        float PosX = Random.Range(-1f, 1f);
        float PosY = Random.Range(-1f, 1f);
        rb.AddForce(new Vector2(PosX, PosY), ForceMode2D.Impulse);
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
            isRigid.simulated = false;
        }
    }
}
