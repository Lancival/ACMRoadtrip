using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenewCard : MonoBehaviour
{
    public GameObject Card1;
    public GameObject Card2;
    public GameObject Card3;
    public GameObject Card4;
    public GameObject Card5;
    public GameObject Background;

    List<GameObject> cards = new List<GameObject>();
    void Start()
    {
        cards.Add(Card1);
        cards.Add(Card2);
        cards.Add(Card3);
        cards.Add(Card4);
        cards.Add(Card5);
    }

    public void OnClick() {
        Debug.Log("I am attempting to make a new card");
        //for (var i = 0; i < 5; i++) {
            GameObject playerCard = Instantiate(cards[Random.Range(0, cards.Count)], new Vector3(0,0,0), Quaternion.identity);
            playerCard.transform.SetParent(Background.transform, false);
        //}
        //CardBeingPlayed.transform.position   
    }
}
