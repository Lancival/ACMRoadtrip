using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    private string name;
    private int powerAttached; //should be changed to an object of type of power rather than int, each int maybe denotes a certain power?
    private bool hasAPower;
    private int level;
    public static List<GameObject> deck = new List<GameObject>();
    public static int currentLevel = 1;
    


    public Card() {
        name = "card";
        powerAttached = 0;
        hasAPower = false;
        level = 1;
    }

    public void incLevel()
    {
        currentLevel++;
    }

    public void deckClear() {
        deck.Clear();
    }

    public Card(string cardName) {
        name = cardName;
        powerAttached = 0;
        hasAPower = false;
    }

    public void setName(string newName) {
        name = newName;
    }

    public void AttachPower(int powerNum) {
        if (!hasAPower)
        {
            powerAttached = powerNum;
            hasAPower = true;
        }
        else {
            Debug.Log("this card already has a power attached");
        }
    }

    public void nextLevel() {
        level++;
    }


    public void SayClicked() {
        Debug.Log("Card Clicked");
    }
}
