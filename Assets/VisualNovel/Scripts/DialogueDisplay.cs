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
    [SerializeField] private Switch screen;
    [SerializeField] private SceneLoader loader;
    [SerializeField] private TextAsset file;
    [SerializeField] private Image[] images;

    [SerializeField] private Sprite[] boxes;

    private List<Dialogue> nodes; 
    private List<int> rCurr;
    private List<int> rNext;
    private int index = 0;
    private bool doneTyping = false;
    private bool stopTyping = false;
    private bool canClick = true;

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

        // Display Character Images
        HideImages();
        ShowImages();

        // Display Dialogue
        PrintName(nodes[index].speakerID);
        UpdateMood(nodes[index].speakerID, nodes[index].mood);
        StartCoroutine(PrintText(nodes[index].content));

        /*if (nodes[index].Responses().Count > 1)
            for (int i = 0; i < nodes[index].Responses().Count; i++)
                Button(nodes[nodes[index].Responses()[i] - 1].content, nodes[nodes[index].Responses()[i] - 1].dialogueID);*/
    }

    // Update is called once per frame
    void Update()
    {
        if (canClick && !Settings.PAUSED)
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
                else if (nodes[index].Responses().Count > 1)
                    canClick = false;
                else if (index >= 0) 
                {
                    rCurr = nodes[index].Responses();
                    if (rCurr[0] != -1)
                    {
                        if (nodes[index].speakerID < 18 && images[nodes[index].speakerID])
                            images[nodes[index].speakerID].color = Color.grey;

                        if (rCurr.Count > 1)
                        {
                            canClick = false;
                            for (int i = 0; i < rCurr.Count; i++)
                                Button(nodes[rCurr[i] - 1].content, nodes[rCurr[i] - 1].dialogueID);
                        }
                        else
                        {
                            index = rCurr[0] - 1;
                            StartCoroutine(PrintText(nodes[index].content));
                        }

                        PrintName(nodes[index].speakerID);
                        UpdateMood(nodes[index].speakerID, nodes[index].mood);

                        /*rNext = nodes[index+1].Responses();
                        if (nodes[index].speakerID < 18 && images[nodes[index].speakerID])
                            images[nodes[index].speakerID].color = Color.grey;
                        index = rCurr[0]-1;
                        Debug.Log("incremented, index is now " + (rCurr[0]-1));
                        PrintName(nodes[index].speakerID);
                        UpdateMood(nodes[index].speakerID, nodes[index].mood);
                        StartCoroutine(PrintText(nodes[index].content));
                        if (rNext.Count > 1)
                        {
                            canClick = false;
                            for (int i = 0; i < rNext.Count; i++)
                                Button(nodes[rNext[i]-1].content, nodes[rNext[i]-1].dialogueID);
                        }*/
                    }
                    // Reached the end of the dialogue tree
                    else if (rCurr[0] == -1)
                    {
                        // If in a multi-dialogue scene, return to character-selection
                        if (screen != null)
                        {
                            HideImages();
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
            if (Input.GetKeyDown("t"))
            {
                if (screen != null)
                {
                    HideImages();
                    screen.HideDialogue();
                    screen.ShowSelection();
                }
                else
                    loader.LoadNextScene();
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
                    //Debug.Log("stop typing");
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

    void TaskOnClick(int dID)
    {
        if (nodes[index].speakerID < 18 && images[nodes[index].speakerID])
            images[nodes[index].speakerID].color = Color.grey;
        index = dID-1;
        UpdateMood(nodes[index].speakerID, nodes[index].mood);
        canClick = true;
        foreach(Transform child in transform.GetChild(1))
        {
            Destroy(child.gameObject);
        }
	}

    // Set name in the name box
    void PrintName(int sID)
    {
        nameBox.text = Names.NameList[sID];

        if (sID < 18 && images[sID])
            images[sID].color = Color.white;
        
        if (sID == 18)
        {
            nameBox.transform.parent.gameObject.SetActive(false);
            nameBox.transform.parent.parent.GetComponent<Image>().sprite = boxes[1];
        }
        else
        {
            nameBox.transform.parent.gameObject.SetActive(true);
            nameBox.transform.parent.parent.GetComponent<Image>().sprite = boxes[0];
        }
    }

    // Print text letter by letter 
    IEnumerator PrintText(string text)
    {
        indicator.GetComponent<Fade>().FadeOut();

        string printedText = "";

        doneTyping = false;

        if (stopTyping)
            stopTyping = false;

        for (int i = 0; i < text.Length; i++)
        {
            if (stopTyping)
                break;

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
        //Debug.Log("done typing");
        indicator.GetComponent<Fade>().FadeIn();

        rCurr = nodes[index].Responses();
        if (rCurr.Count > 1)
        {
            canClick = false;
            for (int i = 0; i < rCurr.Count; i++)
                Button(nodes[rCurr[i] - 1].content, nodes[rCurr[i] - 1].dialogueID);
        }
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
        HideImages();
        ShowImages();

        // Display Dialogue
        PrintName(nodes[index].speakerID);
        UpdateMood(nodes[index].speakerID, nodes[index].mood);
        StartCoroutine(PrintText(nodes[index].content));
    }

    void UpdateMood(int speakerID, string mood)
    {
        if (mood != null)
        {
            Animator am = images[speakerID].transform.GetComponent<Animator>();
            if (am)
                am.SetTrigger(mood);
            else
                Debug.Log("Error: Attempted to change mood of speakerID: " + speakerID);
        }
    }

    void ShowImages()
    {
        foreach (Dialogue node in nodes)
        {
            if (node.speakerID < images.Length)
            {
                Image i = images[node.speakerID];
                if(i)
                {
                    i.transform.GetComponent<Fade>().FadeIn();
                    i.color = Color.grey;
                }
            }
            else if (node.speakerID != 18)
                Debug.Log("Error: Attempted to display null image corresponding to speakerID: " + node.speakerID);
        }
    }

    void HideImages()
    {
        foreach (Image i in images)
            if (i && i.gameObject.activeSelf)
                i.transform.GetComponent<Fade>().FadeOut();
    }

}
