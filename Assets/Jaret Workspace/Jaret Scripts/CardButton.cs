using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardButton : MonoBehaviour
{

    private GameObject gameManager;
    private GameObject mapManager;
    public SpriteRenderer spriteRenderer;

    
    private JCard currentCard;
    private JCard tempCard;

    private bool mouseHover = false;
    
    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        mapManager = GameObject.Find("MapManager");

        spriteRenderer.sprite = null;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && mouseHover)
        {
            RightClick();
        }
    }
    
    void OnMouseDown()
    {
        if (!mapManager.GetComponent<MapManager>().NotMyTurn())
        {
            if (currentCard.tutorial && !currentCard.tutorialCorrect)
            {
                return;
            }

            mapManager.GetComponent<MapManager>().PlayCard(currentCard);
            //Debug.Log("Card Played");
            DrawCard();
        }
    }

    public void DrawCard()
    {

        currentCard = gameManager.GetComponent<JaretGameManager>().GetCard(currentCard);
        
        if (currentCard == null)
        {
            spriteRenderer.sprite = null;
            return;
        }    

        spriteRenderer.sprite = currentCard.artwork;
        Destroy(GetComponent<BoxCollider2D>());
        gameObject.AddComponent<BoxCollider2D>();
    }

    public void DebugCardContents()
    {
        Debug.Log(currentCard.cardName);
        Debug.Log(currentCard.artwork);
    }

    private void RightClick()
    {
        mapManager.GetComponent<MapManager>().DisplayCardInstructions(currentCard.cardInstructions);
    }

    private void OnMouseEnter()
    {
        mouseHover = true;
    }
    private void OnMouseExit()
    {
        mouseHover = false;
    }
}
