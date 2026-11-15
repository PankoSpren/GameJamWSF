using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class PlayerControll : MonoBehaviour
{
    public HUDLogic hudLogic;
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private Tilemap wallTilemap;
    [SerializeField] private Tilemap interactableNegative;
    [SerializeField] private Tilemap interactablePositive;
    [SerializeField] private Tilemap buttonTile;

    [SerializeField] private Tilemap spoolTrail1;
    [SerializeField] private Tilemap spoolTrail2;
    [SerializeField] private Tilemap spoolTrail3;
    [SerializeField] private Tilemap spoolTrail4;
    [SerializeField] private Tilemap spoolTrail5;

    [SerializeField] private AnimatedTile[] spoolStart;
    [SerializeField] private AnimatedTile spoolStraightTopDown;
    [SerializeField] private AnimatedTile spoolStraightLeftRight;
    [SerializeField] private AnimatedTile spoolLeftDown;
    [SerializeField] private AnimatedTile spoolLeftUp;
    [SerializeField] private AnimatedTile spoolRightDown;
    [SerializeField] private AnimatedTile spoolRightUp;
    [SerializeField] private AnimatedTile catAttack;

    [SerializeField] private GameObject top;
    [SerializeField] private GameObject down;
    [SerializeField] private GameObject left;
    [SerializeField] private GameObject right;

    [SerializeField] private AnimatedTile capturedButton;
    [SerializeField] private AnimatedTile normalButton;

    public Animator animator;


    AudioManager audioManager;

    Vector3Int startingLocation;

    List<int> previousMoveDirection = new List<int>();
    int victoryMoveCount = 0;
    int currentMoveDirection = 0;

    int collectedButtons = 0;
    int buttonCount = 1;
    bool victoryMarchInProgress = false;
    bool noActionsAvalable = false;

    int lastCoinCaptureCount;
    


    public int maxSpool = 36;
    public int currentSpool = 36;

    private void Awake()
    {
    
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        top.SetActive(false);
        down.SetActive(false);
        left.SetActive(false);
        right.SetActive(false);

        startingLocation = groundTilemap.WorldToCell(transform.position);
        previousMoveDirection.Add(0);
        hudLogic.SetMaxSpool(maxSpool);
        hudLogic.changeSpool(0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        { CatAttack(); }
        if (victoryMarchInProgress == true || noActionsAvalable == true) { return; }
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
            if (previousMoveDirection[0] == 0)
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

                ChangeSpool(4);
                audioManager.PlaySFX(audioManager.star); 
                hudLogic.SetMaxSpool(maxSpool + 4);
                maxSpool = maxSpool + 4;
                /*currentSpool = currentSpool + 10;
                hudLogic.changeSpool(10);*/
                interactablePositive.SetTile(gridPosition, null);
            }
            else if (InteractableCheck(direction) == 2)
            {
                Debug.Log("Negative interaction");

                ChangeSpool(-4);
                CatAttack();
                audioManager.PlaySFX(audioManager.cat);
                hudLogic.SetMaxSpool(maxSpool - 4);
                maxSpool = maxSpool - 4;
                /* currentSpool = currentSpool - 10;
                 hudLogic.changeSpool(-10);*/
                Vector3Int modifiedGridPosition = new Vector3Int(gridPosition.x - 1, gridPosition.y, 0);
                interactableNegative.SetTile(gridPosition, null);
            }
            else if (InteractableCheck(direction) == 3)
            {
                if (buttonTile.ContainsTile(capturedButton) == false)
                {
                    Debug.Log("Button");
                    collectedButtons += 1;
                    buttonTile.SetTile(gridPosition, capturedButton);
                    audioManager.PlaySFX(audioManager.collect);
                    lastCoinCaptureCount = previousMoveDirection.Count - 1;
                    print("Number of buttons collected: " + collectedButtons);
                }

            }
          
            else if (InteractableCheck(direction) == 0)
            {
                Debug.Log("no interaction");
            }

            transform.position += (Vector3)direction;

        }

        CheckTransitionConditions();
        CheckVictoryConditions();
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
        if (buttonTile.HasTile(gridPosition))
        {
            Debug.Log("Found button tile at position: " + gridPosition);
            return 3;
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

    }
    bool PlaceThread(Vector2 direction)
    {
        Vector3Int gridPosition = groundTilemap.WorldToCell(transform.position);
        Vector3Int futureGridPostion = groundTilemap.WorldToCell(transform.position + (Vector3)direction);
   

        if (currentMoveDirection + previousMoveDirection[previousMoveDirection.Count - 1] == 0) //the spool is moving back
        {
            gridPosition = groundTilemap.WorldToCell(transform.position + (Vector3)direction);
            PlaceSpoolTile(gridPosition, null, futureGridPostion, true);


            Vector3Int currentTile = groundTilemap.WorldToCell(transform.position);
            if (buttonTile.HasTile(currentTile) == true && lastCoinCaptureCount == previousMoveDirection.Count - 1)
            {
                collectedButtons -= 1;
                buttonTile.SetTile(currentTile, normalButton);

                print("Coin Count" + collectedButtons);
            }

            AddTail(direction, false, previousMoveDirection[previousMoveDirection.Count - 2]);


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
            AnimatedTile firstMove;
            //print("This is fist move");
            if (direction.x == 1)
            {
                 firstMove = spoolStart[0];
            }
            else
            {
                 firstMove = spoolStart[1];
            }
                PlaceSpoolTile(gridPosition, firstMove, futureGridPostion, false);
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

        AddTail(direction, true, 0);


        return true;
    }

    void PlaceSpoolTile(Vector3Int gridPosition, AnimatedTile tile, Vector3Int futureGidPossiton, bool isReturning)
    {
        if (isReturning == true)
        {


            ChangeSpool(1);


            if (spoolTrail5.HasTile(gridPosition) == true)
            {
                spoolTrail5.SetTile(gridPosition, null);
            }
            else if (spoolTrail4.HasTile(gridPosition) == true)
            {
                spoolTrail4.SetTile(gridPosition, null);
            }
            else if (spoolTrail3.HasTile(gridPosition) == true)
            {
                spoolTrail3.SetTile(gridPosition, null);
            }
            else if (spoolTrail2.HasTile(gridPosition) == true)
            {

                spoolTrail2.SetTile(gridPosition, null);
            }
            else if (spoolTrail1.HasTile(gridPosition) == true)
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
        else if (spoolTrail3.HasTile(gridPosition) == true)
        {
            spoolTrail4.SetTile(gridPosition, tile);
        }
        else if (spoolTrail2.HasTile(gridPosition) == true)
        {
            spoolTrail3.SetTile(gridPosition, tile);
        }
        else if (spoolTrail1.HasTile(gridPosition) == true)
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
        if (currentSpool > maxSpool)
        {
            currentSpool = maxSpool;
        }
        print("Spool Count " + currentSpool);
    }
    void CheckVictoryConditions()
    {
        if (groundTilemap.WorldToCell(transform.position) == startingLocation && buttonCount <= collectedButtons && currentSpool == 0)
        {
            print("Victory");
            PerformVictoryMarch();
        }
    }
    void PerformVictoryMarch()
    {
        victoryMarchInProgress = true;
        buttonCount += 1;
        collectedButtons = 0;
        hudLogic.SpawnVictory();

        left.SetActive(false);
        top.SetActive(false);
        down.SetActive(false);
        right.SetActive(false);

        InvokeRepeating("PerformVictoryMarchMove", 0.25f, 0.25f);

    }
    void PerformVictoryMarchMove()
    {
        //print("Spool count " + currentSpool);
        victoryMoveCount++;
        //currentSpool += 1;

        CheckTransitionConditions();

        //AddTail(new Vector2(0, 0), false, previousMoveDirection[previousMoveDirection.Count - victoryMoveCount+1]); Will not implement

        PlaceSpoolTile(groundTilemap.WorldToCell(transform.position), null, groundTilemap.WorldToCell(transform.position), true);
        if (previousMoveDirection[previousMoveDirection.Count - victoryMoveCount] == -1)
        {
            Vector2 i = new Vector2(0, 1);
            transform.position += (Vector3)i;
        }

        if (previousMoveDirection[previousMoveDirection.Count - victoryMoveCount] == -2)
        {
            Vector2 i = new Vector2(1, 0);
            transform.position += (Vector3)i;
        }

        if (previousMoveDirection[previousMoveDirection.Count - victoryMoveCount] == 1)
        {
            Vector2 i = new Vector2(0, -1);
            transform.position += (Vector3)i;
        }

        if (previousMoveDirection[previousMoveDirection.Count - victoryMoveCount] == 2)
        {
            Vector2 i = new Vector2(-1, 0);
            transform.position += (Vector3)i;
        }


        print(previousMoveDirection[previousMoveDirection.Count - victoryMoveCount]);

        if (victoryMoveCount == previousMoveDirection.Count - 1)
        {


            CancelInvoke("PerformVictoryMarchMove");
            print("YouHaveReachedTheStart");
            print("YouHavePrformed " + (previousMoveDirection.Count - 1) + " moves");
            print("In the vicotrymarch you moved times; " + victoryMoveCount);

            SceneManager.LoadScene("Game2");
        }
    }




    void CheckTransitionConditions()
    {
        float sigleBlock = maxSpool / 18;
        int x = 17;
        //print(playerScript.maxSpool + "  " + playerScript.currentSpool + "  " + sigleBlock);

        while (x > 0)
        {
            if (sigleBlock * x < currentSpool)
            {
                animator.SetInteger("TransitionInt", (17 - x));
                x = 0; break;
            }
            else { x--; }
        }
    }


    void CatAttack()
    {
        noActionsAvalable = true;
        Vector3Int gridposition = groundTilemap.WorldToCell(transform.position - new Vector3 (0, 1, 0));
        
        interactableNegative.SetTile(gridposition, catAttack);
        Invoke("KillKat", 2);
    }
    void KillKat()
    {
        Vector3Int gridposition = groundTilemap.WorldToCell(transform.position - new Vector3(0, 1, 0));
        interactableNegative.SetTile(gridposition, null);
    }

    void AddTail(Vector2 direction, bool isMovingForward, int reversedMove)
    {
        if (isMovingForward == true)
        {
            if (direction.x == 1)
            {


                left.SetActive(true);
                top.SetActive(false);
                down.SetActive(false);
                right.SetActive(false);
            }
            else if (direction.x == -1)
            {
                left.SetActive(false);
                top.SetActive(false);
                down.SetActive(false);
                right.SetActive(true);
            }
            else if (direction.y == 1)
            {
                left.SetActive(false);
                top.SetActive(false);
                down.SetActive(true);
                right.SetActive(false);
            }
            else if (direction.y == -1)
            {
                left.SetActive(false);
                top.SetActive(true);
                down.SetActive(false);
                right.SetActive(false);
            }
        }
        else
        {
            if (reversedMove == 1)
            {
                left.SetActive(false);
                top.SetActive(false);
                down.SetActive(true);
                right.SetActive(false);


            }
            else if (reversedMove == -1)
            {

                left.SetActive(false);
                top.SetActive(true);
                down.SetActive(false);
                right.SetActive(false);

            }
            else if (reversedMove == 2)
            {
                left.SetActive(true);
                top.SetActive(false);
                down.SetActive(false);
                right.SetActive(false);
            }
            else if (reversedMove == -2)
            {

                left.SetActive(false);
                top.SetActive(false);
                down.SetActive(false);
                right.SetActive(true);
            }
        }

    }
}
