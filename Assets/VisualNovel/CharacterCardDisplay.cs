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
    [SerializeField] private GameObject chara1;
    [SerializeField] private GameObject chara2;
    private bool canClick = true;


    // Start is called before the first frame update
    void Start()
    {
        chara1.SetActive(true);
        chara2.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (canClick)
        {
            // the following nested code simulates how I want the screen to act, but the code is not accurate to what we want to do rn
            if (Input.GetKey("right") || Input.GetKey("left")) 
            { Move(); }

            RightArrow.onClick.AddListener(click);
            LeftArrow.onClick.AddListener(click);
        }
    }

    void Move(){
        if (chara1.activeSelf == true){
            chara1.SetActive(false);
            chara2.SetActive(true);
        }
        else {
            chara1.SetActive(true);
            chara2.SetActive(false);
        }
        StartCoroutine(DelayClick());
    }

    void click(){
        Debug.Log ("You have clicked the button!");
        Move();
    }

    // Delay click so you can't spam update
    IEnumerator DelayClick()
    {
        canClick = false;
        yield return new WaitForSeconds(0.2f);
        canClick = true;
    }

    // IEnumerator TraverseRight()    
    // {
    //     gameObject.GetComponent<Fade>().FadeOut();
    //     yield return new WaitForSeconds(0.5f);
    //     // traverse list rightwards
    //     index++;
    //     if (index > sizeOfList-1)
    //         index = 0;
    //     character = sprites[index];
    //     gameObject.GetComponent<Fade>().FadeIn();
    // }

    // IEnumerator TraverseLeft()    
    // {
    //     gameObject.GetComponent<Fade>().FadeOut();
    //     yield return new WaitForSeconds(0.5f);
    //     // traverse list leftwards
    //     index--;
    //     if (index < 0)
    //         index = sizeOfList-1;
    //     character = sprites[index];
    //     gameObject.GetComponent<Fade>().FadeIn();
    // }
}
