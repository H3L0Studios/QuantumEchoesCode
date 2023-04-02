//PURPOSE: Manages the battery meter and related events that effect it.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatteryPower : MonoBehaviour
{
    [SerializeField] Image batteryUI; //UI image is used to show how much battery is left by subtracting a vertical fill amount of this image.
    [SerializeField] float drainTime = 180f; //Hows fast it drains
    [SerializeField] float power; //current power left in battery meter
    void Update()
    {
        
        if (SaveScript.batteryRefill == true) //checks if we used a new battery
        {
            SaveScript.batteryRefill = false; //if so, set us as not having used a battery
            batteryUI.fillAmount = 1.0f; //and then fill battery to full.
        }        
        if (SaveScript.flashLightOn == true || SaveScript.nvLightOn == true) //check if either the flashlight or night vision are on, if so...
        {
            batteryUI.fillAmount -= 1.0f / drainTime * Time.deltaTime; //drain the battery image fill amount
            power = batteryUI.fillAmount; //make the power value match the fillamount
            SaveScript.batteryPower = power; //Record it to our savescript for saving/tracking etc
        }
    }
}
