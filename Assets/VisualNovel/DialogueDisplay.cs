using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueDisplay : MonoBehaviour
{
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private TextMeshProUGUI dialogue;
    [SerializeField] private TextMeshProUGUI nameBox;
    [SerializeField] private GameObject indicator;

    [SerializeField] private TextAsset file;
    
    /*[SerializeField] private string nameText;
	[SerializeField] private string text;
	[SerializeField] private string buttonText;*/

    private float DEFAULT_FONT_SIZE;	// Original font size of text

    // Start is called before the first frame update
    void Start()
    {
    	// Check that all required parameters have been provided
    	bool disable = true;
        if (buttonPrefab == null)
        	Debug.Log("Missing button prefab. Disabling dialogue.");
        else if (dialogue == null)
        	Debug.Log("Missing dialogue box. Disabling dialogue.");
        else if (nameBox == null)
        	Debug.Log("Missing name box. Disabling dialogue.");
        else if (indicator == null)
        	Debug.Log("Missing indicator. Disabling dialogue.");
        else if (file == null)
        	Debug.Log("Missing dialogue text file. Disabling dialogue.");
        else
        	disable = false;

        // If any required parameters are missing, disable this script and its gameObject
        if (disable)
        {
        	gameObject.SetActive(false);
        	return;
        }

        // Scale font
		Settings.FONT_SCALE = 1.0f; // Temporary for testing puposes
        DEFAULT_FONT_SIZE = dialogue.fontSize;
        UpdateFontSize();


        // Parse dialogue
        List<Dialogue> nodes = Dialogue.Parse(file.text);

        // Display Dialogue
        PrintName("Name from Node 0");
        StartCoroutine(PrintText(nodes[0].content));
        Button("Example Button");
    }

    // Update is called once per frame
    /*void Update()
    {
        
    }*/

    // Set name in the name box
    void PrintName(string name)
    {
        nameBox.text = name;
    }

    // Print text letter by letter 
    IEnumerator PrintText(string text)
    {
        indicator.GetComponent<Fade>().FadeOut();

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

        indicator.GetComponent<Fade>().FadeIn();
    }

    // Instantiate a button as the child of the Options Container

    /* NOTE: Maybe we should consider object pooling for better performance - instead of
     * instantiating and deleting buttons everytime, we could create buttons and then
     * reuse them, deactivating them when not in use.
     */
    void Button(string buttonText)
    {
        GameObject button = Instantiate(buttonPrefab, gameObject.transform.GetChild(1));

        TextMeshProUGUI gui = button.GetComponentInChildren<TextMeshProUGUI>();
        gui.text = buttonText;
        gui.fontSize = DEFAULT_FONT_SIZE * Settings.FONT_SCALE;

        button.GetComponent<Fade>().FadeIn();
    }

    // Sets the font size of the dialogue box and name box based on the font scale in Settings
    void UpdateFontSize()
    {
        dialogue.fontSize = DEFAULT_FONT_SIZE * Settings.FONT_SCALE;
        nameBox.fontSize = DEFAULT_FONT_SIZE * 1.33f * Settings.FONT_SCALE;
    }

    // Scale font
    void ScaleFont()
    {
        dialogue.fontSize = dialogue.fontSize*Settings.FONT_SCALE;
        nameBox.fontSize = nameBox.fontSize*Settings.FONT_SCALE;
    }

}
