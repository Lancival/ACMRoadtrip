using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheckBranch : MonoBehaviour
{

	[SerializeField] private TextMeshProUGUI dialogue;		// Text of dialogue box to check
	[SerializeField] private string target;					// Target line of dialogue to check for
	[SerializeField] private CharacterCardDisplay ccd;
	[SerializeField] private TextAsset file;				// Replacement dialogue file
	[SerializeField] private int index;						// Index of conversation to replace

    // Update is called once per frame
    void Update()
    {
        if (dialogue.text == target)
        {
        	ccd.SetAvailable(index);
        	ccd.UpdateConvos(index, file);
        	Destroy(this);
        }
    }
}
