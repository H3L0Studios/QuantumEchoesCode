//PURPOSE: manages UI events for the gun
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunUIScript : MonoBehaviour
{
    [SerializeField] Text bulletText;
    void Start()
    {
        bulletText.text = SaveScript.bulletCount + ""; //initialize bullet count
    }

    void Update()
    { //make sure we can fire the gun. Did we aim, pull trigger, we arent in the inventory or options at the moment.
        if (Input.GetKeyDown(KeyCode.Mouse0) && Input.GetKey(KeyCode.Mouse1) && SaveScript.inInventory == false && SaveScript.inOptions == false) 
        {
            if (SaveScript.bulletCount > 0) //if we have bullets...
            {
                SaveScript.bulletCount -= 1; //subract a bullet
                bulletText.text = SaveScript.bulletCount + ""; //update ammo amount for HUD
            }
        }
    }
}
