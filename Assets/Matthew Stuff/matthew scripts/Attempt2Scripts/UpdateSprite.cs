using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateSprite : MonoBehaviour
{
    public Sprite cardFace;
    public Sprite cardBack;
    private SpriteRenderer spriteRenderer;
    private CardGame cardGame;

    // Start is called before the first frame update
    void Start()
    {

        List<string> deck = CardGame.GenerateDeckLevel1();
        cardGame = FindObjectOfType<CardGame>();

        //int i = 0;
        foreach (string card in deck) {
            if (this.name == "Storm")
            {
                cardFace = cardGame.cardFaces[0];
            }
            else if (this.name == "M_UpUp")
            {
                cardFace = cardGame.cardFaces[1];
            }
            else if (this.name == "M_RightRight")
            {
                cardFace = cardGame.cardFaces[2];
            }
            else if (this.name == "M_RightUp")
            {
                cardFace = cardGame.cardFaces[3];
            }
            else if (this.name == "M_LeftLeft") {
                cardFace = cardGame.cardFaces[4];
            }
            //i++;
        }
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        List<string> deck = CardGame.GenerateDeckLevel1();
        //int i = 0;
        foreach (string card in deck)
        {
            if (this.name == "Storm")
            {
                cardFace = cardGame.cardFaces[0];
            }
            else if (this.name == "M_UpUp")
            {
                cardFace = cardGame.cardFaces[1];
            }
            else if (this.name == "M_RightRight")
            {
                cardFace = cardGame.cardFaces[2];
            }
            else if (this.name == "M_RightUp")
            {
                cardFace = cardGame.cardFaces[3];
            }
            else if (this.name == "M_LeftLeft")
            {
                cardFace = cardGame.cardFaces[4];
            }
            //i++;
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = cardFace;
        GetMouseClick();
    }

    public void WhenClicked() {
        List<string> deck = CardGame.GenerateDeckLevel1();
        if (cardGame.deckPosition < deck.Count)
        {
            this.name = deck[cardGame.deckPosition];
            cardGame.deckPosition++;
            print(cardGame.deckPosition);
        }
    }

    void GetMouseClick() {
        //print("Get Mouse Click reached");
        if (Input.GetMouseButtonDown(0)) {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10));
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit) {
                if (hit.collider.CompareTag("Card")) {
                    print("card clicked");
                    WhenClicked();
                }
            }
        }
    }
}
