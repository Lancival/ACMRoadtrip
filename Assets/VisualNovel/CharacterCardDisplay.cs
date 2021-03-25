using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterCardDisplay : MonoBehaviour
{
    [SerializeField] private GameObject character;
    [SerializeField] private TextMeshProUGUI info;

    private List<string> texts = new List<string>
        { "one", "two", "three" };

    private int index = 0;
    private int sizeOfList;
    private bool canClick = true;

    // Start is called before the first frame update
    void Start()
    {
        info.text = texts[index];
        sizeOfList = texts.Count;
    }

    // Update is called once per frame
    void Update()
    {
        if (canClick)
        {
            // the following nested code simulates how I want the screen to act, but the code is not accurate to what we want to do rn
            if (Input.GetKey("right"))
            {
                // traverse list rightwards
                index++;
                if (index > sizeOfList-1)
                    index = 0;
                info.text = texts[index];
                
                StartCoroutine(DelayClick());
            }
            if (Input.GetKey("left"))
            {
                // traverse list leftwards
                index--;
                if (index < 0)
                    index = sizeOfList-1;
                info.text = texts[index];
                StartCoroutine(DelayClick());
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
}
