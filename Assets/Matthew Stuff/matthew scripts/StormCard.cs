using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormCard : Card
{
    private int xCoord;
    private int yCoord;

    public StormCard(int x, int y) : base("Storm"){
        xCoord = x;
        yCoord = y;
    }

    public void DoStorm()
    {
        Debug.Log("I am a storm card and I have been played");
    }

    public int x() {
        return xCoord;
    }

    public int y()
    {
        return yCoord;
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
