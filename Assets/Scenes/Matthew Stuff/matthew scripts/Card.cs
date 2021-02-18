using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card
{
    private string name;
    private int powerAttached; //should be changed to an object of type of power rather than int, each int maybe denotes a certain power?
    private bool hasAPower;

    public Card() {
        name = "card";
        powerAttached = 0;
        hasAPower = false;
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
}
