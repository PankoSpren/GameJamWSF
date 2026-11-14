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
    [SerializeField] private Tilemap spoolTrail5;

    [SerializeField] private AnimatedTile spoolStraightTopDown;
    [SerializeField] private AnimatedTile spoolStraightLeftRight;
    [SerializeField] private AnimatedTile spoolLeftDown;
    [SerializeField] private AnimatedTile spoolLeftUp;
    [SerializeField] private AnimatedTile spoolRightDown;
    [SerializeField] private AnimatedTile spoolRightUp;


    List<int> previousMoveDirection = new List<int>();
    int moveCount = 0;
    int currentMoveDirection = 0;

    public int maxSpool = 40;
    public int currentSpool = 40;

    
    void Start()
    {

        previousMoveDirection.Add(0);
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
            if (previousMoveDirection[moveCount] == 0)
            {
                CheckMoveDirection(direction);
            }
            else
            {
                CheckMoveDirection(direction);
            }
            if (PlaceThread(direction) == false)
            { 
            return;
            }

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
            //print("PlayerHasMovedUp");
            currentMoveDirection = 1;
        }
        else if (direction.x == 1)
        {
            //print("PlayerHasMovedRight");
            currentMoveDirection = 2;
        }
        else if (direction.y == -1)
        {
            //print("PlayerHasMovedDown");
            currentMoveDirection = -1;
        }
        else if (direction.x == -1)
        {
            //print("PlayerHasMovedLeft");
            currentMoveDirection = -2;
        }
        print(currentMoveDirection);
    }
    bool PlaceThread(Vector2 direction)
    {
        Vector3Int gridPosition = groundTilemap.WorldToCell(transform.position);
        Vector3Int futureGridPostion = groundTilemap.WorldToCell(transform.position + (Vector3)direction);

        if (currentMoveDirection + previousMoveDirection[previousMoveDirection.Count-1] == 0) //the spool is moving back
        {
            gridPosition = groundTilemap.WorldToCell(transform.position + (Vector3)direction);
            PlaceSpoolTile(gridPosition, null, futureGridPostion, true);
            //spoolTrail1.SetTile(gridPosition, null);
            previousMoveDirection.RemoveAt(previousMoveDirection.Count - 1);

            return true;
        }

        if (currentSpool <= 0)
        {
            print("OutofSpool");
            return false;
        }

        else if (previousMoveDirection[previousMoveDirection.Count - 1] == 0)
        {
            //print("This is fist move");
            PlaceSpoolTile(gridPosition, spoolStraightTopDown, futureGridPostion, false);
            //spoolTrail1.SetTile(gridPosition, spoolStraightTopDown);
        }

        else if (previousMoveDirection[previousMoveDirection.Count - 1] == currentMoveDirection && Mathf.Abs(currentMoveDirection) == 2)
        {

            // print("PlaceStraightLine");
            PlaceSpoolTile(gridPosition, spoolStraightLeftRight, futureGridPostion, false);
            //  spoolTrail1.SetTile(gridPosition, spoolStraightLeftRight);
        }
        else if (previousMoveDirection[previousMoveDirection.Count - 1] == currentMoveDirection && Mathf.Abs(currentMoveDirection) == 1)
        {
            //print("PlaceStraightLine");
            PlaceSpoolTile(gridPosition, spoolStraightTopDown, futureGridPostion, false);
            // spoolTrail1.SetTile(gridPosition, spoolStraightTopDown);
        }

        else if (previousMoveDirection[previousMoveDirection.Count - 1] > 0 && Mathf.Abs(previousMoveDirection[previousMoveDirection.Count - 1] + currentMoveDirection) == 1)
        {
            // print("Placea leftdowntile");
            PlaceSpoolTile(gridPosition, spoolLeftDown, futureGridPostion, false);
            //spoolTrail1.SetTile(gridPosition, spoolLeftDown);
        }
        else if (previousMoveDirection[previousMoveDirection.Count - 1] < 0 && Mathf.Abs(previousMoveDirection[previousMoveDirection.Count - 1] + currentMoveDirection) == 1)
        {
            //print("PlacerightUp");
            PlaceSpoolTile(gridPosition, spoolRightUp, futureGridPostion, false);
            // spoolTrail1.SetTile(gridPosition, spoolRightUp);
        }

        else if (previousMoveDirection[previousMoveDirection.Count - 1] == 2 && Mathf.Abs(previousMoveDirection[previousMoveDirection.Count - 1] + currentMoveDirection) == 3)
        {
            // print("Placea leftuptile");
            PlaceSpoolTile(gridPosition, spoolLeftUp, futureGridPostion, false);
            // spoolTrail1.SetTile(gridPosition, spoolLeftUp);
        }
        else if (previousMoveDirection[previousMoveDirection.Count - 1] == -1 && Mathf.Abs(previousMoveDirection[previousMoveDirection.Count - 1] + currentMoveDirection) == 3)
        {
            // print("Placea leftuptile");
            PlaceSpoolTile(gridPosition, spoolLeftUp, futureGridPostion, false);
            //  spoolTrail1.SetTile(gridPosition, spoolLeftUp);
        }

        else if (previousMoveDirection[previousMoveDirection.Count - 1] == -2 && Mathf.Abs(previousMoveDirection[previousMoveDirection.Count - 1] + currentMoveDirection) == 3)
        {
            // print("Placea right down");
            PlaceSpoolTile(gridPosition, spoolRightDown, futureGridPostion, false);
            //spoolTrail1.SetTile(gridPosition, spoolRightDown);
        }
        else if (previousMoveDirection[previousMoveDirection.Count - 1] == 1 && Mathf.Abs(previousMoveDirection[previousMoveDirection.Count - 1] + currentMoveDirection) == 3)
        {
            // print("Placea right down");
            PlaceSpoolTile(gridPosition, spoolRightDown, futureGridPostion, false);
            // spoolTrail1.SetTile(gridPosition, spoolRightDown);
        }
        previousMoveDirection.Add(currentMoveDirection);
        //print(previousMoveDirection);
        return true;
    }

    void PlaceSpoolTile(Vector3Int gridPosition, AnimatedTile tile, Vector3Int futureGidPossiton, bool isReturning)
    {
        if (isReturning == true)
        {
            print("thisshoudgo");
            ChangeSpool(1);
            if (spoolTrail5.HasTile(gridPosition) == true)
            {
                spoolTrail5.SetTile(gridPosition, null);
            }
            else if (spoolTrail4.HasTile(gridPosition) == true)
            {
                spoolTrail4.SetTile(gridPosition, null);
            }
            else if(spoolTrail3.HasTile(gridPosition) == true)
            {
                spoolTrail3.SetTile(gridPosition, null);
            }
            else if(spoolTrail2.HasTile(gridPosition) == true)
            {
    
                spoolTrail2.SetTile(gridPosition, null);
            }
            else if(spoolTrail1.HasTile(gridPosition) == true)
            {
   
                spoolTrail1.SetTile(gridPosition, null);
            }
        }
        else if (spoolTrail5.HasTile(gridPosition) == true)
        {
            print("We Have A problem");
        }
        else if (spoolTrail4.HasTile(gridPosition) == true)
        {
            spoolTrail5.SetTile(gridPosition, tile);
        }
        else if(spoolTrail3.HasTile(gridPosition) == true)
        {
            spoolTrail4.SetTile(gridPosition, tile);
        }
        else if(spoolTrail2.HasTile(gridPosition) == true)
        {
            spoolTrail3.SetTile(gridPosition, tile);
        }
        else if(spoolTrail1.HasTile(gridPosition) == true)
        {
            spoolTrail2.SetTile(gridPosition, tile);
        }
        else 
        {
            spoolTrail1.SetTile(gridPosition, tile);
        }
        if (isReturning == false)
        {
            ChangeSpool(-1);
        }


    }
    void ChangeSpool(int value)
    {
        currentSpool += value;
        hudLogic.changeSpool(value);
        if (currentSpool < 0)
        { 
            currentSpool = 0;
        }
        print("Spool Count " + currentSpool);
    }    
}
