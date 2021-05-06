using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private GameObject descriptionContainer;
    [SerializeField] private GameObject characterContainer;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("k") )
        {
            if (dialogueBox.activeSelf == true)
            {
                HideDialogue();
                ShowSelection();
            }
            else
            {
                ShowDialogue();
                HideSelection();
            }
        }
    }

    void HideDialogue()
    {
        dialogueBox.GetComponent<Fade>().FadeOut();
    }
    void ShowDialogue()
    {
        dialogueBox.GetComponent<Fade>().FadeIn();
    }
    void HideSelection()
    {
        descriptionContainer.GetComponent<Fade>().FadeOut();
        characterContainer.GetComponent<Fade>().FadeOut();
    }
    void ShowSelection()
    {
        descriptionContainer.GetComponent<Fade>().FadeIn();
        characterContainer.GetComponent<Fade>().FadeIn();
    }
}
