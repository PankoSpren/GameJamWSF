using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpoolController : MonoBehaviour
{
    // Update is called once per frame

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Kolizja z  " + collision.gameObject.name);
            Destroy(gameObject);
            if (gameObject.tag == "Negative")
            {
             //   changeSpool(-10);
                Destroy(gameObject);
            }
            else if (gameObject.tag == "Additive")
            {
              //  changeSpool(10);
                Destroy(gameObject);
            }
        }
    }
}
