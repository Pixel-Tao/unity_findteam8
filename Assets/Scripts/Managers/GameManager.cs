using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public enum GameModeType
{
    None,
    Normal,
    Hard,
    Crazy,
    Hidden,
    Hidden2,
}

public class GameManager : MonoBehaviour
{

    #region 정적 변수 & 함수 선언부
    /// <summary>
    /// 싱글톤 정적 변수
    /// GameManager.Instance로 접근 가능
    /// </summary>
    public static GameManager Instance { get; private set; }
    /// <summary>
    /// 디버그 모드 사용 여부
    /// GameManager.DebugMode 로 사용
    /// </summary>
    public static bool DebugMode { get; private set; } = false;
    /// <summary>
    /// 게임 모드 사용 여부
    /// GameManager.GameMode 로 사용
    /// </summary>
    public static GameModeType GameMode { get; private set; } = GameModeType.None;
    /// <summary>
    /// 게임 모드 선택
    /// GameManager.SetGameMode(GameModeType) 로 사용
    /// </summary>
    /// <param name="mode"></param>
    public static void SetGameMode(GameModeType mode)
    {
        GameMode = mode;
        Debug.Log("SelectedMode: " + GameMode.ToString());
    }
    /// <summary>
    /// 디버그 모드 설정
    /// GameManager.SetDebugMode(bool) 로 사용
    /// </summary>
    /// <param name="mode"></param>
    public static void SetDebugMode(bool mode)
    {
        DebugMode = mode;
    }
    #endregion

    #region 변수 선언부

    [Header("제한시간 텍스트")]
    public Text timeText;

    [Header("게임 클리어, 게임 오버 팝업")]
    public GameObject gameClear;
    public GameObject gameOver;

    [Header("커서")]
    public GameObject cursor;

    [Header("카드 프리팹")]
    public GameObject cardPrefab;

    [Header("카드가 배치될 게임 오브젝트")]
    public GameObject cardBoard;

    [Header("선택한 카드")]
    public GameObject firstCard;
    public GameObject secondCard;

    /// <summary>
    /// 카드 깔리는 오브젝트
    /// </summary>
    public Board Board;
    /// <summary>
    /// 파티클 생성되는 오브젝트 풀 객체
    /// </summary>
    public SpawningPool spawningPool;

    /// <summary>
    /// 카드 인스턴스 리스트
    /// </summary>
    private List<Card> cardList;

    /// <summary>
    /// 흘러가는 시간
    /// </summary>
    private float time;
    [Header("제한시간")]
    public float endTime = 30.0f;

    /// <summary>
    /// 히든1 방해요소 오브젝트
    /// </summary>
    public GameObject troll;
    /// <summary>
    /// 히든2 흰색 공
    /// </summary>
    public GameObject whiteBall;
    /// <summary>
    /// 플레이 여부
    /// 사용하는 이유 Time.timeScale = 0.0f; 사용시 게임 애니메이션 효과 및 렌더링하는데도 영향이 있기때문에 기타 효과들의 후 처리를 위해 사용
    /// </summary>
    private bool isPlaying = false;
    #endregion

    #region 내부 함수 구현부 (private)
    /// <summary>
    /// GameManager가 처음 시작될때(렌더링 전) 실행되는 함수
    /// Start보다 우선으로 실행됨.
    /// </summary>
    private void Awake()
    {
        // Singleton을 사용할때 필요한 선언부
        // 객체화 하여 정적변수 Instance 에 할당해준다.
        Instance = this;
    }
    /// <summary>
    /// Awake 이후 1번 실행
    /// </summary>
    private void Start()
    {
        SetTimer(GameMode);
    }
    /// <summary>
    /// 매 프레임 마다 실행 함수
    /// </summary>
    private void Update()
    {
        // 게임중이라면 아무런 행위를 하지 않고 return
        if (!isPlaying) return;
        
        // 타이머를 감소한다.
        time -= Time.deltaTime;
        // 오브젝트의 null 체크
        if (timeText == null) return;
        // 제한시간 텍스트 변경, time과 0 중 큰 값을 선택하여 반환하고 그 값을 0.00형식으로 변환함
        timeText.text = MathF.Max(time, 0).ToString("F2");
        if (time <= 0)
        {
            // 게임 오버 함수 실행
            GameOver();
        }
    }
    /// <summary>
    ///  게임 시작전에 난이도에 맞게 시작 타이머와 색상을 선택하도록 한다.
    /// </summary>
    /// <param name="mode">게임 난이도</param>
    private void SetTimer(GameModeType mode)
    {
        switch (mode)
        {
            case GameModeType.None:
            case GameModeType.Normal:
            case GameModeType.Hard:
                endTime = 30.0f;
                break;
            case GameModeType.Crazy:
            case GameModeType.Hidden:
                timeText.color = Color.red;
                endTime = 20.0f;
                break;
            case GameModeType.Hidden2:
                timeText.color = Color.white;
                endTime = 60.0f;
                break;
        }
        time = endTime;
        timeText.text = time.ToString("F2");
    }
    /// <summary>
    /// 매칭 여부를 확인한다.
    /// </summary>
    /// <returns>firstCard 와 secondCard 의 일치 여부</returns>
    private bool IsCardMatched()
    {
        // firstCard, secondCard 의 자식 오브젝트 중에 SpriteRenderer 컴포넌트를 찾아서 sprite의 이름 즉 이미지 이름을 가져온다.
        string firstCardName = firstCard.transform.Find("Front").GetComponent<SpriteRenderer>().sprite.name;
        string secondCardName = secondCard.transform.Find("Front").GetComponent<SpriteRenderer>().sprite.name;

        // 혹시 모르니까 이름이 null 또는 공백일 경우 false 반환
        if (string.IsNullOrWhiteSpace(firstCardName) && string.IsNullOrWhiteSpace(secondCardName))
            return false;

        // 처음 고른 카드와 두번째 고른 카드의 이름을 비교하여 bool 값을 반환
        return firstCardName == secondCardName;
    }
    #endregion

    #region 공개 함수 구현부 (public)
    /// <summary>
    /// 게임을 시작한다.
    /// </summary>
    public void GameStart()
    {
        // 게임 시작 사운드
        SoundManager.Instance.BSound(AudioType.BGM);
        SoundManager.Instance.ESound(AudioType.Ball03);
        // 게임 시작을 설정한다.
        isPlaying = true;
        // 게임이 시작되었을때 커서를 보여준다.
        cursor.gameObject.SetActive(true);
        // 카드가 배치될 오브젝트
        Board board = cardBoard.GetComponent<Board>();
        // GameMode = GameModeType.Hidden2;
        // 난이도 별 카드 배치 방식
        switch (GameMode)
        {
            case GameModeType.Normal:
                Debug.Log("Normal Mode");
                cardList = board.NormalModeShuffle();
                break;
            case GameModeType.Hard:
                Debug.Log("Hard Mode");
                cardList = board.HardModeShuffle();
                break;
            case GameModeType.Crazy:
                Debug.Log("Crazy Mode");
                cardList = board.HellModeShuffle();
                break;
            case GameModeType.Hidden:
                Debug.Log("Hidden Mode");
                cardList = board.HiddenModeShuffle();
                break;
            case GameModeType.Hidden2:
                Debug.Log("Hidden Mode2");
                cardList = board.Hidden2ModeShuffle();
                break;
        }
    }
    /// <summary>
    /// 게임을 제한시간 내에 모든 카드를 찾아냈을 경우 실행
    /// </summary>
    public void GameClear()
    {
        // 게임 종료를 설정한다.
        isPlaying = false;
        // 성공 사운드
        SoundManager.Instance.ESound(AudioType.Win);

        // 자식 object 삭제
        for (int i = 0; i < cardBoard.transform.childCount; i++)
            Destroy(cardBoard.transform.GetChild(i).gameObject);

        // 불필요한 오브젝트는 삭제한다.
        Destroy(troll);
        Destroy(whiteBall);

        // 텍스트 변경 or 게임 클리어 팝업
        gameClear.SetActive(true);

        // 게임을 멈춤
        //Time.timeScale = 0.0f;

    }
    /// <summary>
    /// 제한시간 내에 게임을 성공하지 못했을 경우 실행
    /// </summary>
    public void GameOver()
    {
        // 게임 종료를 설정한다.
        isPlaying = false;
        // 성공 사운드
        SoundManager.Instance.ESound(AudioType.Defeat);

        // 자식 object 삭제
        for (int i = 0; i < cardBoard.transform.childCount; i++)
            Destroy(cardBoard.transform.GetChild(i).gameObject);

        // 불필요한 오브젝트는 삭제한다.
        Destroy(troll);
        Destroy(whiteBall);

        // 텍스트 변경 or 게임 오버 팝업
        gameOver.SetActive(true);


        // 게임을 멈춤
        //Time.timeScale = 0.0f;
    }
    /// <summary>
    /// 선택한 카드를 저장하고 매칭 여부를 확인한다.
    /// </summary>
    /// <param name="cardObj">선택한 카드 GameObject</param>
    public void SelectCard(GameObject cardObj, Vector3 hitPoint)
    {
        // 첫번째 카드가 들어오면 firstCard 가 null 일때 GameObject 를 넣어주고 아무런 행위를 하지않고 실행 종료
        if (firstCard == null)
        {
            // Hit 파티클 실행
            ShowParticle(ParticleEffectType.Hit, hitPoint);
            // 첫번째 카드 저장
            firstCard = cardObj;
            // 당구공 소리
            SoundManager.Instance.ESound(AudioType.Ball01);
            return;
        }
        // 두번째 카드 들어오면 secondCard 가 null 일때 GameObject 를 넣어주고 매칭 여부를 확인한다.
        else if (secondCard == null)
        {
            // 두번째 카드 저장
            secondCard = cardObj;
            // 두장의 카드 매치 여부 확인
            if (IsCardMatched())
            {
                // 카드 뒤집는 사운드
                SoundManager.Instance.ESound(AudioType.Ball01);
                // Nice 파티클 실행
                ShowParticle(ParticleEffectType.Nice, firstCard.transform.position);
                ShowParticle(ParticleEffectType.Nice, secondCard.transform.position);

                // 일치하는 카드 제거
                // 일치한 카드 객체를 리스트에서 삭제한다.
                cardList.Remove(firstCard.GetComponent<Card>());
                cardList.Remove(secondCard.GetComponent<Card>());
                // 카드 게임 오브젝트를 제거한다.
                firstCard.GetComponent<Card>().DestroyCard();
                secondCard.GetComponent<Card>().DestroyCard();
                // 남은 카드 수
                int cardsLeft = cardList.Count;

                // cardList에서 남은 카드가 없을 경우 게임 클리어
                if (cardsLeft == 0)
                {
                    GameClear();
                }
            }
            else
            {
                // 카드 뒤집기 실패 사운드
                SoundManager.Instance.ESound(AudioType.Dodge);
                // Miss 파티클 실행
                ShowParticle(ParticleEffectType.Miss, hitPoint);

                // 카드 원래대로 뒤집기
                firstCard.GetComponent<Card>().CloseCard();
                secondCard.GetComponent<Card>().CloseCard();
            }

            // 매칭여부 확인 후 firstCard, secondCard 초기화
            firstCard = null;
            secondCard = null;
        }
    }
    /// <summary>
    /// 이펙트 파티클을 실행하는 함수
    /// </summary>
    /// <param name="type">실행할 파티클 타입</param>
    /// <param name="pos">위치</param>
    public void ShowParticle(ParticleEffectType type, Vector3 pos)
    {
        spawningPool.Spawn(type.ToString(), pos);
    }
    /// <summary>
    /// 이펙트 파티클을 숨기는 함수
    /// </summary>
    /// <param name="go"></param>
    public void HideParticle(GameObject go)
    {
        // SpawningPool에 있는 Despawn 함수를 호출하여 파티클을 숨긴다.
        spawningPool.Despawn(go);
    }
    #endregion
}
