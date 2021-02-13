using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueDisplay : MonoBehaviour
{
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private TextMeshProUGUI dialogue;
    [SerializeField] private TextMeshProUGUI nameBox;
    [SerializeField] private GameObject indicator;

    [SerializeField] private TextAsset file;
    private List<Dialogue> nodes; 
    private List<int> rCurr;
    private List<int> rNext;
    private int index = 0;
    
    private bool doneTyping = false;
    private bool stopTyping = false;
    private bool canClick = true;
    
    private static readonly string[] names =
    {
            "Misaki",   // ID = 0
            "Summer"    // ID = 1
    };

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
        nodes = Dialogue.Parse(file.text);

        // Display Dialogue
        PrintName(nodes[index].speakerID);
        StartCoroutine(PrintText(nodes[index].content));
    }

    // Update is called once per frame
    void Update()
    {
        if (canClick)
        {
            // left click or space bar or enter/return key
            if (Input.GetMouseButtonUp(0) || Input.GetKeyDown("space") || Input.GetKeyDown("enter") || Input.GetKeyDown("return"))
            {
                if (!doneTyping)
                {
                    stopTyping = true;
                    dialogue.text = nodes[index].content;
                    Debug.Log("stop typing");
                    StartCoroutine(DelayClick());
                }
                else if (index >= 0) 
                {
                    rCurr = nodes[index].Responses();
                    if (rCurr[0] != -1)
                    {
                        rNext = nodes[index+1].Responses();
                        index = rCurr[0]-1;
                        Debug.Log("incremented, index is now " + (rCurr[0]-1));
                        PrintName(nodes[index].speakerID);
                        StartCoroutine(PrintText(nodes[index].content));
                        if (rNext.Count > 1)
                        {
                            canClick = false;
                            for (int i = 0; i < rNext.Count; i++)
                                Button(nodes[rNext[i]-1].content, nodes[rNext[i]-1].dialogueID);
                        }
                    }
                }
            }
        }
    }

    // Delay click so you can't spam update
    IEnumerator DelayClick()
    {
        canClick = false;
        yield return new WaitForSeconds(0.2f);
        canClick = true;
    }

    void TaskOnClick(int dID){
		Debug.Log ("You have clicked the button!");
        index = dID-1;
        canClick = true;
        foreach(Transform child in transform.GetChild(1))
        {
            Destroy(child.gameObject);
        }
	}

    // Set name in the name box
    void PrintName(int sID)
    {
        string name;

        switch (sID) {
        case 0:
            name = "Anna";
            break;
        case 1:
            name = "Bob";
            break;
        default:
            name = "Stranger";
            break;
        }

        nameBox.text = name;
    }

    // Print text letter by letter 
    IEnumerator PrintText(string text)
    {
        indicator.GetComponent<Fade>().FadeOut();

        string printedText = "";

        doneTyping = false;

        for (int i = 0; i < text.Length; i++)
        {
            if (stopTyping)
            {
                stopTyping = false;
                break;
            }

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
        doneTyping = true; // should it be before or after fade
        Debug.Log("done typing");
        indicator.GetComponent<Fade>().FadeIn();
    }

    // Instantiate a button as the child of the Options Container

    /* NOTE: Maybe we should consider object pooling for better performance - instead of
     * instantiating and deleting buttons everytime, we could create buttons and then
     * reuse them, deactivating them when not in use.
     */
    void Button(string buttonText, int dID)
    {
        GameObject button = Instantiate(buttonPrefab, gameObject.transform.GetChild(1));

        TextMeshProUGUI gui = button.GetComponentInChildren<TextMeshProUGUI>();
        gui.text = buttonText;
        gui.fontSize = DEFAULT_FONT_SIZE * Settings.FONT_SCALE;

        button.GetComponent<Fade>().FadeIn();

        Button btn = button.GetComponent<Button>();
        btn.onClick.AddListener(delegate {TaskOnClick(dID);});
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
