using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JaretGameManager : MonoBehaviour
{


    private int Level = 1;
    private bool[] levelPass = new bool[] { false, false, false, false, false, false };

    public List<JCard> StartingDeck;   // insert starting deck

    public List<JCard> TutorialDeck;


    //////////////////////////////////////////////
    [SerializeField]
    private List<JCard> DiscardDeck;
    [SerializeField]
    private List<JCard> PlayableDeck;

    [SerializeField]
    private List<JCard> Hand;

    [SerializeField]
    private List<JCard> Deck;

    private List<JCard> TutorialCorrectDeck;

    private List<JCard> ShuffleDeck;
    

    


    


    void Awake()
    {
        DiscardDeck = new List<JCard>();
        TutorialCorrectDeck = new List<JCard>();
        PlayableDeck = new List<JCard>();
        Hand = new List<JCard>();
        Deck = new List<JCard>();
        ShuffleDeck = new List<JCard>();

        foreach(JCard card in StartingDeck)
        {
            Deck.Add(Object.Instantiate(card));
        }
        
        foreach(JCard card in TutorialDeck)
        {
            TutorialCorrectDeck.Add(Object.Instantiate(card));
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

       /* if (Level == 1)
        {
            
            int length = TutorialDeck.Count;
            for (int i = 0; i < length; i++)
            {
                TutorialCorrectDeck[i].tutorial = true;
                if (i == 1 || i==6 || i == 5)
                {
                    TutorialCorrectDeck[i].tutorialCorrect = true;
                }
                PlayableDeck.Add(TutorialCorrectDeck[i]);
            }
        }
        else*/
       // {
            int length = Deck.Count;
            for (int i = 0; i < length; i++)
            {
                PlayableDeck.Add(Deck[Random.Range(0, Deck.Count)]);

                Deck.Remove(PlayableDeck[i]);

            }

            foreach (JCard card in PlayableDeck)   // dont use equal sign, they are just pointing to the same thing.  This loop makes two separate decks that can be edited differently
            {
                Deck.Add(card);
            }
            
       // }


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
            Hand.Remove(oldCard);
            DiscardDeck.Add(oldCard);

            if (PlayableDeck.Count == 0)
                return null;

            temp = PlayableDeck[0];


            Hand.Add(temp);
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

    public int GetLevel()
    {
        return Level;
    }

    public void Shuffle()         // Shuffle Playable Deck
    {
        if (ShuffleDeck.Count != 0)
        {
            Debug.Log("remove shuffle");
            for (int i = ShuffleDeck.Count - 1; i > -1; i--)
            {
                ShuffleDeck.Remove(ShuffleDeck[i]);
            }
        }

        
        int length = PlayableDeck.Count;
        for (int i = 0; i < length; i++)
        {
            Debug.Log(i);
            ShuffleDeck.Add(PlayableDeck[Random.Range(0, PlayableDeck.Count)]);

            PlayableDeck.Remove(ShuffleDeck[i]);

        }
        foreach (JCard card in ShuffleDeck)   // dont use equal sign, they are just pointing to the same thing.  This loop makes two separate decks that can be edited differently
        {
            PlayableDeck.Add(card);
        }
    }

    public Sprite TopCard(int cardPosition)
    {
        return PlayableDeck[cardPosition].artwork;
    }

    public int DeckLength()
    {
        return PlayableDeck.Count;
    }


}
