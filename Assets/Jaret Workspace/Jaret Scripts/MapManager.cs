using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;



public class MapManager : MonoBehaviour
{
    [SerializeField]
    private Tilemap map;


    [SerializeField]
    private List<TileData> tileDatas;

    

    private Dictionary<TileBase, TileData> dataFromTiles;
    private List<GameObject> StormList = new List<GameObject>();


    public GameObject StormPrefab;

    public Vector3[] StormPosition;


    private int turn;



    void Awake()
    {
        turn = 0;
        dataFromTiles = new Dictionary<TileBase, TileData>();

        foreach(var tileData in tileDatas)                 // Create and store the Tiles
        {
            foreach(var tile in tileData.tiles)
            {
                dataFromTiles.Add(tile, tileData);
            }
        }


        /////////////////////////////////////////////////////////// Create and Store the Storm Objects
        
        foreach (Vector3 i in StormPosition)
        {
            Vector3Int stormNum = Vector3Int.FloorToInt(i);
            StormList.Add(Instantiate(StormPrefab, map.CellToWorld(stormNum), this.transform.rotation));
        }

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

    public Vector3 PlayerMove(Vector2 mousePosition)                // Player Object calls this function
    {
        Vector3Int gridPosition = map.WorldToCell(mousePosition);
        return map.CellToWorld(gridPosition);
        turn++;
        if (turn >=5)
        {
            MoveStorms();
        }
    }

    private void MoveStorms()
    {
        foreach( GameObject i in StormList)
        {

        }
    }

    
    



}
