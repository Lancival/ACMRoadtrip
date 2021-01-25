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
        StartCoroutine(Print());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Print()
    {
        string printedText = "";

        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] == '<')
            {
                while (i+1 < text.Length)
                {
                    if (text[i] == '>' && text[i+1] != '<')
                    {
                        printedText += text[i];
                        i++;
                        break;
                    }
                    printedText += text[i];
                    i++;
                }
            }

            printedText += text[i];
            dialogue.text = printedText;
            yield return new WaitForSeconds(0.2f);
        }
    }
}
