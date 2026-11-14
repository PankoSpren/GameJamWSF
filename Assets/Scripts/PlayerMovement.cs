using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    bool performedMove;
    Vector2 currentPossition = new Vector2 (1.5f, -0.5f);
    bool canMoveUp;
    bool canMoveDown;
    bool canMoveLeft;
    bool canMoveRight;

    public GameObject[] colliders;

    void Start()
    {
        ClearWallBools();
       // DeployColliders();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) == true)
        { 
            DeployColliders();
        }

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

            print("MoveUp");
            
            transform.position = new Vector3(currentPossition.x, currentPossition.y + 1);
 
        }
        else if (direction == false)
        {
            if (canMoveDown == false)
            {
                print("MoveNo");
                return false;
            }

            print("Movedown");
            transform.position = new Vector3(currentPossition.x, currentPossition.y - 1);
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

            print("Moveleft");
            transform.position = new Vector3(currentPossition.x - 1, currentPossition.y);
        }
        else if (direction == false)
        {
            if (canMoveRight == false)
            {
                print("MoveNo");
                return false;
            }

            print("Moveright");
            transform.position = new Vector3(currentPossition.x + 1, currentPossition.y);
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




}
