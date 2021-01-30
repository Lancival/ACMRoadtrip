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

    void Awake()
    {
        dataFromTiles = new Dictionary<TileBase, TileData>();

        foreach(var tileData in tileDatas)
        {
            foreach(var tile in tileData.tiles)
            {
                dataFromTiles.Add(tile, tileData);
            }
        }
    }


    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPosition = map.WorldToCell(mousePosition);

            TileBase clickedTile = map.GetTile(gridPosition);

            Debug.Log("At position" + gridPosition + "there is a " + clickedTile);


        }
    }

    public Vector3 PlayerMove(Vector2 mousePosition)
    {
        Vector3Int gridPosition = map.WorldToCell(mousePosition);
        return map.CellToWorld(gridPosition);
    }

    
    



}
