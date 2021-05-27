using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class JCard : ScriptableObject
{

    public string cardName;

    public Sprite artwork;
    public Sprite UpgradedArtwork;

    public int directionOne = 0;
    public int directionTwo = 0;
    public int directionThree = 0;

    public bool isStorm = false;
    public bool isLighthouse = false;
    public bool isWind = false;
    public bool isShuffle = false;
    public bool isClairvoyant = false;

    private bool cutMovement = false;
    public bool tutorialCorrect = false;
    public bool tutorial = false;

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

    public void UpgradeMovement()
    {
        directionThree = directionOne;
        artwork = UpgradedArtwork;

    }

    public void ReduceMovement()
    {
        cutMovement = true;
    }

    


}
