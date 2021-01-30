using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueDisplay : MonoBehaviour
{
    public GameObject buttonPrefab;
    private GameObject button;
    [SerializeField] private string buttonText; 

    [SerializeField] private TextMeshProUGUI dialogue;
    [SerializeField] private TextMeshProUGUI nameBox;
    [SerializeField] private string nameText;
	[SerializeField] private string text;

    // Start is called before the first frame update
    void Start()
    {
        dialogue = gameObject.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        nameBox = gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        ScaleFont();
        
        PrintName();
        StartCoroutine(PrintText());
        Button();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Set name in textbox
    void PrintName()
    {
        nameBox.text = nameText;
        return;
    }

    // Print text letter by letter 
    IEnumerator PrintText()
    {
        string printedText = "";

        for (int i = 0; i < text.Length; i++)
        {
            // if within <>, submit all the consecutive <>'s plus next letter
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

            // sets dialogue speed
            yield return new WaitForSeconds(Settings.DIALOGUE_SPEED);
        }
    }

    // Instantiate button at position (160, 100, 0)
    void Button()
    {
        button = Instantiate(buttonPrefab, new Vector3(160, 100, 0), Quaternion.identity);
        button.transform.SetParent(gameObject.transform);
        button.GetComponentInChildren<TextMeshProUGUI>().text = buttonText;
    }

    // Scale font
    void ScaleFont()
    {
        dialogue.fontSize = dialogue.fontSize*Settings.FONT_SCALE;
        nameBox.fontSize = nameBox.fontSize*Settings.FONT_SCALE;
    }

}
