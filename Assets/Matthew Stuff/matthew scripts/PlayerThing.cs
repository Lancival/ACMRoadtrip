using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThing : MonoBehaviour
{
    private double xCoord = 0;
    private double yCoord = 0;

    double getX() {
        return xCoord;
    }
    double getY() {
        return yCoord;
    }
    void setX(double x) {
        xCoord = x;
    }
    void setY(double y) {
        yCoord = y;
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
