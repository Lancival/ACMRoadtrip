using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;

    private bool hidden = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // fade out dialogue box
        if (!hidden && Input.GetKeyDown("f"))
        {
            StartCoroutine(FadeOutBox());
            Debug.Log("goodbye");
        }
        // fade in dialogue box
        else if (hidden && Input.GetKeyDown("f"))
        {
            Debug.Log("hello");
            StartCoroutine(FadeInBox());
        }
        if (Input.GetKeyDown("q"))
        {
            Debug.Log("Q");
        }
    }

    IEnumerator FadeOutBox()
    {
        dialogueBox.GetComponent<Fade>().FadeOut();
        hidden = true;
        Debug.Log("fade out");
        yield return new WaitForSeconds(0.2f);
    }

    IEnumerator FadeInBox()
    {
        dialogueBox.GetComponent<Fade>().FadeIn();
        hidden = false;
        Debug.Log("fade in");
        yield return new WaitForSeconds(0.2f);
    } 
}
