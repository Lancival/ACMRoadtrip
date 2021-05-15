using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class JCard : ScriptableObject
{

    public string cardName;

    public Sprite artwork;

    public int directionOne = 0;
    public int directionTwo = 0;
    public int directionThree = 0;

    public bool isStorm = false;

    public void SetThirdDirection(int thirdDirection)
    {
        directionThree = thirdDirection;
    }

    public void AddStormPower()
    {
        isStorm = true;
    }

    public bool IsStorm()
    {
        return isStorm;
    }

    


}
