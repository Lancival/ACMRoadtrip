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
    [SerializeField] Switch screen;
    [SerializeField] DialogueDisplay dialogue;
    private bool canClick = true;
    private bool canClickChara1 = true;
    private bool canClickChara2 = true;

    private Image image1;
    private Image image2;
    private Color imageColorToBeUsed = Color.gray;

    [SerializeField] private TextAsset[] convos;


    // Start is called before the first frame update
    void Start()
    {
        chara1.SetActive(true);
        chara2.SetActive(false);
        image1 = chara1.GetComponent<Image>();
        image2 = chara2.GetComponent<Image>();

        RightArrow.onClick.AddListener(Click);
        LeftArrow.onClick.AddListener(Click);
        charaCard.onClick.AddListener(Talk);
    }

    // Update is called once per frame
    void Update()
    {
        if (canClick)
        {
            if (Input.GetKey("right") || Input.GetKey("left")) 
            { Move(); }
        }
    }

    void Move(){
        if (chara1.activeSelf == true) {
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
        if (canClick)
        	Move();
    }

    void Talk(){
        if (chara1.activeSelf == true && canClickChara1){  
            screen.ShowDialogue();
            screen.HideSelection(); 
            TextAsset convo = convos[0];
            dialogue.parseAndDisplay(convo);

            canClickChara1 = false;
            image1.color = imageColorToBeUsed;
        }
        else if (chara2.activeSelf == true && canClickChara2) {
            screen.ShowDialogue();
            screen.HideSelection(); 
            TextAsset convo = convos[1];
            dialogue.parseAndDisplay(convo);

            canClickChara2 = false;
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
}
