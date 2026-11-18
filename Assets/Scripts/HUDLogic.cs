using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDLogic : MonoBehaviour
{

    public Slider spoolSlider;
    public TMP_Text count;
    public TMP_Text victoryWord;
    public TMP_Text victoryDescription;

    private void Start()
    {
    }

    public void SetMaxSpool(int maxSpool)
    {
        spoolSlider.maxValue = maxSpool;
       // spoolSlider.value = maxSpool;
    }
    // Start is called before the first frame update
    public void changeSpool(int spool)
    {
 
        spoolSlider.value = spoolSlider.value + spool;
        count.text = spoolSlider.value.ToString();
    }
    public void SpawnVictory()
    {
        victoryWord.text = "Victory!!!";
        Invoke("SpawnVictoryDescription", 2f);

    }

    void SpawnVictoryDescription()
    {
        victoryDescription.text = "But now you must retern to begin your journey anew";
    }


    
}

