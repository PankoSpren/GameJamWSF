using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CheckTile : MonoBehaviour
{
    public PlayerMovement playerMovement;
    [SerializeField] int direction;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        print("test");
        if (collision.gameObject.CompareTag("Wall") == true)
        {
            print("detectedwall");
            playerMovement.CheckWall(direction);
        }
        else
        {
            print("no wall");
            return;
        }
        print("test");
    }
}
