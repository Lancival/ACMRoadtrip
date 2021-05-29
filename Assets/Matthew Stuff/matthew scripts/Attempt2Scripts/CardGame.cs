using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGame : MonoBehaviour
{
    public Sprite[] cardFaces;
    public GameObject cardPrefab;
    public GameObject[] pos;
    public List<string> deck1;

    public int deckPosition;
    //public List<string> bottoms;
    // Start is called before the first frame update
    void Start()
    {
        PlayCards();
        deckPosition = 4;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayCards()
    {
        deck1 = GenerateDeckLevel1();

        foreach (string card in deck1) {
            print(card);
        }
        DealWithIt();
    }

    public static List<string> GenerateDeckLevel1() {
        List<string> newDeck = new List<string>();
        newDeck.Add("Storm");
        newDeck.Add("M_UpUp");
        newDeck.Add("Storm");
        newDeck.Add("M_RightRight");
        newDeck.Add("Storm");
        newDeck.Add("M_RightUp");
        newDeck.Add("M_LeftLeft");
        return newDeck;
    }

    void DealWithIt() {
        print("your mom");
        int i = 0;
        foreach (string card in deck1)
        {
            GameObject newCard = Instantiate(cardPrefab, new Vector3(pos[i].transform.position.x, pos[i].transform.position.y, 0), Quaternion.identity, pos[i].transform);
            newCard.name = card;
            i++;
            if (i == 5)
                break;
        }
        
    }
    /*
    void Sort() {
        for (int i = 0; i < 5; i++) {
            for (int j = i; j < 5; j++) { 
                
            }
        }
    }
    */
}
