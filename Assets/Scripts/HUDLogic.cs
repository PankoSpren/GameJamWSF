using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDLogic : MonoBehaviour
{

    public Slider spoolSlider;

    public void SetMaxSpool(int maxSpool)
    {
        spoolSlider.maxValue = maxSpool;
        spoolSlider.value = maxSpool;
    }
    // Start is called before the first frame update
    public void changeSpool(int spool)
    {
        spoolSlider.value = spoolSlider.value + spool;
    }
}

