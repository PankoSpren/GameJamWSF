using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpoolController : MonoBehaviour
{
    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player collided with tile");
            //Destroy(other.gameObject);
        }
    }
}
