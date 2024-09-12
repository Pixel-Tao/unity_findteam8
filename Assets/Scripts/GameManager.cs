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
    /// <summary>
    /// Singleton pattern
    /// </summary>
    public static GameManager Instance { get; private set; }

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

    //Add by orgin/feature_lms 
    public Board Board;
    public SpawningPool spawningPool;

    // 카드 인스턴스 리스트
    private List<Card> cardList;

    // 흘러가는 시간
    private float time;
    [Header("제한시간")]
    public float endTime = 30.0f;

    public GameObject troll;


    private bool isPlaying = false;

    public static bool DebugMode { get; private set; } = false;
    public static GameModeType GameMode { get; private set; } = GameModeType.None;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SetTimer(GameMode);
    }

    private void Update()
    {
        if (!isPlaying) return;

        time -= Time.deltaTime;
        if (timeText == null) return;
        timeText.text = MathF.Max(time, 0).ToString("F2");
        if (time <= 0)
        {
            GameOver();
        }
    }

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
        SoundManager.inst.BSound(AudioType.BGM);
        SoundManager.inst.ESound(AudioType.Ball03);
        isPlaying = true;
        cursor.gameObject.SetActive(true);
        Board board = cardBoard.GetComponent<Board>();
        //GameMode = GameModeType.Hidden2;
        // 일반 배치
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
        Time.timeScale = 1.0f;
    }

    public static void SelectMode(GameModeType mode)
    {
        GameMode = mode;
        Debug.Log("SelectedMode: " + GameMode.ToString());
    }

    public static void SetDebugMode(bool mode)
    {
        DebugMode = mode;
    }



    /// <summary>
    /// 게임을 제한시간 내에 모든 카드를 찾아냈을 경우 실행
    /// </summary>
    public void GameClear()
    {
        isPlaying = false;
        SoundManager.inst.Stop();
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
        isPlaying = false;
        SoundManager.inst.Stop();
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
    public void SelectCard(GameObject cardObj, Vector3 hitPoint)
    {
        // 첫번째 카드가 들어오면 firstCard 가 null 일때 GameObject 를 넣어주고 아무런 행위를 하지않고 실행 종료
        if (firstCard == null)
        {
            ShowParticle(ParticleEffectType.Hit, hitPoint);

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
                ShowParticle(ParticleEffectType.Nice, firstCard.transform.position);
                ShowParticle(ParticleEffectType.Nice, secondCard.transform.position);

                // 일치하는 카드 제거
                cardList.Remove(firstCard.GetComponent<Card>());
                cardList.Remove(secondCard.GetComponent<Card>());
                firstCard.GetComponent<Card>().DestroyCard();
                secondCard.GetComponent<Card>().DestroyCard();

                int cardsLeft = cardList.Count;

                // cardList에서 남은 카드가 없을 경우 게임 클리어
                if (cardsLeft == 0)
                {
                    Invoke("GameClear", 1f);
                }
            }
            else
            {
                // 카드 뒤집기 실패 사운드
                SoundManager.inst.ESound(AudioType.Dodge);
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

    public void ShowParticle(ParticleEffectType type, Vector3 pos)
    {
        spawningPool.Spawn(type.ToString(), pos);
    }

    public void HideParticle(GameObject go)
    {
        spawningPool.Despawn(go);
    }
}
