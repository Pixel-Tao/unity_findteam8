using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board : MonoBehaviour
{

    public GameObject card;
    public Vector2 areaMin; // 배치 영역의 최소값 (왼쪽 아래)
    public Vector2 areaMax; // 배치 영역의 최대값 (오른쪽 위)



    void Start()
    {
        
        //PlaceCardsRandomly();

    }

    public List<Card> PlaceCardsRandomly()
    {
        List<Card> cards = new List<Card>();
        
        int[] frontArr = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7 };
        frontArr = frontArr.OrderBy(x => Random.Range(0f, 7f)).ToArray();

        int[] backArr = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
        backArr = backArr.OrderBy(x => Random.Range(0f, 15f)).ToArray();


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
}
