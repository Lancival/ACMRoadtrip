using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheckBranch : MonoBehaviour
{

	[SerializeField] private TextMeshProUGUI dialogue;		// Text of dialogue box to check
	[SerializeField] private string[] targets;				// Target lines of dialogue to check for
	[SerializeField] private CharacterCardDisplay ccd;
	[SerializeField] private TextAsset[] files;				// Replacement dialogue files
	[SerializeField] private int index;						// Index of conversation to replace

    // Update is called once per frame
    void Update()
    {
    	for (int i = 0; i < targets.Length; i++)
    	{
	        if (dialogue.text == targets[i])
	        {
	        	ccd.SetAvailable(index);
	        	if (files[i] != null)
	        		ccd.UpdateConvos(index, files[i]);
	        	Destroy(this);
	        }
    	}
    }
}
