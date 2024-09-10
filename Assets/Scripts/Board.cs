using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board : MonoBehaviour
{

    List<int>[] CardsArray = new List<int>[2];
    void Start()
    {

        CardsArray[0] = new List<int>{ 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7 };
        CardsArray[0] = CardsArray[0].OrderBy(x => Random.Range(0f, 7f)).ToList();

        CardsArray[1] = new List<int>{ 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
        CardsArray[1] = CardsArray[1].OrderBy(x => Random.Range(0f, 15f)).ToList();
        /*for (int i = 0; i < 16; i++)
        {
            GameObject go = Instantiate(card, this.transform);

            float x = (i % 4) * 1.4f - 2.1f;
            float y = (i / 4) * 1.4f - 2.0f;

            go.transform.position = new Vector2(x, y);

            int frontImage = frontArr[i];
            int backImage = backArr[i];

            go.GetComponent<Card>().Setting(frontImage, backImage);
        }*/

    }

    public List<int>[] GetCardArray()
    {
        return CardsArray;
    }

}
