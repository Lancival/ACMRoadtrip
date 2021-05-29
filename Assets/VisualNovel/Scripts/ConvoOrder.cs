using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvoOrder : MonoBehaviour
{
	[SerializeField] private int post;	// Index of conversation to set available
	[SerializeField] private int[] pre;	// Indices of conversations that need to be finished first
	[SerializeField] private CharacterCardDisplay ccd;

    private bool[] wasAvailable;

    void Start()
    {
        wasAvailable = new bool[pre.Length];
        for (int i = 0; i < pre.Length; i++)
            wasAvailable[i] = false;
    }

    // Update is called once per frame
    void Update()
    {
    	bool check = true;
        for (int i = 0; i < pre.Length; i++)
        {
        	if (ccd.CheckAvailable(pre[i]))
        	{
                wasAvailable[i] = true;
        		check = false;
        	}
            if (!wasAvailable[i])
                check = false;
        }
        if (check)
        {
        	ccd.SetAvailable(post);
        	Destroy(this);
        }
    }
}
