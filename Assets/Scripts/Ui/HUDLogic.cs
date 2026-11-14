using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDLogic : MonoBehaviour
{

    public Slider slider;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) {changeSpool(-10);}
        else if (Input.GetKeyDown(KeyCode.S)) {changeSpool(10);}
    }

    public void SetMaxSpool(int maxSpool)
    {
        slider.maxValue = maxSpool;
        slider.value = maxSpool;
    }
    // Start is called before the first frame update
    public void changeSpool(int spool)
    {
        slider.value = slider.value +spool;
    }


}
