using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerControll : MonoBehaviour
{
    public HUDLogic hudLogic;
    [SerializeField]private Tilemap groundTilemap;
    [SerializeField]private Tilemap wallTilemap;
    [SerializeField]private Tilemap interactableNegative;
    [SerializeField]private Tilemap interactablePositive;

    public int maxSpool = 100;
    public int currentSpool = 50; 

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) {movePlayer(new Vector2(0, 1));}
        else if (Input.GetKeyDown(KeyCode.S)) {movePlayer(-new Vector2(0, 1));}
        else if (Input.GetKeyDown(KeyCode.A)) {movePlayer(-new Vector2(1, 0));}
        else if (Input.GetKeyDown(KeyCode.D)) {movePlayer(new Vector2(1, 0));}
    }
    void movePlayer(Vector2 direction)
    {
       // playerCube.transform.position += direction;
       if (MoveCheck(direction))
       {
            if (InteractableCheck(direction) == 1)
            {
                Debug.Log("Positive interaction");
                currentSpool = currentSpool + 10;
                hudLogic.changeSpool(10);
            }
            else if (InteractableCheck(direction) == 2)
            {
                Debug.Log("Negative interaction");
                currentSpool = currentSpool - 10;
                hudLogic.changeSpool(-10);
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
}
