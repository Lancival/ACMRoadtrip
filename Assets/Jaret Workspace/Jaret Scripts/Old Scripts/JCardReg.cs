using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class JCardReg 
{

    private string name;
    private int directionOne;
    private int directionTwo;


    public JCardReg(string _name, int _directionOne = 0, int _directionTwo = 0)
    {
        name = _name;
        directionOne = _directionOne;
        directionTwo = _directionTwo;
    }

    public string getName()
    {
        return name;
    }



}
