using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardButton : MonoBehaviour
{

    private GameObject gameManager;
    private GameObject mapManager;
    public SpriteRenderer spriteRenderer;

    private JCard currentCard;
    

    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        mapManager = GameObject.Find("MapManager");

        spriteRenderer.sprite = null;
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
            Debug.Log("Card Played");
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
    }

    public void DebugCardContents()
    {
        Debug.Log(currentCard.cardName);
        Debug.Log(currentCard.artwork);
    }

}
