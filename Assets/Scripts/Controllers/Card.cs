using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    #region 변수 선언부
    // 불필요한 변수이긴 하지만 변수 선언시 어떤 용도 변수인지 이름을 명시적으로 표시해줘야 합니다.
    //public int idx = 0;
    //public int idx2 = 0;

    /// <summary>
    /// 카드 앞면 오브젝트
    /// </summary>
    public GameObject frontCard;
    /// <summary>
    /// 카드 뒷면 오브젝트
    /// </summary>
    public GameObject backCard;
    /// <summary>
    /// 애니메이터 컴포넌트
    /// </summary>
    public Animator Anim; // 애니메이션 변수

    /// <summary>
    /// 카드 앞면 사진 Sprite
    /// </summary>
    public SpriteRenderer frontImage;
    /// <summary>
    /// 카드 뒷면 사진 Sprite
    /// </summary>
    public SpriteRenderer backImage;

    /// <summary>
    /// 디버그 모드일때 보려일 index 텍스트
    /// </summary>
    public Text cheatText;

    /// <summary>
    /// 물리 적용을 위한 Rigidbody2D
    /// </summary>
    private Rigidbody2D rb = null;
    /// <summary>
    /// 물리적용 유지시간
    /// 변수명은 클래스 자체가 1가지 기능만 있는것이 아니면 명확한 용도의 이름으로 해주는게 좋습니다.
    /// </summary>
    private float physicsTick = 0;
    /// <summary>
    /// 물리적용 해제시간
    /// </summary>
    private float physicsStopTick;

    /// <summary>
    /// 외부에서 현재카드가 열려있는지 닫혀있는지 알 수 있도록 해주는 bool 변수
    /// 변경은 클래스 내부에서만 가능
    /// </summary>
    public bool isOpen { get; private set; }
    #endregion

    #region 내부 함수 선언부
    private void Start()
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

    /// <summary>
    /// 물리 연산을 위한 함수
    /// https://docs.unity3d.com/kr/2023.2/Manual/ExecutionOrder.html#InBetweenFrames
    /// </summary>
    private void FixedUpdate()
    {
        // 난이도 별 물리 적용 방식
        // 다른 if이라도 같은 로직을 탄다면 switch문을 사용하거나, 실행될 함수에 파라미터를 넘겨서 구분하는것 사용하는것도 방법입니다.
        ZeroVelocity(GameManager.GameMode);
        //switch (GameManager.GameMode)
        //{
        //    case GameModeType.Normal:
        //    case GameModeType.Hard:
        //    case GameModeType.Crazy:
        //        ZeroVelocity();
        //        break;
        //    case GameModeType.Hidden:
        //        break;
        //    case GameModeType.Hidden2:
        //        break;
        //}
        //if (GameManager.GameMode == GameModeType.Normal)
        //    ZeroVelocity();
        //if (GameManager.GameMode == GameModeType.Hard)
        //    ZeroVelocity();
        //if (GameManager.GameMode == GameModeType.Crazy)
        //    ZeroVelocity();
    }
    /// <summary>
    /// 일정시간 후 카드를 닫히게 하기 위한 함수
    /// </summary>
    private void CloseCardInvoke()
    {
        isOpen = false;
        Anim.SetBool("isOpen", isOpen);
        frontCard.SetActive(isOpen);
        backCard.SetActive(!isOpen);
    }
    /// <summary>
    /// 가드 게임오브젝트를 삭제하기 위한 함수
    /// </summary>
    private void DestroyCardInvoke()
    {
        Destroy(gameObject);
    }
    /// <summary>
    /// 물리적으로 이동하는 것을 일정 시간 후 멈추게 하는 함수
    /// </summary>
    private void ZeroVelocity(GameModeType gameMode)
    {
        // 난이도가 노멀, 하드, 헬이 아닌 경우 물리적용을 계속 유지
        if (gameMode > GameModeType.Crazy) return;

        physicsTick += Time.deltaTime;
        if (physicsTick > physicsStopTick)
        {
            Rigidbody2D isRigid = GetComponent<Rigidbody2D>();
            // 회전속도를 0으로
            isRigid.angularVelocity = 0;
            // 물리적인 이동속도 0으로
            isRigid.velocity = Vector2.zero;
            // 물리적용을 멈춤
            isRigid.simulated = false;
        }
    }
    #endregion

    #region 외부 함수 선언부
    /// <summary>
    /// 카드의 앞면과 뒷면을 설정하는 함수
    /// </summary>
    /// <param name="frontNumber">앞면 사진 index 번호</param>
    /// <param name="backNumber">뒷면 사진 index 번호</param>
    public void Setting(int frontNumber, int backNumber)
    {
        frontImage.sprite = Resources.Load<Sprite>($"teamPhoto{frontNumber}");
        backImage.sprite = Resources.Load<Sprite>($"ballBack{backNumber}");
        // 디버그 모드일때만 텍스트 보이기
        cheatText.text = GameManager.DebugMode ? $"{frontNumber}" : "";
        // 게임 모드가 Hidden2가 아닐때 클릭 대상으로 설정
        cheatText.raycastTarget = GameManager.GameMode != GameModeType.Hidden2;
    }
    /// <summary>
    /// 카드를 열어주는 함수
    /// </summary>
    public void OpenCard()
    {
        // 열렸다는 것을 설정
        isOpen = true;
        // 애니메이션을 실행
        Anim.SetBool("isOpen", isOpen);
        // 앞면을 보여주고
        frontCard.SetActive(isOpen);
        // 뒷면은 숨기기
        backCard.SetActive(!isOpen);
        
        if (GameManager.GameMode == GameModeType.Hidden2)
        {
            // 히든2모드일 때는 WhiteBall이 물리적으로 닿은 카드만 열어주기 때문에 이쪽으로
            GameManager.Instance.SelectCard(gameObject, transform.position);
        }
        else
        {
            // 그 외 모드일 때는 마우스 위치에서 클릭했을때 마우스 포인터 위치를 알아내기 위해 이쪽으로
            Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
            Input.mousePosition.y, 0));
            GameManager.Instance.SelectCard(gameObject, point);
        }

    }
    /// <summary>
    /// 카드를 0.5초뒤에 닫아주는 함수
    /// </summary>
    public void CloseCard()
    {
        Invoke("CloseCardInvoke", 0.5f);
    }
    /// <summary>
    /// 카드를 0.5초뒤에 삭제하는 함수
    /// </summary>
    public void DestroyCard()
    {
        Invoke("DestroyCardInvoke", 0.5f);
    }

    /// <summary>
    /// 아규먼츠로 Float값을 입력하면 그 시간만큼 물리를 적용합니다.
    /// </summary>
    /// <param name="stopTick">해당 시간만큼 물리적용</param>
    /// <param name="smooth">물리력의 저항을 줄건지</param>
    public void InitVelocity(float stopTick, bool smooth = false)
    {
        rb = GetComponent<Rigidbody2D>();
        rb.simulated = true;

        // https://docs.unity3d.com/kr/2023.2/Manual/class-Rigidbody.html
        if (smooth)
            rb.drag = 0.8f;
        // 2 Range만큼의 범위에서 랜덤으로 방향을 정하고
        float PosX = Random.Range(-1f, 1f);
        float PosY = Random.Range(-1f, 1f);
        // 물리적인 힘을 가함.
        rb.AddForce(new Vector2(PosX, PosY), ForceMode2D.Impulse);
        // 각 볼 객체의 물리 적용 시간을 수정합니다.
        physicsStopTick = stopTick; 
    }
    #endregion
}
