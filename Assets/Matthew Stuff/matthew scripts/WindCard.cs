using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindCard : Card
{
    private int direction;

    int UP = 1;
    int DOWN = 2;
    int LEFT = 3;
    int RIGHT = 4;

    public WindCard(int dir) : base("Wind") {
        direction = dir;
    }

    public void DoWind() {
        Debug.Log("I am a wind card and I have been played");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
