//PURPOSE: Used to manage the stamina guage.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class StaminaGague : MonoBehaviour
{
    [SerializeField] GameObject fpcCont; //get the player object
    [SerializeField] Image staminaBar; //stamina bar image
    [SerializeField] int decayRate = 1; //decay rate of stamina when running
    [SerializeField] float refillRate = 0.25f; //refill rate when not running
    [SerializeField] GameObject lightBreathing; //The light breathing sound
    [SerializeField] GameObject heavyBreathing; //The heavy breathing sound
    private bool lightBreath; //Tracks if we are light breathing
    private bool heavyBreath; //track if we are heavy breathing
    private bool outOfBreath = false; //tracks if we ran out of stamina forcing a break
    private float runSpeedBakup; //tracks what our set walk speed is normally
    private float walkSpeedBakup; //tracks what our set run speed is normally

    void Start()
    {
        lightBreathing.gameObject.SetActive(false); //start with full stamina so no breathing sounds...
        heavyBreathing.gameObject.SetActive(false); //start with full stamina so no breathing sounds...
        walkSpeedBakup = fpcCont.GetComponent<FirstPersonController>().WalkSpeedGetter(); //get our normal walk speed from the first person character script
        runSpeedBakup = fpcCont.GetComponent<FirstPersonController>().RunSpeedGetter(); //get our normal run speed from the first person character script
    }

    void Update()
    {
        staminaBar.fillAmount = SaveScript.stamina/SaveScript.maxStamina; //set the stamina guage amount in the HUD based on how much stamina is missing
        if (lightBreath == false) //check if we are not light breathing
        {
            if (SaveScript.stamina < 3) //if not light breathing & stamina is less than 30%
            {
                lightBreathing.gameObject.SetActive(true); //We are now light breathing
                heavyBreathing.gameObject.SetActive(false); //Make sure we are not heavy breathing
                lightBreath = true; //Track the breathing state
            }
        }
        if (lightBreath == true) //If we have been light breathing
        {
            if (SaveScript.stamina >= 3) //if our stamina is now above 30%...
            {
                lightBreathing.gameObject.SetActive(false);//disable light breathing
                heavyBreathing.gameObject.SetActive(false);//disable heavy breathing
                lightBreath = false; //track breathing
                fpcCont.GetComponent<FirstPersonController>().RunSpeedSetter(runSpeedBakup); //make sure we can run at normal pace again.we disable it below if player gets too low in stamina
            }
        }
        if (heavyBreath == false) //if not heavy breathing...
        {
            if (SaveScript.stamina < 1.0) //and our stamina fell below 10%
            {
                lightBreathing.gameObject.SetActive(false);//turn off light breathing
                heavyBreathing.gameObject.SetActive(true); //enable heavy breathing
                heavyBreath = true; //track heavy breathing
                fpcCont.GetComponent<FirstPersonController>().RunSpeedSetter(walkSpeedBakup); //Our run speed is now restricted to our walk speed...we need a break from holding run button
            }
        }
        if (heavyBreath == true || outOfBreath == true) //if heavy breathing or out of breath...
        {
            if (SaveScript.stamina >= 3) //and if our stamina has recovered above 30%...
            {
                lightBreathing.gameObject.SetActive(false); //turn off light breathing
                heavyBreathing.gameObject.SetActive(false); //turn off heavy breathing
                heavyBreath = false; //tracker updates
                outOfBreath = false; //tracker updates
                fpcCont.GetComponent<FirstPersonController>().RunSpeedSetter(runSpeedBakup); //run speed is normal again
            }
        }
        if (Input.GetKey(KeyCode.LeftShift)) //if we hold the shift key
        {
            if (SaveScript.stamina > 0.1 && outOfBreath==false) //and we still have stamina and are not out of breath
            {
                SaveScript.stamina = SaveScript.stamina - decayRate * Time.deltaTime; //drain stamina
            }
        }
        if (!Input.GetKey(KeyCode.LeftShift)) //if we are not holding down the shift key...
        {
            if (SaveScript.stamina < SaveScript.maxStamina) //and if our stamina is below max stamina...
            {
                SaveScript.stamina = SaveScript.stamina + refillRate * Time.deltaTime; //start recovering stamina
            }
        }
        if (SaveScript.stamina < 0.1f) //if we've lost all our stamina...
        {
            SaveScript.stamina = 0.1f; //stop it from hitting 0 so we don't get division by 0 errors, just keep it at 0.1
            outOfBreath = true; //mark us as out of breath...no more running for us until recovered to 30%
        }
        if (outOfBreath == true) //if we are out of breath
        {
            if(SaveScript.stamina >= 3.0f) //and we now have over 30% tamina
            {
                outOfBreath = false; //we are not longer out of breath
            }
        }
    }
}
//DEBUG BOX
//Debug.Log(player.attackStamina + "  -  " + player.maxAttackStamina);
//Debug.Log("Setting Run Speed");
