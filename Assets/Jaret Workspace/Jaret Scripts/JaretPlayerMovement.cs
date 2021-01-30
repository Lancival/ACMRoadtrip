using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JaretPlayerMovement : MonoBehaviour
{

    private MapManager mapManager;

    void Awake()
    {
        mapManager = FindObjectOfType<MapManager>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            movePlayer(mousePosition);

        }
    }

    private void movePlayer(Vector2 mousePosition)
    {
        transform.position = mapManager.PlayerMove(mousePosition);
    }

}
