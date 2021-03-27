using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JaretGameManager : MonoBehaviour
{
    public GameObject StormPrefab;

    public Vector3[] StormList;


    // Start is called before the first frame update
    void Start()
    {
        foreach (Vector3 i in StormList)
        {
            Debug.Log("Storm is Coming");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

}
