using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AddCard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogue;		// Text of dialogue box to check
	[SerializeField] private string[] targets;				// Target lines of dialogue to check for
	[SerializeField] private JCard[] cards;					// Cards to add to deck

	private JaretGameManager manager;

	void Start()
	{
		if (targets.Length != cards.Length)
		{
			Debug.Log("Error: Number of targets and number of cards not equal.");
			Destroy(this);
		}

		GameObject go = GameObject.Find("GameManager");
		if (go)
			manager = go.GetComponent<JaretGameManager>();
		else
		{
			Debug.Log("Error: Unable to locate GameManager.");
			Destroy(this);
		}
	}

    void Update()
    {
    	for (int i = 0; i < targets.Length; i++)
    	{
	        if (dialogue.text == targets[i])
	        {
	        	manager.AddCard(cards[i]);
	        	transform.GetChild(0).GetComponent<Fade>().FadeIn();
	        	transform.GetChild(0).GetComponent<Image>().sprite = cards[i].artwork;
	        	Destroy(this);
	        }
    	}
    }
}
