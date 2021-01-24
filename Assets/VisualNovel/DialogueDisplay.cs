using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueDisplay : MonoBehaviour
{

	[SerializeField] private TextMeshProUGUI dialogue;
	[SerializeField] private string text;

    // Start is called before the first frame update
    void Start()
    {
        dialogue = gameObject.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
    	dialogue.SetText(text);
    }
}
