using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterCardDisplay : MonoBehaviour
{
    [SerializeField] private Button charaCard;
    [SerializeField] private Button RightArrow;
    [SerializeField] private Button LeftArrow;
    [SerializeField] private Switch screen;
    [SerializeField] private DialogueDisplay dialogue;
    [SerializeField] private Transform images;          // Transform of gameObject containing children with images of characters

    private bool canClick = true;
    private bool[] available;       // Whether each character can be talked to

    private int index;              // Current character being displayed

    private Color imageColorToBeUsed = Color.gray;

    [SerializeField] private TextAsset[] convos;    // Dialogue associated with each character


    // Start is called before the first frame update
    void Start()
    {
        // Check that dialogue files and images match
        if (convos.Length != images.childCount)
        {
            Debug.Log("Error: Number of images and number of dialogue files do not match. Disabling dialogue.");
            Destroy(this);
        }

        // Initialize whether each character can be talked to
        available = new bool[convos.Length];
        for (int i = 0; i < convos.Length; i++)
            available[i] = images.GetChild(i).gameObject.activeSelf;

        // Initialize starting character
        index = 0;
        ShowCharacter(index);

        RightArrow.onClick.AddListener(MoveRight);
        LeftArrow.onClick.AddListener(MoveLeft);
        charaCard.onClick.AddListener(Talk);
    }

    // Update is called once per frame
    void Update()
    {
        if (canClick) {
            if (Input.GetKey("right"))
                MoveLeft();
            else if (Input.GetKey("left")) 
                MoveRight();
        }
    }

    void MoveLeft()
    {
        if (canClick)
        {
            HideCharacter(index);
            if (index == 0)
                index = convos.Length - 1;
            else
                index--;
            ShowCharacter(index);
            StartCoroutine(DelayClick());
        }
    }

    void MoveRight(){
        if (canClick)
        {
            HideCharacter(index);
            if (index == convos.Length - 1)
                index = 0;
            else
                index++;
            ShowCharacter(index);
            StartCoroutine(DelayClick());
        }
    }

    void Talk(){
        if (available[index])
        {
            // Deactivate current character
            available[index] = false;
            images.GetChild(index).GetComponent<Image>().color = Color.grey;

            // Switch to dialogue screen
            screen.ShowDialogue();
            screen.HideSelection();
            dialogue.parseAndDisplay(convos[index]);
        }
    }

    // Delay click so you can't spam update
    IEnumerator DelayClick()
    {
        canClick = false;
        yield return new WaitForSeconds(0.2f);
        canClick = true;
    }

    // Show character image corresponding to index
    void ShowCharacter(int index)
    {
        if (index < convos.Length && index >= 0)
        {
            Transform image = images.GetChild(index);
            image.GetComponent<Fade>().FadeIn();
            image.GetComponent<Image>().color = (available[index] ? Color.white : Color.grey);
        }
        else
            Debug.Log("Error: Attempted to show an out of bounds character.");
    }

    // Hide the character image corresponding to index
    void HideCharacter(int index)
    {
        if (index < convos.Length && index >= 0)
        {
            Transform image = images.GetChild(index);
            image.GetComponent<Fade>().FadeOut();
        }
        else
            Debug.Log("Error: Attempted to hide an out of bounds character.");
    }
}
