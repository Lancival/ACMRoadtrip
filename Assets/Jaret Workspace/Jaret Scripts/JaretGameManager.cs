using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JaretGameManager : MonoBehaviour
{


    private int Level = 1;
    private bool[] levelPass = new bool[] { false, false, false, false, false, false };


    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        
    }

    // Update is called once per frame
    void Update()
    {

    }

}
