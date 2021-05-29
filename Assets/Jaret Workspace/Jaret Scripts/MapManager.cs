using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;



/// <summary>
/// /////////////////////////////// Note:
///                                        map.WorldToCell has what I think are rounding errors at 0 and will lose functionality. There is probably
///                                            a better way to fix this, but I fixed it by offsetting the vector I passed into the function by 0.2f.
///                                            This should work as long as the tile width is greater than the value used to offset in this case 0.2f
///                                            
/// 
/// 
///                                 Int direction code:
///                                     1 = Up
///                                     2 = Right
///                                     3 = Down
///                                     4 = Left
/// </summary>

public class MapManager : MonoBehaviour
{
    [SerializeField]
    private Tilemap map;


    [SerializeField]
    private List<TileData> tileDatas;

    

    private Dictionary<TileBase, TileData> dataFromTiles;
    private List<GameObject> MovableStormList = new List<GameObject>();
    private List<GameObject> StormList = new List<GameObject>();
    private List<GameObject> RockList = new List<GameObject>();

    public float smoothing = 2f;


    public GameObject StormPrefab;
    public GameObject PlayerPrefab;
    public GameObject RockPrefab;
    public GameObject MovableStormPrefab;
    public GameObject HandPlacementPrefab;
    public GameObject cardButtonPrefab;
    public GameObject clairvoyanceUIPrefab;
    public GameObject InstructionsUIPrefab;
    


    private GameObject Player;
    private GameObject handPlacement;
    private List<GameObject> cardButtonList = new List<GameObject>();


    public Text LevelText;
    public TextMeshProUGUI LevelTextPro;

    public string nextScene;


    public Vector3 playerSpawn;
    public Vector3[] StormPosition;   //input how many storms and where they are located in Unity Interface
    public Vector3[] MovableStormPosition;
    public Vector3[] RockPosition;
    public int stormBeginTurn = 5;
    public int stormMovInterval = 3;
    public int turnLimit = 10;


    private int turn;
    private bool death = false;
    private bool win = false;
    private bool cardPlayed = false;
    private bool notMyTurn = true;
    private bool shield = false;

    private Vector2 targetPosition;
    private Vector3 worldTargetPosition;
    private JCard cardInPlay;
    private GameObject gameManager;
    private GameObject cardOverlay;
    private GameObject transitionOverlay;

    private GameObject cardInstructionsOverlay;
    private string cardInstructions;
    private bool isCardInstructions = false;



    void Awake()
    {
        turn = 1;
        dataFromTiles = new Dictionary<TileBase, TileData>();

        foreach(var tileData in tileDatas)                 // Create and store the Tiles
        {
            foreach(var tile in tileData.tiles)
            {
                dataFromTiles.Add(tile, tileData);
            }
        }

        /////////////////////////////////////////////////////////// Create the Player
        Player = Instantiate(PlayerPrefab, map.CellToWorld(Vector3Int.FloorToInt(playerSpawn)), this.transform.rotation);



        /////////////////////////////////////////////////////////// Create and Store the movableStorm Objects based on inputted positions

        foreach (Vector3 i in MovableStormPosition)
        {
            Vector3Int movStormNum = Vector3Int.FloorToInt(i);
            MovableStormList.Add(Instantiate(MovableStormPrefab, map.CellToWorld(movStormNum), this.transform.rotation));
        }


        /////////////////////////////////////////////////////////// Create and store Rocks and nonMovableStorms

        foreach (Vector3 i in StormPosition)
        {
            Vector3Int stormNum = Vector3Int.FloorToInt(i);
            StormList.Add(Instantiate(StormPrefab, map.CellToWorld(stormNum), this.transform.rotation));
        }


        foreach (Vector3 i in RockPosition)
        {
            Vector3Int rockNum = Vector3Int.FloorToInt(i);
            RockList.Add(Instantiate(RockPrefab, map.CellToWorld(rockNum), this.transform.rotation));

        }

        /////////////////////////////////////////////////////////// Create Hand Object
        ///

        handPlacement = Instantiate(HandPlacementPrefab, new Vector3(0, -1 * map.size.y/3, 0), this.transform.rotation);

        for (int i = 0; i < handPlacement.transform.childCount; i++)
        {
            cardButtonList.Add(Instantiate(cardButtonPrefab, handPlacement.transform.GetChild(i).transform.position, this.transform.rotation));
        }


        
        
        
        

    }

    void Start()
    {
        transitionOverlay = GameObject.Find("Transition Canvas");
        gameManager = GameObject.Find("GameManager");
        gameManager.GetComponent<JaretGameManager>().NextLevel();
        gameManager.GetComponent<JaretGameManager>().EmptyDiscard();
        gameManager.GetComponent<JaretGameManager>().EmptyHand();
        gameManager.GetComponent<JaretGameManager>().CreatePlayableDeck();
        

        ////////////////////////////////////////////// Start Game


        StartCoroutine(LevelOrder());
    }


    private void Update()
    {
        /*if(Input.GetMouseButtonDown(0))                                // This spits out the position of a clicked on tile into console, I used it for debugging, not needed in final
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPosition = map.WorldToCell(mousePosition);

            TileBase clickedTile = map.GetTile(gridPosition);

           // Debug.Log("At position" + gridPosition + "there is a " + dataFromTiles[clickedTile].name);


        }*/

        
    }

    IEnumerator LevelOrder()
    {
        if (turn > turnLimit)
            yield break;

        yield return StartCoroutine(OceanCurrents());
        turn++;
        yield return StartCoroutine(LevelOrder());
        if (turn > turnLimit)
            yield break;
    }

    IEnumerator DrawCards()
    {
        yield return StartCoroutine(ChangeLevelUI());

        if (turn == 1)
        {
           
            foreach (GameObject cardButton in cardButtonList)
            {
                cardButton.GetComponent<CardButton>().DrawCard();
                //yield return new WaitForSeconds(0.25f);
            }
            
        }
        yield break;
    }

    IEnumerator WindCurrent()
    {
        

        yield return new WaitForSeconds(1);
            
            while (true)
            {
                
                if (Input.GetMouseButtonDown(0))
                {
                    
                    Vector2 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector3Int cellTargetPosition = map.WorldToCell(new Vector2(targetPosition.x + 0.1f, targetPosition.y + 0.1f));

                    foreach (GameObject i in StormList)
                    {
                        Vector3Int stormPosition = map.WorldToCell(new Vector2(i.transform.position.x + 0.1f, i.transform.position.y + 0.1f));
                        //Vector3 currentPosition = i.transform.position;
                        if (stormPosition.y == cellTargetPosition.y)
                        {
                            Vector3 worldTarget = map.CellToWorld(new Vector3Int(stormPosition.x + 1, stormPosition.y, 0));
                            while (Vector3.Distance(i.transform.position, worldTarget) > 0.05f)
                            {
                                i.transform.position = Vector3.Lerp(i.transform.position, worldTarget, smoothing * Time.deltaTime);
                            yield return null;
                            }
                        }
                    }

                foreach (GameObject i in MovableStormList)
                {
                    Vector3Int stormMovPosition = map.WorldToCell(new Vector2(i.transform.position.x + 0.1f, i.transform.position.y + 0.1f));
                    //Vector3 currentPosition = i.transform.position;
                    if (stormMovPosition.y == cellTargetPosition.y)
                    {
                        Vector3 worldTarget = map.CellToWorld(new Vector3Int(stormMovPosition.x + 1, stormMovPosition.y, 0));
                        while (Vector3.Distance(i.transform.position, worldTarget) > 0.05f)
                        {
                            i.transform.position = Vector3.Lerp(i.transform.position, worldTarget, smoothing * Time.deltaTime);
                            yield return null;
                        }
                    }
                }

                yield break;
                }
                yield return null;
            }
        
        
        yield break;
    }

    


    IEnumerator MovePlayer()
    {

        yield return StartCoroutine(DrawCards());

        notMyTurn = false;

        while(true)
        {
            if (Input.GetMouseButtonDown(0) && isCardInstructions)
            {
                Destroy(cardInstructionsOverlay);
                isCardInstructions = false;
            }

            ///////////////////////////////////////////////////////////////////////
            if (cardPlayed)
            {

                if (cardInPlay.isShuffle)
                {
                    gameManager.GetComponent<JaretGameManager>().Shuffle();
                    cardPlayed = false;
                    yield break;
                }

                if (cardInPlay.isClairvoyant)
                {
                    cardOverlay = Instantiate(clairvoyanceUIPrefab);

                    int length = gameManager.GetComponent<JaretGameManager>().DeckLength();
                    for (int i = 0; i < length && i < 3; i++)
                    {
                        
                        cardOverlay.transform.GetChild(i + 1).gameObject.GetComponent<Image>().sprite = gameManager.GetComponent<JaretGameManager>().TopCard(i);
                    }
                    

                    cardPlayed = false;
                    yield return new WaitForSeconds(1);
                    while(true)
                    {
                        
                        if (Input.GetMouseButtonDown(0))
                        {
                            
                            Destroy(cardOverlay);
                            yield break;
                        }
                        yield return null;
                    }
                }

                if (cardInPlay.IsStorm())
                {
                    //Vector3 stormSpawnLocation = StormSpawn();
                    //StormList.Add( Instantiate( StormPrefab, map.CellToWorld( Vector3Int.FloorToInt(stormSpawnLocation)), this.transform.rotation));
                    StormSpawn();
                    cardPlayed = false;
                    yield break;
                }

                if (cardInPlay.isLighthouse)
                {
                    shield = true;
                    cardPlayed = false;
                    yield break;
                }

                if (cardInPlay.isWind)
                {
                    cardOverlay = Instantiate(clairvoyanceUIPrefab);
                    for (int i = 0; i < 3; i++)
                    {
                        Destroy(cardOverlay.transform.GetChild(i + 1).gameObject);
                    }
                    cardOverlay.transform.GetChild(0).GetComponent<TMP_Text>().text = "Click a tile to move Storms";
                    Destroy(cardOverlay.transform.GetChild(0).transform.GetChild(0).gameObject);
                    yield return StartCoroutine(WindCurrent());
                    Destroy(cardOverlay);
                    
                }

                worldTargetPosition = map.CellToWorld(  CalculateDirection(  cardInPlay.directionOne, map.WorldToCell(  new Vector2 (  Player.transform.position.x + 0.1f, Player.transform.position.y + 0.1f  )  ), cardInPlay.directionThree  )  );

                while (Vector3.Distance(Player.transform.position, worldTargetPosition) > 0.05f)
                {
                    Player.transform.position = Vector3.Lerp(Player.transform.position, worldTargetPosition, (smoothing * 1.5f) * Time.deltaTime);

                    
                    cardPlayed = false;
                    yield return null;
                }


                worldTargetPosition = map.CellToWorld(CalculateDirection(cardInPlay.directionTwo, map.WorldToCell(new Vector2(Player.transform.position.x + 0.1f, Player.transform.position.y + 0.1f))));

                while (Vector3.Distance(Player.transform.position, worldTargetPosition) > 0.05f)
                {
                    Player.transform.position = Vector3.Lerp(Player.transform.position, worldTargetPosition, (smoothing * 1.5f) * Time.deltaTime);

                    cardPlayed = false;
                    yield return null;
                }


                


                cardPlayed = false;

                yield break;
            }



            

            
            yield return null;
        }

    }

    IEnumerator MoveStorms()
    {
        yield return StartCoroutine(MovePlayer());
        
        if (turn >= stormBeginTurn && ((turn-stormBeginTurn)%stormMovInterval) == 0)
        {
            foreach (GameObject i in MovableStormList)
            {

                yield return StartCoroutine(MoveOneStorm(i));
            }
        }
        

        yield break;
    }

    IEnumerator MoveOneStorm(GameObject storm)
    {
        
        Vector3 targetPos = StormTarget(storm.transform.position);
        //Vector3 targetPos = DebugStormTarget(storm.transform.position);   this function checks if collision storm is working

        if (collisionCheckStorm(targetPos, storm))
        {
            Debug.Log("CollisionStorm");
            yield break;
        }

        if (!onMapCheck(targetPos))
        {
            
            yield break;
        }

        while (Vector3.Distance(storm.transform.position, targetPos) > 0.05f)
        {
            storm.transform.position = Vector3.Lerp(storm.transform.position, targetPos, 1.5f * smoothing * Time.deltaTime);
            yield return null;
        }

        yield break ;
    }

    IEnumerator ChangeLevelUI()
    {
        if (win)
        {
            LevelTextPro.text = "You Win";
            gameManager.GetComponent<JaretGameManager>().WinLevel();
            yield return new WaitForSeconds(1);
            transitionOverlay.transform.GetChild(0).GetComponent<SceneLoader>().LoadNextScene();
           /* if (nextScene != null)
            {

                SceneManager.LoadScene(nextScene);
            }*/
               

            yield break;
        }
        if (death)
        {
            LevelTextPro.text = "Game Over";
            yield return new WaitForSeconds(1);
            transitionOverlay.transform.GetChild(0).GetComponent<SceneLoader>().LoadNextScene();

            yield break;
        }
        LevelTextPro.text = "Turn " + turn.ToString();

        yield break;
    }

    IEnumerator OceanCurrents()
    {
        yield return StartCoroutine(MoveStorms());
        
        Vector3Int gridTargetPosition = map.WorldToCell(new Vector3(Player.transform.position.x+.1f, Player.transform.position.y+.1f,Player.transform.position.z));  // this is to fix rounding errors

        /*if (map.CellToWorld(gridTargetPosition) != Player.transform.position)
        {
            Debug.Log("Bad Conversion");
            Debug.Log(gridTargetPosition);                                         //////// if statement here is for debugging, positions were not lining up, uncomment if happens again
            Debug.Log(map.CellToWorld(gridTargetPosition));
            Debug.Log(Player.transform.position);
        }*/
        Vector3 TargetPosition;
        
        int temp = oceanCurrentDir(Player.transform.position);
        
        switch(oceanCurrentDir(Player.transform.position))
        {
            case 1:                                //////////// up Current
                {
                    gridTargetPosition.y++;
                    break;
                }
            case 2:                                /////////// right Current
                {
                    gridTargetPosition.x++;
                    break;
                }
            case 3:                               //////////// down Current
                {
                    gridTargetPosition.y--;
                    break;
                }
            case 4:                                /////////// left Current
                {
                    gridTargetPosition.x--;
                    break;
                }
            default:                              //////////// Not on Current
                {
                    //Debug.Log("default");
                    yield break;
                }
        }
        
        TargetPosition = map.CellToWorld(gridTargetPosition);

        while(Vector3.Distance(Player.transform.position, TargetPosition) > 0.05f)
        {
            Player.transform.position = Vector3.Lerp(Player.transform.position, TargetPosition, smoothing * Time.deltaTime);

            yield return null;
        }

        yield break;
        
    }



    public void PlayerHit()
    {
        if (shield)
        {
            shield = false;
            return;
        }
        death = true; 
    }

    public void PlayerWin()
    {
        win = true;
    }






    

    /*public Vector3 PlayerMove(Vector2 mousePosition)                // Player Object calls this function
    {
        Vector3Int gridPosition = map.WorldToCell(mousePosition);
        turn++;
        if (turn >=5 && turn % 3 == 2)   ///// Only move storms every third turn after the fifth turn
        {
            MoveStorms();
        }
        return map.CellToWorld(gridPosition);
    }
    */





    private Vector3 StormTarget(Vector3 currentPos)                     // Moves all storms in a randome direction, currently it is a filler function that can be changed based on future implementation
    {
        Vector3Int intNewPosition;
        Vector3 newPosition;
        
            int dir = Random.Range(0, 2);
            if (dir == 1)
            {
                int up = Random.Range(0, 2);   /// max is exclusive while min is inclusive for int values
                if (up == 1)
                {
                    intNewPosition = map.WorldToCell(new Vector3(currentPos.x + .2f, currentPos.y + .2f, currentPos.z));
                    intNewPosition.y += up;
                    return  map.CellToWorld(intNewPosition);

                }
                else
                {
                    intNewPosition = map.WorldToCell(new Vector3(currentPos.x + .2f, currentPos.y + .2f, currentPos.z));
                    intNewPosition.y -= 1;
                    return  map.CellToWorld(intNewPosition);
                }
            }
            else
            {
                int right = Random.Range(0, 2);
                if (right == 1)
                {
                    intNewPosition = map.WorldToCell(new Vector3(currentPos.x + .2f, currentPos.y + .2f, currentPos.z));
                    intNewPosition.x += right;
                    return  map.CellToWorld(intNewPosition);

                }
                else
                {
                    intNewPosition = map.WorldToCell(new Vector3(currentPos.x + .2f, currentPos.y + .2f, currentPos.z));
                    intNewPosition.x -= 1;
                    return  map.CellToWorld(intNewPosition);
                }
            }
            
        
    }


    private Vector3 DebugStormTarget(Vector3 currentPos)
    {
        Vector3Int intNewPosition;

        intNewPosition = map.WorldToCell(currentPos);
        intNewPosition.x += 1;
        return map.CellToWorld(intNewPosition);

    }


    private int oceanCurrentDir(Vector2 objectPosition)
    {
        Vector2 testVector = new Vector2(objectPosition.x + .2f, objectPosition.y + .2f);
        Vector3Int gridPosition = map.WorldToCell(testVector);
        TileBase clickedTile = map.GetTile(gridPosition);
        
        if (clickedTile == null)
        {
            //Debug.Log("null tile");
            return 0;
        }
        if (dataFromTiles[clickedTile].name == "Current")
        {
            return dataFromTiles[clickedTile].dirInt;
        }
        //Debug.Log(dataFromTiles[clickedTile].name);
        return 0;
    }

    private bool collisionCheckStorm(Vector3 targetPos, GameObject myObject = null)
    {
        Vector3 testVector1 = new Vector3(targetPos.x + .2f, targetPos.y + .2f, targetPos.z);
        Vector3 testVector2;

        Vector3Int targetPosInt = map.WorldToCell(targetPos);
        foreach(GameObject i in MovableStormList)
        {
            testVector2.x = i.transform.position.x + .2f;
            testVector2.y = i.transform.position.y + .2f;
            testVector2.z = i.transform.position.z;

            if (myObject != i && targetPosInt == map.WorldToCell(testVector2))
            {
                return true;
            }
        }

        foreach (GameObject i in StormList)
        {
            testVector2.x = i.transform.position.x + .2f;
            testVector2.y = i.transform.position.y + .2f;
            testVector2.z = i.transform.position.z;

            if (myObject != i && targetPosInt == map.WorldToCell(testVector2))
            {
                return true;
            }
        }
        Debug.Log("false");
        return false;
    }

    private bool collisionCheckRock(Vector3 targetPos, GameObject myObject)
    {
        Vector3 testVector1 = new Vector3(targetPos.x + .2f, targetPos.y + .2f, targetPos.z);
        Vector3 testVector2;

        Vector3Int targetPosInt = map.WorldToCell(targetPos);
        foreach (GameObject i in RockList)
        {
            testVector2.x = i.transform.position.x + .2f;
            testVector2.y = i.transform.position.y + .2f;
            testVector2.z = i.transform.position.z;

            if (myObject != i && targetPosInt == map.WorldToCell(testVector2))
            {
                return true;
            }
        }

        
        Debug.Log("false");
        return false;
    }
    

    private bool onMapCheck(Vector3 targetPos)
    {

        Vector3Int gridPosition = map.WorldToCell(new Vector3(targetPos.x+0.1f, targetPos.y+0.1f,targetPos.z));

        TileBase clickedTile = map.GetTile(gridPosition);

        if (clickedTile == null)
        {
            Debug.Log("off map");
            return false;
        }
        Debug.Log("on map");
        return true;
        // Debug.Log("At position" + gridPosition + "there is a " + dataFromTiles[clickedTile].name);

    }

    

    public void PlayCard(JCard card)
    {
        notMyTurn = true;
        cardPlayed = true;
        cardInPlay = card;
        
        
    }
    
    private Vector3Int CalculateDirection(int direction, Vector3Int startingPoint, int upgrade = 0)
    {
        Vector3Int endPoint = new Vector3Int(startingPoint.x, startingPoint.y, 0);


        switch (direction)
        {
            case 1:                                //////////// up 
                {
                    endPoint.y += (2 + upgrade);
                    break;
                }
            case 2:                                /////////// right 
                {
                    endPoint.x += (2 + upgrade);
                    break;
                }
            case 3:                               //////////// down 
                {
                    endPoint.y -= (2 + upgrade);
                    break;
                }
            case 4:                                /////////// left 
                {
                    endPoint.x -= (2 + upgrade);
                    break;
                }
            case 5:
                {
                    int i = Random.Range(0, 5);
                    endPoint = CalculateDirection(i, endPoint);
                    break;
                }
            default:                              //////////// No Movement
                {
                    //Debug.Log("default");
                     break;
                }
        }

        return endPoint;
    }

    public bool NotMyTurn()
    {
        return notMyTurn;
    }

    private void StormSpawn()
    {
        
        int xCoord = Random.Range(-1 * ((map.size.x/2) - 1), (map.size.x/2)-1);
        int yCoord = Random.Range(-1 * ((map.size.y / 2) - 1), (map.size.y / 2) - 1);
        Vector3 temp = new Vector3(xCoord, yCoord, 0);

        GameObject tempStorm = Instantiate(StormPrefab, map.CellToWorld(Vector3Int.FloorToInt(temp)), this.transform.rotation);
        StormList.Add(tempStorm);




        //collisionCheckStorm(map.CellToWorld(Vector3Int.FloorToInt(temp)))
        int limit = 0;
        while (collisionCheckStorm(tempStorm.transform.position, tempStorm) || CollisionCheckPlayer(tempStorm.transform.position) )
        {
            //temp = StormSpawn();
            limit++;
            changeStormPosition(tempStorm);
            if (limit > 6)
                return;
        }

        
    }

    private void changeStormPosition(GameObject storm)
    {
        
        int xCoord = Random.Range(-1 * ((map.size.x / 2) - 1), (map.size.x / 2) - 1);
        int yCoord = Random.Range(-1 * ((map.size.y / 2) - 1), (map.size.y / 2) - 1);
        Vector3 temp = new Vector3(xCoord, yCoord, 0);

        storm.transform.position = temp;
    }

    private bool CollisionCheckPlayer(Vector3 targetPosition)
    {
        Vector3Int testVectorInt = map.WorldToCell(new Vector3(Player.transform.position.x + .2f, Player.transform.position.y + .2f, 0));

        Vector3Int targetVectorInt = map.WorldToCell(new Vector3(targetPosition.x + .2f, targetPosition.y + .2f, 0));

        if (testVectorInt == targetVectorInt)
            return true;
        return false;
    }
    
    public void DisplayCardInstructions(string _cardInstructions)
    {
        cardInstructions = _cardInstructions;
        
        if (!isCardInstructions)
        {
            cardInstructionsOverlay = Instantiate(InstructionsUIPrefab);
            isCardInstructions = true;
        }
        
        cardInstructionsOverlay.transform.GetChild(0).transform.GetChild(0).GetComponent<TMP_Text>().text = cardInstructions;

    }
}
