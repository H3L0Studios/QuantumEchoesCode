//PURPOSE: Used to manage what happens when an enemy is attacked and takes damage
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] public int enemyHealth = 100; //initialize enemy with full health
    private AudioSource hurtSound; //sound to play when this enemy gets hurt
    public bool hasDied = false; //tracks if enemy has died or not
    private Animator anim; //get the animator for the enemy
    private AudioSource stabAudio; //audio to play when hit by a melee weapon
    [SerializeField] bool blazeAI; //If the enemy is wires for blaze AI. Currently not used.
    private GameObject bloodSplatKnife; //Blood splatter object to play when hit.
    private GameObject bloodSplatBat; //Blood splatter object to play when hit.
    private GameObject bloodSplatAxe; //Blood splatter object to play when hit.

    void Start()
    {
        hurtSound = GetComponent<AudioSource>(); //get audio source hurt sound is played on
        anim = GetComponentInParent<Animator>(); //get enemy animator
       
        StartCoroutine(startElements()); //initialize things...like make sure splatters arent running.
    }

    void Update()
    {
        if (enemyHealth <= 0) //check if the enemy has run out of health...
        {
            if (hasDied == false) //if not already marked as dead...
            {
                SaveScript.isBeingAttacked = false; //makes it so the player can;t be attacked by this enemy anymore
                anim.SetTrigger("Death"); //do the death animation
                hasDied = true; //mark it as dead
                anim.SetBool("isDead", true); //used to stop any other animations
                if (blazeAI == true) //if we are using blazeAI
                {
                    GetComponentInParent<BlazeAI>().Death(); // do its death sequence instead
                }
                Destroy(this.transform.parent.gameObject, 30f); //after 30 seconds, destroy the enemy from the world
                SaveScript.enemiesOnScreen--; //used to track how many enemies are on screen...this is used when spawners spawn enemies periodically. if too many...it stops the spawns
            }
        }
    }

    private void OnTriggerEnter(Collider other) //when attacked, did we collide with any of these player weapons???
    {
        if (other.gameObject.CompareTag("P_Knife")) //the knife
        {
            enemyHealth -= 10; //enemy loses 10 health
            hurtSound.Play(); //play hurt sound
            stabAudio.Play(); //play melee stab sound
            bloodSplatKnife.gameObject.SetActive(true); //show blood splatter
        }
        if (other.gameObject.CompareTag("P_Bat")) //the bat
        {
            enemyHealth -= 15; //enemy loses 15 health
            hurtSound.Play();
            stabAudio.Play();
            bloodSplatBat.gameObject.SetActive(true);
        }
        if (other.gameObject.CompareTag("P_Axe")) //the axe
        {
            enemyHealth -= 25; //enemy loses 15 health
            hurtSound.Play();
            stabAudio.Play();
            bloodSplatAxe.gameObject.SetActive(true);
        }
        if (other.gameObject.CompareTag("P_Crossbow")) //crossbow bolt
        {
            enemyHealth -= 60; //enemy loses 60 health
            hurtSound.Play();
            stabAudio.Play();
            Destroy(other.gameObject, 0.05f);
        }
        //Gun is handled in its own script because it doesnt use object colliders, it uses rays
    }

    IEnumerator startElements() //initializes everything when enemy is activated in the world.
    {
        yield return new WaitForSeconds(0.1f);
        bloodSplatAxe = NavPointLocations.axeBloodSpray;
        bloodSplatKnife = NavPointLocations.knifeBloodSpray;
        bloodSplatBat = NavPointLocations.batBloodSpray;
        bloodSplatKnife.gameObject.SetActive(false);
        bloodSplatBat.gameObject.SetActive(false);
        bloodSplatAxe.gameObject.SetActive(false);
        stabAudio = NavPointLocations.stabSound;
    }
}
//DEBUG BOX
//Debug.Log("Hit Enemy! Health is: " + enemyHealth);
