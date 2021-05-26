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
    [SerializeField] Switch screen;
    [SerializeField] SceneLoader loader;
    [SerializeField] private TextAsset file;
    [SerializeField] private Animator main;
    [SerializeField] private Animator best;

    private List<Dialogue> nodes; 
    private List<int> rCurr;
    private List<int> rNext;
    private int index = 0;
    private bool doneTyping = false;
    private bool stopTyping = false;
    private bool canClick = true;
    
    // TODO: Move character name information to Settings!
    /*private static readonly string[] names =
    {
            "Misaki",   // ID = 0
            "Summer"    // ID = 1
    };*/

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
        UpdateMood(nodes[index].speakerID, nodes[index].mood);
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
                        StartCoroutine(PrintText(nodes[index].content));
                        if (rNext.Count > 1)
                        {
                            canClick = false;
                            for (int i = 0; i < rNext.Count; i++)
                                Button(nodes[rNext[i]-1].content, nodes[rNext[i]-1].dialogueID);
                        }
                    }
                    // Reached the end of the dialogue tree
                    else if (rCurr[0] == -1)
                    {
                        // If in a multi-dialogue scene, return to character-selection
                        if (screen != null)
                        {
                            screen.HideDialogue();
                            screen.ShowSelection();
                        }
                        // Otherwise, move to the next scene
                        else
                            loader.LoadNextScene();
                    }
                }
            }
            // Skip to character selection
            if (Input.GetKeyDown("t") && screen != null)
            {
                screen.HideDialogue();
                screen.ShowSelection();
            }
        }
        else 
        {
            if (Input.GetMouseButtonUp(0) || Input.GetKeyDown("space") || Input.GetKeyDown("enter") || Input.GetKeyDown("return"))
            {
                if (!doneTyping)
                {
                    stopTyping = true;
                    dialogue.text = nodes[index].content;
                    Debug.Log("stop typing");
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
        index = dID-1;
        UpdateMood(nodes[index].speakerID, nodes[index].mood);
        canClick = true;
        foreach(Transform child in transform.GetChild(1))
        {
            Destroy(child.gameObject);
        }
	}

    // Set name in the name box
    // TODO: Move character name information to Settings!
    void PrintName(int sID)
    {
        string name;

        switch (sID) {
        case 0:
            name = "Misaki";
            break;
        case 1:
            name = "Summer";
            break;
        default:
            name = "Default";
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
            if (Settings.DIALOGUE_SPEED > 0)
                yield return new WaitForSeconds(Settings.DIALOGUE_SPEED);
        }
        doneTyping = true; // should it be before or after fade
        Debug.Log("done typing");
        indicator.GetComponent<Fade>().FadeIn();
    }

    // Instantiate a button as the child of the Options Container
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

    // Parse and display dialogue
    public void parseAndDisplay(TextAsset convo)
    {
        index = 0;

        // Parse dialogue
        file = convo;
        nodes = Dialogue.Parse(convo.text);

        // Display Dialogue
        PrintName(nodes[index].speakerID);
        UpdateMood(nodes[index].speakerID, nodes[index].mood);
        StartCoroutine(PrintText(nodes[index].content));
    }

    void UpdateMood(int speakerID, string mood)
    {
        if (mood != null)
        {
            if (speakerID == 0)
                main.SetTrigger(mood);
            else if (speakerID == 1)
                best.SetTrigger(mood);
        }
    }

}
