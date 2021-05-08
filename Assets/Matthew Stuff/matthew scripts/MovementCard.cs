using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementCard : Card
{

    int NONE = 0;
    int UP = 1;
    int DOWN = 2;
    int LEFT = 3;
    int RIGHT = 4;

    private int dir1;
    private int dir2;
    private int dir3;



    public MovementCard(int d1, int d2, int d3 = 0) {
        string s = "";
        dir1 = d1;
        dir2 = d2;
        dir3 = d3;
        if (d1 == UP)
            s += "U";
        if (d1 == DOWN)
            s += "D";
        if (d1 == LEFT)
            s += "L";
        if (d1 == RIGHT)
            s += "R";

        if (d2 == UP)
            s += "U";
        if (d2 == DOWN)
            s += "D";
        if (d2 == LEFT)
            s += "L";
        if (d2 == RIGHT)
            s += "R";

        if (d3 == UP)
            s += "U";
        if (d3 == DOWN)
            s += "D";
        if (d3 == LEFT)
            s += "L";
        if (d3 == RIGHT)
            s += "R";

        setName(s);
    }

    public void DoMovementCardThing() {
        Debug.Log("I am a movement card and I have been played");
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
