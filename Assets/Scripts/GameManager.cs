using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    Hidden
}

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Singleton pattern
    /// </summary>
    public static GameManager Instance { get; private set; }

    [Header("제한시간 텍스트")]
    public Text timeText;

    [Header("게임 클리어, 게임 오버 팝업")]
    public GameObject gameClear;
    public GameObject gameOver;

    [Header("카드 프리팹")]
    public GameObject cardPrefab;

    [Header("카드가 배치될 게임 오브젝트")]
    public GameObject cardBoard;

    [Header("선택한 카드")]
    public GameObject firstCard;
    public GameObject secondCard;

    //Add by orgin/feature_lms 
    public Board Board;

    // 카드 인스턴스 리스트
    private List<Card> cardList;

    // 흘러가는 시간
    private float time;
    [Header("제한시간")]
    public float endTime = 30.0f;

    public GameObject troll;
    

    public GameModeType GameMode { get; private set; } = GameModeType.None;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameStart();
    }

    private void Update()
    {
        time += Time.deltaTime;
        if (timeText == null) return;
        timeText.text = time.ToString("F2");

        if (time >= endTime)
        {
            GameOver();
        }
    }

    /// <summary>
    /// 매칭 여부를 확인한다.
    /// </summary>
    /// <returns>firstCard 와 secondCard 의 일치 여부</returns>
    private bool IsCardMatched()
    {
        string firstCardName = firstCard.transform.Find("Front").GetComponent<SpriteRenderer>().sprite.name;
        string secondCardName = secondCard.transform.Find("Front").GetComponent<SpriteRenderer>().sprite.name;

        if (string.IsNullOrWhiteSpace(firstCardName) && string.IsNullOrWhiteSpace(secondCardName))
            return false;

        return firstCardName == secondCardName;
    }

    /// <summary>
    /// 게임을 시작한다.
    /// </summary>
    public void GameStart()
    {
        // 게임 시작 사운드
        //SoundManager.inst.BSound(AudioType.BGM);
        SoundManager.inst.ESound(AudioType.Ball03);
        time = 0.0f;
        Board board = cardBoard.GetComponent<Board>();
        GameMode = GameModeType.Hidden;
        // 일반 배치
        switch (GameMode)
        {
            case GameModeType.Normal:
                cardList = board.NormalModeShuffle();
                break;
            case GameModeType.Hard:
                cardList = board.HardModeShuffle();
                break;
            case GameModeType.Crazy:
                cardList = board.CrazyModeShuffle();
                endTime = 20.0f;
                break;
            case GameModeType.Hidden:
                cardList = board.HiddenModeShuffle();
                endTime = 20.0f;
                break;
        }
        // BallSpawner();
        Time.timeScale = 1.0f;
    }

    /// <summary>
    /// 게임을 제한시간 내에 모든 카드를 찾아냈을 경우 실행
    /// </summary>
    public void GameClear()
    {
        // 성공 사운드
        SoundManager.inst.ESound(AudioType.Win);

        // 텍스트 변경 or 게임 클리어 팝업
        gameClear.SetActive(true);

        // 게임을 멈춤
        Time.timeScale = 0.0f;

        Destroy(troll);
    }

    /// <summary>
    /// 제한시간 내에 게임을 성공하지 못했을 경우 실행
    /// </summary>
    public void GameOver()
    {
        // 성공 사운드
        SoundManager.inst.ESound(AudioType.Defeat);

        // 자식 object 삭제
        for (int i = 0; i < cardBoard.transform.childCount; i++)
            Destroy(cardBoard.transform.GetChild(i).gameObject);

        Destroy(troll);

        // 텍스트 변경 or 게임 오버 팝업
        gameOver.SetActive(true);


        // 게임을 멈춤
        Time.timeScale = 0.0f;
    }

    /// <summary>
    /// 선택한 카드를 저장하고 매칭 여부를 확인한다.
    /// </summary>
    /// <param name="cardObj">선택한 카드 GameObject</param>
    public void SelectCard(GameObject cardObj)
    {
        // 첫번째 카드가 들어오면 firstCard 가 null 일때 GameObject 를 넣어주고 아무런 행위를 하지않고 실행 종료
        if (firstCard == null)
        {
            firstCard = cardObj;
            SoundManager.inst.ESound(AudioType.Ball01); // 오디오 클립이 한 번 재생
            return;
        }
        // 두번째 카드 들어오면 secondCard 가 null 일때 GameObject 를 넣어주고 매칭 여부를 확인한다.
        else if (secondCard == null)
        {
            secondCard = cardObj;

            if (IsCardMatched())
            {
                // 카드 뒤집는 사운드
                SoundManager.inst.ESound(AudioType.Ball01);

                // 일치하는 카드 제거
                cardList.Remove(firstCard.GetComponent<Card>());
                cardList.Remove(secondCard.GetComponent<Card>());
                firstCard.GetComponent<Card>().DestroyCard();
                secondCard.GetComponent<Card>().DestroyCard();

                int cardsLeft = cardList.Count;

                // cardList에서 남은 카드가 없을 경우 게임 클리어
                if (cardsLeft == 0)
                {
                    Invoke("GameClear", 0.5f);
                }
            }
            else
            {
                // 카드 뒤집기 실패 사운드
                SoundManager.inst.ESound(AudioType.Dodge);

                // 카드 원래대로 뒤집기
                firstCard.GetComponent<Card>().CloseCard();
                secondCard.GetComponent<Card>().CloseCard();
            }

            // 매칭여부 확인 후 firstCard, secondCard 초기화
            firstCard = null;
            secondCard = null;
        }
    }
}
