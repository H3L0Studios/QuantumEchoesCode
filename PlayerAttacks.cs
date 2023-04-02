//PURPOSE: Manages player attack animations and HUD items
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerAttacks : MonoBehaviour
{
    private Animator anim; //player arms animator
    public float stamina; //stamina amount
    public float maxStamina; //max stamina
    [SerializeField] GameObject fpcCont; //player character controller
    [SerializeField] float attackStaminaReduction = 2; //how much stamina reduces for attacking...default is 20%
    [SerializeField] GameObject crossHair; //the crosshair HUD object
    [SerializeField] AudioClip gunShotSound; //gun shot sound
    [SerializeField] AudioClip gunNoBullets; //no bullets sound
    [SerializeField] AudioClip crossbowSound; //xbow fire sound
    [SerializeField] AudioClip splatSound; //splat sound when we hit something
    private AudioSource audioPlayer; //where sounds play from
   
    void Start()
    {
        anim = GetComponent<Animator>();
        crossHair.SetActive(false);
        audioPlayer = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (SaveScript.inInventory == false || SaveScript.inOptions == false) //make sure we arent in inventory or options screens...
        {
            if (SaveScript.stamina > 3) //make sure we have atleast 30% stamina
            {
                if (SaveScript.knifeEquiped == true && SaveScript.inInventory == false && SaveScript.inOptions == false) //handle knife attack
                {
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        anim.SetTrigger("KnifeLMB");
                        SaveScript.stamina -= attackStaminaReduction;
                    }
                    if (Input.GetKeyDown(KeyCode.Mouse1))
                    {
                        anim.SetTrigger("KnifeRMB");
                        SaveScript.stamina -= attackStaminaReduction;
                    }
                }
                if (SaveScript.batEquiped == true && SaveScript.inInventory == false && SaveScript.inOptions == false) //hanlde bat attack
                {
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        anim.SetTrigger("BatLMB");
                        SaveScript.stamina -= attackStaminaReduction;
                    }
                    if (Input.GetKeyDown(KeyCode.Mouse1))
                    {
                        anim.SetTrigger("BatRMB");
                        SaveScript.stamina -= attackStaminaReduction;
                    }
                }
                if (SaveScript.axeEquiped == true && SaveScript.inInventory == false && SaveScript.inOptions == false) //handle axe attack
                {
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        anim.SetTrigger("AxeLMB");
                        SaveScript.stamina -= attackStaminaReduction;
                    }
                    if (Input.GetKeyDown(KeyCode.Mouse1))
                    {
                        anim.SetTrigger("AxeRMB");
                        SaveScript.stamina -= attackStaminaReduction;
                    }
                }
                if (SaveScript.gunEquiped == true && SaveScript.inInventory == false && SaveScript.inOptions == false) //handle gun attack
                {
                    if (Input.GetKey(KeyCode.Mouse1)) //make sure we aim first before firing
                    {
                        anim.SetBool("Aiming",true);
                        crossHair.SetActive(true); //show crosshair
                        if (Input.GetKeyDown(KeyCode.Mouse0)) //trigger is pulled
                        {
                            if (SaveScript.bulletCount > 0) //make sure we have ammo
                            {
                                audioPlayer.clip = gunShotSound;
                                audioPlayer.Play();
                            }
                            if (SaveScript.bulletCount <= 0) //if we have no ammo play the no ammo click sound
                            {
                                audioPlayer.clip = gunNoBullets;
                                audioPlayer.Play();
                            }
                        }
                    }
                    if (Input.GetKeyUp(KeyCode.Mouse1)) //if we let up the aim...
                    {
                        anim.SetBool("Aiming", false);
                        crossHair.SetActive(false);
                    }  
                }
                if (SaveScript.crossbowEquiped == true && SaveScript.inInventory == false && SaveScript.inOptions == false) //handle xbow attack
                {
                    if (Input.GetKey(KeyCode.Mouse1)) //make sure we aim first before firing
                    {
                        anim.SetBool("Aiming", true);
                        crossHair.SetActive(true); //show crosshair
                        if (Input.GetKeyDown(KeyCode.Mouse0))
                        {
                            if (SaveScript.boltCount > 0)
                            {
                                audioPlayer.clip = crossbowSound;
                                audioPlayer.Play();
                            }
                        } //no click sound here..
                    }
                    if (Input.GetKeyUp(KeyCode.Mouse1))
                    {
                        anim.SetBool("Aiming", false);
                        crossHair.SetActive(false);
                    }
                }
            }
        }
    }
}
