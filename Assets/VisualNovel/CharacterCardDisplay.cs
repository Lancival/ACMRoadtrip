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

    private Image image1;
    private Image image2;
    private Color imageColorToBeUsed = Color.gray;


    // Start is called before the first frame update
    void Start()
    {
        chara1.SetActive(true);
        chara2.SetActive(false);
        image1 = chara1.GetComponent<Image>();
        image2 = chara2.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canClick)
        {
            // the following nested code simulates how I want the screen to act, but the code is not accurate to what we want to do rn
            if (Input.GetKey("right") || Input.GetKey("left")) 
            { Move(); }

            RightArrow.onClick.AddListener(Click);
            LeftArrow.onClick.AddListener(Click);
        }

        charaCard.onClick.AddListener(Talk)
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

    void Click(){
        Debug.Log ("You have clicked the button!");
        Move();
    }

    void Talk(){
        if (chara1.activeSelf == true){   
            image1.color = imageColorToBeUsed;
        }
        else {
            image2.color = imageColorToBeUsed;
        }
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
