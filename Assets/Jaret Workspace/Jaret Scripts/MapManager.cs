using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;



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
    private GameObject Player;
    public Text LevelText;


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


        ////////////////////////////////////////////// Start Game


            StartCoroutine(LevelOrder());
        
        
        

    }


    private void Update()
    {
        if(Input.GetMouseButtonDown(0))                                // This spits out the position of a clicked on tile into console, I used it for debugging, not needed in final
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPosition = map.WorldToCell(mousePosition);

            TileBase clickedTile = map.GetTile(gridPosition);

           // Debug.Log("At position" + gridPosition + "there is a " + dataFromTiles[clickedTile].name);


        }
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



    IEnumerator MovePlayer()
    {

        yield return StartCoroutine(ChangeLevelUI());

        while(true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int cellTargetPosition = map.WorldToCell(new Vector2 (targetPosition.x + 0.1f, targetPosition.y+0.1f));
                Vector3Int cellCurrentPosition = map.WorldToCell(new Vector2 (Player.transform.position.x + 0.1f, Player.transform.position.y + 0.1f));
                Vector3 worldTargetPositionUp = map.CellToWorld(new Vector3Int(cellCurrentPosition.x, cellTargetPosition.y, 0));
                Vector3 worldTargetPositionRight = map.CellToWorld(new Vector3Int(cellTargetPosition.x, cellTargetPosition.y, 0));  // right one goes after up, so just use celltarget for both

                

                

                while (Vector3.Distance(Player.transform.position, worldTargetPositionUp) > 0.05f)
                {
                    Player.transform.position = Vector3.Lerp(Player.transform.position, worldTargetPositionUp, (smoothing*1.5f)* Time.deltaTime);

                    yield return null;
                    //yield break;
                }



                while(Vector3.Distance(Player.transform.position, worldTargetPositionRight) > 0.05f)
                {
                    Player.transform.position = Vector3.Lerp(Player.transform.position, worldTargetPositionRight, (smoothing*1.5f) * Time.deltaTime);

                    yield return null;
                   // yield break;
                }



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
            storm.transform.position = Vector3.Lerp(storm.transform.position, targetPos, smoothing * Time.deltaTime);
            yield return null;
        }

        yield break ;
    }

    IEnumerator ChangeLevelUI()
    {
        if (win)
        {
            LevelText.text = "You Win";
            yield break;
        }
        if (death)
        {
            LevelText.text = "Game Over";
            yield break;
        }
        LevelText.text = "Turn " + turn.ToString();

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

    private bool collisionCheckStorm(Vector3 targetPos, GameObject myObject)
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

    public int movePlayerCard(int firstDir, int secondDir, int thirdDir = 0)
    {
        if (firstDir == 1 || firstDir == 3)
        {
            return 0;
        }
        return 1;
    }
    

}
