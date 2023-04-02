//PURPOSE:Used to handle light events such as flashlight and night vision via postprocessing. Should be attached to the camera.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class LightSettingsLayer : MonoBehaviour
{
//NightVision Variables
    [SerializeField] PostProcessVolume myVolume;
    [SerializeField] PostProcessProfile standardProfile;
    [SerializeField] PostProcessProfile nightProfile;
    [SerializeField] GameObject nightVisionOverlay;
    private bool nightvisionmode = false;
//Flashlight Variables
    [SerializeField] GameObject flashLightOverlay;
    [SerializeField] GameObject enemyFlashLightOverlay;
    private bool flashlightMode = false;

    private void Start()
    {
        //Initialize Night Vision as off
        nightVisionOverlay.SetActive(false);
        myVolume.profile = standardProfile;
        
        //Initialize Flashlight as off
        flashlightMode = false;
        flashLightOverlay.SetActive(false);
        enemyFlashLightOverlay.SetActive(false);
    }
    
    void Update()
    {
        if (SaveScript.batteryPower > 0.0f) // makes sure battery power is above 0 before worrying about night vision or flashlight keys
        {
            //NightVision Logic
            if (Input.GetKeyDown(KeyCode.N)) //need to add logic to make sure we have nightvision goggles first
            {
                if (nightvisionmode == false) //see if nighvision is off
                {
                    myVolume.profile = nightProfile; //set nightvision profile
                    nightvisionmode = true; //used to track locally that nightvision is on
                    nightVisionOverlay.SetActive(true); //enable the nightvision overlay
                    SaveScript.nvLightOn = true; //track globally nightvision is on
                }
                else //otherwise nightvision is already on and we need to turn it off
                {
                    myVolume.profile = standardProfile; //switch back to normal vision profile
                    nightvisionmode = false; //used to track locally that nightvision is off
                    nightVisionOverlay.SetActive(false);//disable the nightvision overlay
                    SaveScript.nvLightOn = false;//track globally nightvision is off
                }
            }

            //Flashlight Logic  
            if (Input.GetKeyDown(KeyCode.F) && SaveScript.hasFlashlight==true) //makes sure we have the flashlight first
            {
                if (flashlightMode == false) //if the flashlight is off
                {
                    flashlightMode = true; //track locally that its on 
                    flashLightOverlay.SetActive(true);//enable flashlight overlay
                    enemyFlashLightOverlay.SetActive(true);//enables enemy overly which culls light on enemy objects
                    SaveScript.flashLightOn = true;//track globally that its on
                }
                else //otherwise flashlight is on so we need to turn it off
                {
                    flashlightMode = false; //track locally that its off 
                    flashLightOverlay.SetActive(false);//disable the flashlight overlay
                    enemyFlashLightOverlay.SetActive(false);//disable enemy light culling
                    SaveScript.flashLightOn = false;//track globally that its off 
                }
            }
        } else//this means we've run out of battery and need to disable everything that is on that consumes battery
        {
            myVolume.profile = standardProfile;//switch to standard light profile
            //switch off nightvision
            nightvisionmode = false;
            nightVisionOverlay.SetActive(false);
            SaveScript.nvLightOn = false;
            //switch off flashlight
            flashlightMode = false;
            flashLightOverlay.SetActive(false);
            enemyFlashLightOverlay.SetActive(false);
            SaveScript.flashLightOn = false;
        }
    }
}
