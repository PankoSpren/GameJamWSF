using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;



public class PlayerMovement : MonoBehaviour
{
    bool performedMove;
    public float xStartPosition;
    public float yStartPosition;
    Vector2 currentPossition = new Vector2(-2.5f, -3.5f);
    bool canMoveUp;
    bool canMoveDown;
    bool canMoveLeft;
    bool canMoveRight;

    [SerializeField] Tilemap groundTile;
    [SerializeField] Tile Tile;
    [SerializeField] Vector3Int newTilePosition;


    public GameObject[] colliders;

    void Start()
    {
 
        ClearWallBools();
        DeployColliders();
    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetKeyDown(KeyCode.W) == true || Input.GetKeyDown(KeyCode.S) == true)
        {
    
            MoveVertical(Input.GetKeyDown(KeyCode.W));
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            MoveHorizontal(Input.GetKeyDown(KeyCode.A));
        }

    }
    bool MoveVertical(bool direction)
    {

        if (direction == true)
        {
            if (canMoveUp == false)
            {
                print("MoveNo");
                return false;
            }
            DeployLine();
            print("MoveUp");
            
            transform.position = new Vector3(currentPossition.x, currentPossition.y + 1);
            newTilePosition = new Vector3Int(newTilePosition.x, newTilePosition.y + 1, newTilePosition.z);

        }
        else if (direction == false)
        {
            if (canMoveDown == false)
            {
                print("MoveNo");
                return false;
            }
            DeployLine();
            print("Movedown");
            transform.position = new Vector3(currentPossition.x, currentPossition.y - 1);
            newTilePosition = new Vector3Int(newTilePosition.x, newTilePosition.y - 1, newTilePosition.z);
        }

 

        currentPossition = transform.position;
        ClearWallBools();
        DeployColliders();
        return true;
    }
    bool MoveHorizontal(bool direction)
    {
        if (direction == true)
        {
            if (canMoveLeft == false)
            {
                print("MoveNo");
                return false;
            }
            DeployLine();
            print("Moveleft");
            transform.position = new Vector3(currentPossition.x - 1, currentPossition.y);
            newTilePosition = new Vector3Int(newTilePosition.x - 1, newTilePosition.y, newTilePosition.z);
        }
        else if (direction == false)
        {
            if (canMoveRight == false)
            {
                print("MoveNo");
                return false;
            }
            DeployLine();
            print("Moveright");
            transform.position = new Vector3(currentPossition.x + 1, currentPossition.y);
            newTilePosition = new Vector3Int(newTilePosition.x + 1, newTilePosition.y, newTilePosition.z);
        }

        

        currentPossition = transform.position;
        ClearWallBools();
        DeployColliders();
        return true;
    }

    public void CheckWall(int direction)
    {
        if (direction == 0)
        {
            print("cannotmoveup");
            canMoveUp = false;
        }
        if (direction == 1)
        {
            print("cannotmoveright");
            canMoveRight = false;
        }
        if (direction == 2)
        {
            print("cannotmovedown");
            canMoveDown = false;
        }
        if (direction == 3)
        {
            print("cannotmoveleft");
            canMoveLeft = false;
        }
    }

    void ClearWallBools()
    {
        canMoveUp = true;
        canMoveDown = true;
        canMoveLeft = true;
        canMoveRight = true;
    }

    void DeployColliders()
    {
        print("deployed");
        colliders[0].transform.position = new Vector2(colliders[0].transform.position.x, colliders[0].transform.position.y + 0.5f);
        colliders[1].transform.position = new Vector2(colliders[1].transform.position.x + 0.5f, colliders[1].transform.position.y);
        colliders[2].transform.position = new Vector2(colliders[2].transform.position.x, colliders[2].transform.position.y - 0.5f);
        colliders[3].transform.position = new Vector2(colliders[3].transform.position.x - 0.5f, colliders[3].transform.position.y);

        Invoke("RetractColliders", 0.1f);
 
    }
    
    void RetractColliders()
    {
        print("retracted");
        colliders[0].transform.position = new Vector2(colliders[0].transform.position.x, colliders[0].transform.position.y - 0.5f);
        colliders[1].transform.position = new Vector2(colliders[1].transform.position.x - 0.5f, colliders[1].transform.position.y);
        colliders[2].transform.position = new Vector2(colliders[2].transform.position.x, colliders[2].transform.position.y + 0.5f);
        colliders[3].transform.position = new Vector2(colliders[3].transform.position.x + 0.5f, colliders[3].transform.position.y);
    }

    void DeployLine()
    {
        print("paint");
        groundTile.SetTile(newTilePosition, Tile);
  
    }

}
