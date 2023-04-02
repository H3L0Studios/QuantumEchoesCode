//PURPOSE: manages UI events for the crossbox
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrossbowUIScript : MonoBehaviour
{
    [SerializeField] Text crossbowText;
    void Start()
    {
        crossbowText.text = SaveScript.boltCount + "";
    }

    void Update()
    { //make sure we aimed, pulled the trigger, and are not in inventory or options screens.
        if (Input.GetKeyDown(KeyCode.Mouse0) && Input.GetKey(KeyCode.Mouse1) && SaveScript.inInventory == false && SaveScript.inOptions == false)
        {
            if (SaveScript.boltCount > 0) //if we have bolts...
            {
                SaveScript.boltCount -= 1; //subtract 1
                crossbowText.text = SaveScript.boltCount + ""; //update HUD text
            }
        }
    }
}
