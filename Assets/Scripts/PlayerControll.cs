using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerControll : MonoBehaviour
{
    public HUDLogic hudLogic;
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private Tilemap wallTilemap;
    [SerializeField] private Tilemap interactableNegative;
    [SerializeField] private Tilemap interactablePositive;

    [SerializeField] private Tilemap spoolTrail1;
    [SerializeField] private Tilemap spoolTrail2;
    [SerializeField] private Tilemap spoolTrail3;
    [SerializeField] private Tilemap spoolTrail4;

    [SerializeField] private AnimatedTile spoolStraightTopDown;
    [SerializeField] private AnimatedTile spoolStraightLeftRight;
    [SerializeField] private AnimatedTile spoolLeftDown;
    [SerializeField] private AnimatedTile spoolLeftUp;
    [SerializeField] private AnimatedTile spoolRightDown;
    [SerializeField] private AnimatedTile spoolRightUp;


    int previousMoveDirection = 0;
    int currentMoveDirection = 0;

    public int maxSpool = 50;
    public int currentSpool = 50;

    // Update is called once per frame
    void Start()
    {
        hudLogic.SetMaxSpool(maxSpool);
        hudLogic.changeSpool(0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) { movePlayer(new Vector2(0, 1)); }
        else if (Input.GetKeyDown(KeyCode.S)) { movePlayer(-new Vector2(0, 1)); }
        else if (Input.GetKeyDown(KeyCode.A)) { movePlayer(-new Vector2(1, 0)); }
        else if (Input.GetKeyDown(KeyCode.D)) { movePlayer(new Vector2(1, 0)); }
    }
    void movePlayer(Vector2 direction)
    {
        // playerCube.transform.position += direction;
        if (MoveCheck(direction))
        {
            if (previousMoveDirection == 0)
            {
                CheckMoveDirection(direction);
            }
            else
            {
                CheckMoveDirection(direction);
            }
            PlaceThread(direction);


            Vector3Int gridPosition = groundTilemap.WorldToCell(transform.position + (Vector3)direction);
            if (InteractableCheck(direction) == 1)
            {
                Debug.Log("Positive interaction");
                currentSpool = currentSpool + 10;
                hudLogic.changeSpool(10);
                interactablePositive.SetTile(gridPosition, null);
            }
            else if (InteractableCheck(direction) == 2)
            {
                Debug.Log("Negative interaction");
                currentSpool = currentSpool - 10;
                hudLogic.changeSpool(-10);
                interactableNegative.SetTile(gridPosition, null);
            }
            else if (InteractableCheck(direction) == 0)
            {
                Debug.Log("no interaction");
            }
            transform.position += (Vector3)direction;
            hudLogic.changeSpool(-1);
        }

    }

    bool MoveCheck(Vector2 direction)
    {
        Vector3Int gridPosition = groundTilemap.WorldToCell(transform.position + (Vector3)direction);
        if (!groundTilemap.HasTile(gridPosition) || wallTilemap.HasTile(gridPosition))
        {
            Debug.Log("No Ground or a Wall at: " + gridPosition);
            return false;
        }
        return true;
    }

    int InteractableCheck(Vector2 direction)
    {
        Vector3Int gridPosition = groundTilemap.WorldToCell(transform.position + (Vector3)direction);
        if (interactablePositive.HasTile(gridPosition))
        {
            Debug.Log("Found positive interactable tile at position: " + gridPosition);
            return 1;
        }
        if (interactableNegative.HasTile(gridPosition))
        {
            Debug.Log("Found negative interactable tile at position: " + gridPosition);
            return 2;
        }
        return 0;
    }
    void CheckMoveDirection(Vector2 direction)
    {
        if (direction.y == 1)
        {
            print("PlayerHasMovedUp");
            currentMoveDirection = 1;
        }
        else if (direction.x == 1)
        {
            print("PlayerHasMovedRight");
            currentMoveDirection = 2;
        }
        else if (direction.y == -1)
        {
            print("PlayerHasMovedDown");
            currentMoveDirection = -1;
        }
        else if (direction.x == -1)
        {
            print("PlayerHasMovedLeft");
            currentMoveDirection = -2;
        }
        print(currentMoveDirection);
    }
    void PlaceThread(Vector2 direction)
    {
        Vector3Int gridPosition = groundTilemap.WorldToCell(transform.position);

        if (currentMoveDirection == previousMoveDirection) //the spool is moving back
        {
            gridPosition = groundTilemap.WorldToCell(transform.position + (Vector3)direction);
            spoolTrail1.SetTile(gridPosition, null);
        }

        else if (previousMoveDirection == 0)
        {
            print("This is fist move");
            spoolTrail1.SetTile(gridPosition, spoolStraightTopDown);
        }
        else if (previousMoveDirection + currentMoveDirection == 0)
        {
            print("The player has moved back");
            spoolTrail1.SetTile(gridPosition, null);
        }
        else if (previousMoveDirection == currentMoveDirection && currentMoveDirection == 2)
        {
            print("PlaceStraightLine");
            spoolTrail1.SetTile(gridPosition, spoolStraightLeftRight);
        }
        else if (previousMoveDirection == currentMoveDirection && currentMoveDirection == 1)
        {
            print("PlaceStraightLine");
            spoolTrail1.SetTile(gridPosition, spoolStraightTopDown);
        }

        else if (previousMoveDirection > 0 && Mathf.Abs(previousMoveDirection + currentMoveDirection) == 1)
        {
            print("Placea leftdowntile");
            spoolTrail1.SetTile(gridPosition, spoolLeftDown);
        }
        else if (previousMoveDirection < 0 && Mathf.Abs(previousMoveDirection + currentMoveDirection) == 1)
        {
            print("PlacerightUp");
            spoolTrail1.SetTile(gridPosition, spoolRightUp);
        }

        else if (previousMoveDirection == 2 && Mathf.Abs(previousMoveDirection + currentMoveDirection) == 3)
        {
            print("Placea leftuptile");
            spoolTrail1.SetTile(gridPosition, spoolLeftUp);
        }
        else if (previousMoveDirection == -1 && Mathf.Abs(previousMoveDirection + currentMoveDirection) == 3)
        {
            print("Placea leftuptile");
            spoolTrail1.SetTile(gridPosition, spoolLeftUp);
        }


        else if (previousMoveDirection == -2 && Mathf.Abs(previousMoveDirection + currentMoveDirection) == 3)
        {
            print("Placea right down");
            spoolTrail1.SetTile(gridPosition, spoolRightDown);
        }
        else if (previousMoveDirection == 1 && Mathf.Abs(previousMoveDirection + currentMoveDirection) == 3)
        {
            print("Placea right down");
            spoolTrail1.SetTile(gridPosition, spoolRightDown);
        }

        previousMoveDirection = currentMoveDirection;
        print(previousMoveDirection);
    }
}
