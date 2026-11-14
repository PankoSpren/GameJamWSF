using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDLogic : MonoBehaviour
{

    public Slider slider;


    public void SetMaxSpool(int maxSpool)
    {
        slider.maxValue = maxSpool;
        slider.value = maxSpool;
    }
    // Start is called before the first frame update
    public void setSpool(int spool)
    {
        slider.value = spool;
    }

}
