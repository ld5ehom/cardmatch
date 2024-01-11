//UCLA Ling & Computer Science TaeWook Park
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField]
    private GameObject cardPrefab;

    [SerializeField]
    private Sprite[] cardSprites; // 9 cards

    private List<int> cardIDList = new List<int>();  //card list 
    private List<Card> cardList = new List<Card>();

    // Start is called before the first frame update
    void Start()
    {
        GenerateCardID(); //Get Card List 
        ShuffleCardID(); //random 
        InitBoard(); //start 
    }

    void GenerateCardID()
    {
        for (int i = 0; i < cardSprites.Length; i++) // 0,0,1,1,2,2...8,8 = total 18
        {
            cardIDList.Add(i);
            cardIDList.Add(i);
        }
    }

    void ShuffleCardID()
    {
        int cardCount = cardIDList.Count;
        for(int i = 0 ; i < cardCount ; i++)
        {
            int randomIndex = Random.Range(i,cardCount);
            int temp = cardIDList[randomIndex]; //swap 
            cardIDList[randomIndex] = cardIDList[i];
            cardIDList[i] = temp;
        }
    }

    void InitBoard() 
    {
        float spaceX = 2.2f; // column X 
        float spaceY = 2.7f; // row Y

        int rowCount = 3; // row 
        int colCount = 6; //column

        int cardIndex = 0;

        for (int row = 0; row < rowCount; row++)
        {
            for (int col = 0; col < colCount; col++)
            {
                int cardID = cardIDList[cardIndex++];  // card list (18)
                float posX = (col- (colCount /2)) * spaceX + (spaceX / 2); // X locate 
                float posY = (row - (int)(rowCount / 2)) * spaceY; // Y locate 

                Vector3 pos = new Vector3(posX, posY, 0f);
                GameObject cardObject = Instantiate(cardPrefab, pos, Quaternion.identity);
                Card card = cardObject.GetComponent<Card>();
                card.SetCardID(cardID);
                card.SetSiderealSprite(cardSprites[cardID]);
                cardList.Add(card); //push card to list 

            }
        }
    }

    public List<Card> GetCards()
    {
        return cardList;
    }

}
