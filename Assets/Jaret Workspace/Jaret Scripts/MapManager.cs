using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;



public class MapManager : MonoBehaviour
{
    [SerializeField]
    private Tilemap map;


    [SerializeField]
    private List<TileData> tileDatas;

    

    private Dictionary<TileBase, TileData> dataFromTiles;
    private List<GameObject> StormList = new List<GameObject>();

    public float smoothing = 2f;
    public GameObject StormPrefab;
    public GameObject Player;
    public Text LevelText;

    public Vector3[] StormPosition;   //input how many storms and where they are located in Unity Interface


    private int turn;



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


        /////////////////////////////////////////////////////////// Create and Store the Storm Objects based on inputted positions
        
        foreach (Vector3 i in StormPosition)
        {
            Vector3Int stormNum = Vector3Int.FloorToInt(i);
            StormList.Add(Instantiate(StormPrefab, map.CellToWorld(stormNum), this.transform.rotation));
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

            Debug.Log("At position" + gridPosition + "there is a " + clickedTile);


        }
    }

    IEnumerator LevelOrder()
    {
        if (turn > 10)
            yield break;

        yield return StartCoroutine(MoveStorms());
        turn++;
        yield return StartCoroutine(LevelOrder());
        if (turn > 10)
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
                Vector3 worldTargetPosition = map.CellToWorld(map.WorldToCell(targetPosition));

                while(Vector3.Distance(Player.transform.position, worldTargetPosition) > 0.05f)
                {
                    Player.transform.position = Vector3.Lerp(Player.transform.position, worldTargetPosition, smoothing * Time.deltaTime);

                    yield return null;
                }

                yield break;
            }

            yield return null;
        }

    }

    IEnumerator MoveStorms()
    {
        yield return StartCoroutine(MovePlayer());
        

        foreach( GameObject i in StormList)
        {
            
            yield return StartCoroutine(MoveOneStorm(i));
        }

        yield break;
    }

    IEnumerator MoveOneStorm(GameObject storm)
    {
        
        Vector3 targetPos = StormTarget(storm.transform.position);
        Debug.Log(targetPos);
        Debug.Log(storm.transform.position);

        while (Vector3.Distance(storm.transform.position, targetPos) > 0.05f)
        {
            storm.transform.position = Vector3.Lerp(storm.transform.position, targetPos, smoothing * Time.deltaTime);
            yield return null;
        }

        yield break ;
    }

    IEnumerator ChangeLevelUI()
    {
        LevelText.text = "Turn " + turn.ToString();
        yield break;
    }









    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// Potentially need to change the functions below //////////////
    /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public Vector3 PlayerMove(Vector2 mousePosition)                // Player Object calls this function
    {
        Vector3Int gridPosition = map.WorldToCell(mousePosition);
        turn++;
        if (turn >=5 && turn % 3 == 2)   ///// Only move storms every third turn after the fifth turn
        {
            MoveStorms();
        }
        return map.CellToWorld(gridPosition);
    }

    private Vector3 StormTarget(Vector3 currentPos)                     // Moves all storms in a randome direction, currently it is a filler function that can be changed based on future implementation
    {
        Vector3Int intNewPosition;
        Vector3 newPosition;
        Debug.Log(currentPos);
            int dir = Random.Range(0, 2);
            if (dir == 1)
            {
                int up = Random.Range(0, 2);   /// max is exclusive while min is inclusive for int values
                if (up == 1)
                {
                    intNewPosition = map.WorldToCell(currentPos);
                    intNewPosition.y += up;
                    return  map.CellToWorld(intNewPosition);

                }
                else
                {
                    intNewPosition = map.WorldToCell(currentPos);
                    intNewPosition.y -= 1;
                    return  map.CellToWorld(intNewPosition);
                }
            }
            else
            {
                int right = Random.Range(0, 2);
                if (right == 1)
                {
                    intNewPosition = map.WorldToCell(currentPos);
                    intNewPosition.x += right;
                    return  map.CellToWorld(intNewPosition);

                }
                else
                {
                    intNewPosition = map.WorldToCell(currentPos);
                    intNewPosition.x -= 1;
                    return  map.CellToWorld(intNewPosition);
                }
            }
            
        
    }


    private bool oceanCurrent(Vector2 playerPosition)
    {
        return true;
    }

    
    



}
