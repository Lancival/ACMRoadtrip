using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KPlayerMovement : MonoBehaviour
{
    private Vector2 targetPosition;
    private Vector3 worldTargetPosition;
    private Vector3 startPosition;
    private KMapManager mapManager;
    public float driftSeconds = 1f;
    private float driftTimer = 0f;

    private bool moving = false;

    public GameObject objMapManager;

    void Awake()
    {
        mapManager = objMapManager.GetComponent<KMapManager>();
        moving = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {


            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldTargetPosition = mapManager.PlayerMove(targetPosition);
            startMove();
            //movePlayer(targetPosition);
        }

        if (moving)
        {
            driftTimer += Time.deltaTime;
            if (driftTimer > driftSeconds)
            {
                stopMove();
            }
            else
            {
                float ratio = driftTimer / driftSeconds;
                transform.position = Vector3.Lerp(startPosition, worldTargetPosition, ratio);
            }
        }

    }


    private void startMove()
    {
        moving = true;
        driftTimer = 0f;
        startPosition = this.transform.position;
    }


    private void stopMove()
    {
        moving = false;
        transform.position = worldTargetPosition;

    }






}
