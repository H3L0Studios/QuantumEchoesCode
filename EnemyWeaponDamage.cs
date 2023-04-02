//PURPOSE: Use to manage what happens when the player is hit by the enemy. Attached to enemy weapon object
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponDamage : MonoBehaviour
{
    [SerializeField] int weaponDamage=1;
    private Animator hurtAnim;
    private AudioSource stabSound;
    private GameObject fpsArms;
    [SerializeField] EnemyDamage enemyDmg;
    public bool hitActive = false;

    private void Start()
    {
        fpsArms = NavPointLocations.fpsArms;
        stabSound = NavPointLocations.stabSound;
        hurtAnim = NavPointLocations.hurtAnim;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && enemyDmg.hasDied == false) //if the enemy weapon hits the player, and the enemy isnt dead...
        {
            if (hitActive == false) //make sure the the enemy hasnt already damaged us during this attack.
            {
                hitActive = true; //we are being hit...
                hurtAnim.SetTrigger("Hurt"); //Do the player hurt aninmation
                SaveScript.playerHealth -= weaponDamage; //subtract health from player
                SaveScript.healthChanged = true; //updates HUD value
                stabSound.Play(); //play sound of being hit
                fpsArms.GetComponent<PlayerAttacks>().stamina -= 0.5f ; //make us lose 5% stamina
            }
        }
    }
    private void OnTriggerExit(Collider other) //when collision is over...
    {
        if (other.gameObject.CompareTag("Player")) //if interacted with player,
        {
            if (hitActive == true) //if we got hit..
            {
                hitActive = false; //we can now get hit again since the sinle attack is over.
                this.gameObject.SetActive(false); //disable the attack trigger collider until next swing.
            }
        }
    }
}
