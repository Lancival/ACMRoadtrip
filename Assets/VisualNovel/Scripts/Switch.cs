using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public GameObject dialogueBox;
    [SerializeField] public GameObject descriptionContainer;
    [SerializeField] public GameObject characterContainer;

    private bool state = true;
    private bool talked = false;

    public void HideDialogue()
    {
        dialogueBox.GetComponent<Fade>().FadeOut();
    }
    public void ShowDialogue()
    {
        dialogueBox.GetComponent<Fade>().FadeIn();
    }
    public void HideSelection()
    {
        descriptionContainer.GetComponent<Fade>().FadeOut();
        characterContainer.GetComponent<Fade>().FadeOut();
        state = true;
    }
    public void ShowSelection()
    {
        if (state)
        {
            state = false;
            descriptionContainer.GetComponent<Fade>().FadeIn();
            characterContainer.GetComponent<Fade>().FadeIn();
            characterContainer.GetComponent<CharacterCardDisplay>().IncrementFinished();
            if (!talked)
            {
                characterContainer.GetComponent<CharacterCardDisplay>().SetAvailable(1);
                talked = true;
            }
        }
    }
}
