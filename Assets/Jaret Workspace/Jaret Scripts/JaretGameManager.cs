using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JaretGameManager : MonoBehaviour
{


    private int Level = 0;
    [SerializeField]
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

    [SerializeField]
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
        //CreatePlayableDeck();

    }

    // Start is called before the first frame update
    void Start()
    {

        



    }

    public void EmptyDiscard()
    {
        int length = DiscardDeck.Count;
        for (int i = 0; i < length; i++)
        {
            DiscardDeck.Remove(DiscardDeck[0]);
        }
        
    }

    public void EmptyHand()
    {
        int length = Hand.Count;
        for (int i = 0; i < length; i++)
        {
            Hand.Remove(Hand[0]);
        }
        
    }

    public void CreatePlayableDeck()
    {

        if (Level == 1)
        {
            Debug.Log("Level 1");
            int length = TutorialCorrectDeck.Count;
            for (int i = 0; i < length; i++)
            {
                PlayableDeck.Add(TutorialCorrectDeck[Random.Range(0, TutorialCorrectDeck.Count)]);

                TutorialCorrectDeck.Remove(PlayableDeck[i]);

            }

            foreach (JCard card in PlayableDeck)   // dont use equal sign, they are just pointing to the same thing.  This loop makes two separate decks that can be edited differently
            {
                TutorialCorrectDeck.Add(card);
            }

        }
        else
        {
            Debug.Log("Level other");
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
            
        }


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

    public void NextLevel()
    {
        Level++;
        Debug.Log(Level);
    }

    public void WinLevel()
    {
        levelPass[Level - 1] = true;
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
        if (ShuffleDeck.Count != 0)
        {
            foreach (JCard card in ShuffleDeck)   // dont use equal sign, they are just pointing to the same thing.  This loop makes two separate decks that can be edited differently
            {
                PlayableDeck.Add(card);
            }
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

    public int PuzzlesWon()
    {
        int won = 0;
        foreach (bool wonLevel in levelPass)
            if (wonLevel)
                won++;
        return won;
    }


}
