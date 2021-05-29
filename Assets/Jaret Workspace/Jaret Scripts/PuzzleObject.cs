using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleObject : MonoBehaviour
{

    private GameObject mapManager;

    public Sprite shieldShip;
    public Sprite brokenShip;
    public Sprite normShip;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    void Awake()
    {
        mapManager = GameObject.Find("MapManager");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GetTileType()
    {

    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Hit2");
        if (other.gameObject.tag == "Storm")
        {
            
            mapManager.GetComponent<MapManager>().PlayerHit(true);
        }

        if (other.gameObject.tag == "Rock")
        {

            mapManager.GetComponent<MapManager>().PlayerHit(false);
        }

        if (other.gameObject.tag == "Island")
        {
            
            mapManager.GetComponent<MapManager>().PlayerWin();
        }
    }

    public void SetShieldSprite()
    {
        this.GetComponent<SpriteRenderer>().sprite = shieldShip;
    }
    public void SetNormSprite()
    {
        this.GetComponent<SpriteRenderer>().sprite = normShip;
    }
    public void SetBrokenSprite()
    {
        this.GetComponent<SpriteRenderer>().sprite = brokenShip;
    }

    // public void OnCollisionEnter2D(Collision2D other)
    // {
    //     Debug.Log("Hit");
    //     if (other.gameObject.tag == "Storm")
    //     {
    //         mapManager.GetComponent<MapManager>().PlayerHit();
    //     }
    // }
}
