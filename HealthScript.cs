//PURPOSE: manages the player health in HUD, and handles situations where we die in HUD.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{
    [SerializeField] Text healthText; //HUD Health text
    [SerializeField] GameObject deathPanel; //Death panel to hsow when we die.

    void Start()
    {
        healthText.text = SaveScript.playerHealth+"%"; //set the player health at start
        deathPanel.SetActive(false); //death panel is off
    }

    // Update is called once per frame
    void Update()
    {
        if (SaveScript.healthChanged == true) //when health is marked as changed...
        {
            SaveScript.healthChanged = false; //turn that marker off
            healthText.text = SaveScript.playerHealth + "%"; //update the HUD health amount
        }
        if (SaveScript.playerHealth <= 0) //if we hit 0 health...
        {
            SaveScript.playerHealth = 0; //dont let it go below 0
            deathPanel.SetActive(true); //turnb on the death panel tell player that they died.
        }
    }

}
