using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board : MonoBehaviour
{

    public GameObject card;
    public Vector2 areaMin; // 배치 영역의 최소값 (왼쪽 아래)
    public Vector2 areaMax; // 배치 영역의 최대값 (오른쪽 위)

    List<int>[] CardsArray = new List<int>[2];
    void Start()
    {
        
        //PlaceCardsRandomly();

    }

    /// <summary>
    /// 카드를 섞는다.
    /// </summary>
    /// <returns>섞은 카드 번호 배열</returns>
    private int[] ShuffleCardsFront()
    {
        // 카드 섞기
        //int[] cards = Board.GetCardArray()[0].ToArray<int>();

        int[] cards = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7 };
        return cards.OrderBy(x => Random.Range(0f, 7f)).ToArray(); 
    }
    private int[] ShuffleCardsBack()
    {
        // 카드 섞기
        //int[] cards = Board.GetCardArray()[1].ToArray<int>();
        int[] cards = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
        return cards.OrderBy(x => Random.Range(0f, 15f)).ToArray();
    }

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

        return cardList;
    }

    public List<Card> CrazyModeShuffle()
    {
        List<Card> cardList = new List<Card>();

        return cardList;
    }

    public List<int>[] GetCardArray()
    {
        return CardsArray;
    }
}
