using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public GameObject dialogueBox;
    [SerializeField] public GameObject descriptionContainer;
    [SerializeField] public GameObject characterContainer;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
    }
    public void ShowSelection()
    {
        descriptionContainer.GetComponent<Fade>().FadeIn();
        characterContainer.GetComponent<Fade>().FadeIn();
    }
}
