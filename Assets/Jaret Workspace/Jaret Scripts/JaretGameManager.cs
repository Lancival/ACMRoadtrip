using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JaretGameManager : MonoBehaviour
{


    private int Level = 1;
    private bool[] levelPass = new bool[] { false, false, false, false, false, false };

    public List<JCard> StartingDeck;   // insert starting deck

    public List<JCard> TutorialDeck;

    private List<JCard> DiscardDeck;
    [SerializeField]
    private List<JCard> PlayableDeck;

    [SerializeField]
    private List<JCard> Hand;

    [SerializeField]
    private List<JCard> Deck;

    


    //public string[] startingDeck;


    void Awake()
    {
        DiscardDeck = new List<JCard>();
        
        PlayableDeck = new List<JCard>();
        Hand = new List<JCard>();
        Deck = new List<JCard>();

        foreach(JCard card in StartingDeck)
        {
            Deck.Add(Object.Instantiate(card));
        }

        DontDestroyOnLoad(this.gameObject);
        //Deck.Remove(Deck[8]);

        CreatePlayableDeck();
    }

    // Start is called before the first frame update
    void Start()
    {
        

        
        
        
    }

    private void CreatePlayableDeck()
    {
        

        
        int length = Deck.Count;
        for (int i = 0; i < length; i++)
        {
            PlayableDeck.Add(Deck[Random.Range(0, Deck.Count)]);
           
            Deck.Remove(PlayableDeck[i]);
            
        }
        
        foreach(JCard card in PlayableDeck)   // dont use equal sign, they are just pointing to the same thing.  This loop makes two separate decks that can be edited differently
        {
            Deck.Add(card);
        }
        //PlayableDeck.Remove(Deck[0]);  Debug line
        
    }

    public JCard GetCard(JCard oldCard)
    {
        JCard temp;

        if (oldCard == null)
        {
            if (PlayableDeck.Count == 0)
                return null;

            temp = PlayableDeck[0];


            Hand.Add(temp);
            PlayableDeck.Remove(temp);
            return temp;
        }
        else
        {
            if (PlayableDeck.Count == 0)
                return null;

            temp = PlayableDeck[0];


            Hand.Add(temp);
            Hand.Remove(oldCard);
            PlayableDeck.Remove(temp);
            return temp;
        }

        
        
    }

    public Sprite GetCardSprite()
    {
        return Deck[0].artwork;
    }

    public void AddCard(JCard card)
    {
        Deck.Add(card);
    }

}
