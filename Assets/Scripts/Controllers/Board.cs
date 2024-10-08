using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    public GameObject card;
    public GameObject troll;
    public GameObject whiteBall;
    public Text timeTxt;
    public Animator Anim;
    public GameObject backCard;

    void Start()
    {
    }

    /// <summary>
    /// 카드를 섞는다.
    /// </summary>
    /// <returns>섞은 카드 번호 배열</returns>
    private int[] ShuffleCardsFront()
    {
        // 카드 섞기
        int[] cards = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7 };
        return cards.OrderBy(x => Random.Range(0f, 7f)).ToArray(); 
    }
    private int[] ShuffleCardsBack()
    {
        // 카드 섞기
        int[] cards = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
        return cards.OrderBy(x => Random.Range(0f, 15f)).ToArray();
    }

    /// <summary>
    /// 가만히 있는 공
    /// </summary>
    /// <returns></returns>
    public List<Card> NormalModeShuffle()
    {
        List<Card> cards = new List<Card>();

        int[] frontArr = ShuffleCardsFront();
        int[] backArr = ShuffleCardsBack();

        for (int i = 0; i < 16; i++)
        {
            GameObject go = Instantiate(card, this.transform);

            float x = (i % 4) * 1.1f - 1.7f;
            float y = (i / 4) * 1.3f - 2.0f;

            go.transform.position = new Vector2(x, y);

            int frontImage = frontArr[i];
            int backImage = backArr[i];

            float randomAngle = Random.Range(0f, 360f); // 0도에서 360도 사이의 랜덤 각도
            go.transform.GetChild(1).rotation = Quaternion.Euler(0, 0, randomAngle);

            Card item = go.GetComponent<Card>();
            item.Setting(frontImage, backImage);
            cards.Add(item);
        }
        
        return cards;
    }

    /// <summary>
    /// 움직이는 공
    /// </summary>
    /// <returns></returns>
    public List<Card> HardModeShuffle()
    {
        List<Card> cardList = new List<Card>();
        int[] cardNumbers = ShuffleCardsFront();
        int[] cardBacks = ShuffleCardsBack();
        int Scale = 0;

        while (true)
        {
            if (Scale == 16)//level :: TODO)
                break;

            float x = Random.Range(-1.7f, 1.7f);
            float y = Random.Range(-3.7f, 3.7f);

            GameObject newCard = Instantiate(card);
            Card item = newCard.GetComponent<Card>();

            newCard.transform.parent = this.transform;
            newCard.transform.position = new Vector3(x, y, 0);

            item.InitVelocity(3f, true);
            item.Setting(cardNumbers[Scale % 16], cardBacks[Scale % 16]);
            cardList.Add(newCard.GetComponent<Card>());

            Scale++;
        }

        return cardList;
    }

    /// <summary>
    /// 모두 8번 공
    /// </summary>
    /// <returns></returns>
    public List<Card> HellModeShuffle()
    {
        List<Card> cardList = new List<Card>();
        int[] cardNumbers = ShuffleCardsFront();
        int[] cardBacks = ShuffleCardsBack();
        int Scale = 0;

        while (true)
        {
            if (Scale == 16)//level :: TODO)
                break;

            float x = Random.Range(-1.7f, 1.7f);
            float y = Random.Range(-3.7f, 2.3f);

            GameObject newCard = Instantiate(card);
            Card item = newCard.GetComponent<Card>();

            newCard.transform.parent = this.transform;
            newCard.transform.position = new Vector3(x, y, 0);

            item.InitVelocity(1.5f);
            item.Setting(cardNumbers[Scale % 16], cardBacks[Scale % 16]);
            cardList.Add(newCard.GetComponent<Card>());
            Scale++;
        }

        int[] ShuffleCardsBack()
        {
            // 카드 섞기
            int[] cards = {8,8,8,8,8,8,8,8,8,8,8,8,8,8,8,8};
            return cards.OrderBy(x => Random.Range(0f, 15f)).ToArray();
        }
        return cardList;
    }

    /// <summary>
    /// 헬 모드 + Troll 모드
    /// </summary>
    /// <returns></returns>
    public List<Card> HiddenModeShuffle()
    {
        List<Card> cardList = HellModeShuffle();

        timeTxt.color = Color.red;
        troll.SetActive(true);
        backCard.transform.localScale = new Vector3(0.7f, 0.7f, 1f);

        return cardList;
    }

    /// <summary>
    /// 노멀 모드 + whiteBall 추가
    /// </summary>
    /// <returns></returns>
    public List<Card> Hidden2ModeShuffle()
    {
        whiteBall.SetActive(true);
        return NormalModeShuffle();
    }

}
