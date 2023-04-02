//PURPOSE: Handles player interactions with objects. There is room to convert some of this to arrays for shorting code long term. For now we handle it the long way for readability.
//This script should reside on the player camera as it shoots rays from it to detect things we can interact with.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pickups : MonoBehaviour
{
    RaycastHit hit;
    [SerializeField] float pickupDistance; //How far can we interact with something from
    [SerializeField] float doorDistance; //How far can we interact withg a door from. Do we need this?*************************
    [SerializeField] GameObject pickupMessage; //The UI Text to show when you can pick something up
    [SerializeField] GameObject fullMessage; //Displays that you cannot carry anymore of an item if you have to many of it
    [SerializeField] GameObject doorMessage; //message to show when interacting with doors
    [SerializeField] GameObject playerArms; //Captures the player Arms object so we can display it when using a weapon
    [SerializeField] GameObject bulletText; //for displaying how many bullets we have in the HUD
    [SerializeField] GameObject boltText; //for displaying how many crossbow bolts we have in the HUD
    [SerializeField] AudioClip doorLockedSound; //sound to play if we try to open a locked door
    [SerializeField] AudioClip pickupSound; //sound to play when picking something up
    [SerializeField] AudioClip noteSound; //sound to play when opening a note
    [SerializeField] int maxBullets = 128; //max bullets count. used to show if we are full or not when interacting.
    [SerializeField] int maxBolts = 32; //max bullets count. used to show if we are full or not when interacting.

    private float rayDistance; //used for casting a ray this distance at interactable objects.
    private bool canSeeObject = false; //used to track if ray hit an object we can interact with or not
    private string fullType; //used for tracking what type of object we are full of. leveraged for dynamic population of the full message
    private bool applesFull = false; //marks if we are full of apples.
    private bool batteriesFull = false; //marks if we are full of batteries.
    private bool bulletsFull = false; //marks if we are full of bullets.
    private bool boltsFull = false; //marks if we are full of bolts.
    private AudioSource soundPlayer; //where sounds are played from when interacting with things.
    
    void Start()
    {
        pickupMessage.gameObject.SetActive(false); //disables pickup message at start
        fullMessage.gameObject.SetActive(false); //disables full message at start
        doorMessage.gameObject.SetActive(false); //disables door message at start
        playerArms.gameObject.SetActive(false); //disables player arms at start. ie No weapon is drawn
        rayDistance = pickupDistance; //set the ray distance to our set pickup distance. This is changed later which is why we have this.
        soundPlayer = GetComponent<AudioSource>(); //get the audiosource attached to the player camera where this script lives.
    }

    void Update()
    {
        //check if raycast hitting collectibles
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, rayDistance)) //Shoot a ray at a distance of raydistance. If it hits something...
        {
            if (hit.distance >=pickupDistance) //if we moved beyond the pickupdistance specified...
            {
                hideMessage(); //hide the pickup message
            }
            hitHandler(hit); //launch hithandler which checks if anything the ray hit is something we can interact with.
        }
        // Check of object can be seen and process it depending on what it is and where you are from it.
        if (canSeeObject == true) // Is the object viewable still?
        {
            checkIfFull(); //check if we are full of that object
        }
        if (canSeeObject == false) //If the object is not viewable anymore...this is to stop an anomaly where the message kept displaying when not looking at something anymore.
        {
            hideMessage(); //hide the message
        }
    }
    private void hideMessage() //hides the interact messages because we arent looking at it or too far away
    {
        fullMessage.gameObject.SetActive(false); //disable full message
        pickupMessage.gameObject.SetActive(false); //disable pickup message
        doorMessage.gameObject.SetActive(false); //disable door message
        rayDistance = pickupDistance; //reset the ray distance
        fullType = "none"; // reset the full type
        canSeeObject = false; // set that we can no longer see an interactable object
    }

    private void hitHandler(RaycastHit hit) //checks for if anything the ray hit is tagged as an interactable object -- this part could probably be consolidated into arrays by type of interaction
    {
        if (hit.transform.CompareTag("1Apple")) //is it an apple?
        {
            if (SaveScript.appleCount >= 6) //make sure we don't have more than 6. these are tracked in savescript
            {
                applesFull = true; //set applesfull to true
                fullType = hit.transform.tag; //use the tag to populate the message of what we dont have room for.
            }
            canSeeObject = true; //we mark that we can see it which results in the message to pick it up showing
            if (Input.GetKeyDown(KeyCode.E) && SaveScript.appleCount < 6) //If we have less than the max amount and press E...
            {
                hit.transform.gameObject.SetActive(false); //disable the object
                SaveScript.appleCount += 1; //mark that we have 1 more apple in savescript tracker
                soundPlayer.Play(); //play the take object sound.
            }
        }
        else if (hit.transform.CompareTag("Shell")) //Is the object a shell? if so and player hits E, then disable it and make it reflect in savescript
        {
            canSeeObject = true;
            if (Input.GetKeyDown(KeyCode.E))
            {
                hit.transform.gameObject.SetActive(false);
                SaveScript.shellCount += 1;
                soundPlayer.Play();
            }
        }
        else if (hit.transform.CompareTag("Battery")) //Is it a battery...
        {
            if (SaveScript.batteryCount >= 4)
            {
                batteriesFull = true;
                fullType = hit.transform.tag;
            }
            canSeeObject = true;
            if (Input.GetKeyDown(KeyCode.E) && SaveScript.batteryCount < 4)
            {
                hit.transform.gameObject.SetActive(false);
                SaveScript.batteryCount += 1;
                soundPlayer.clip = pickupSound;
                soundPlayer.Play();
            }
        }
        else if (hit.transform.CompareTag("Backpack")) //Are we looking at the backpack?
        {
            canSeeObject = true;
            if (Input.GetKeyDown(KeyCode.E) && SaveScript.hasBackpack == false) //make sure we already don't have the backpack - shouldnt happen unless dev puts two in the game because player loses it
            {
                hit.transform.gameObject.SetActive(false);
                SaveScript.hasBackpack = true;
                soundPlayer.Play();
            }
        }
        else if (hit.transform.CompareTag("WaterBottle")) //Are we looking at a water bottle?
        {
            canSeeObject = true;
            if (Input.GetKeyDown(KeyCode.E) && SaveScript.waterBottleCount <= 4)
            {
                hit.transform.gameObject.SetActive(false);
                SaveScript.waterBottleCount += 1;
                soundPlayer.Play();
            }
        }
        else if (hit.transform.CompareTag("CarKeys")) //Are we looking at Car keys
        {

            canSeeObject = true;
            if (Input.GetKeyDown(KeyCode.E) && SaveScript.hasCarKeys == false) //make sure we already don't have the car keys - shouldnt happen unless dev puts two in the game because player loses it
            {
                hit.transform.gameObject.SetActive(false);
                SaveScript.hasCarKeys = true;
                soundPlayer.Play();
            }
        }
        else if (hit.transform.CompareTag("FirstAid")) //Are we looking at a First Aid Kit
        {

            canSeeObject = true;
            if (Input.GetKeyDown(KeyCode.E) && SaveScript.firstAidCount <= 3)
            {
                hit.transform.gameObject.SetActive(false);
                SaveScript.firstAidCount++;
                soundPlayer.Play();
            }
        }
        else if (hit.transform.CompareTag("Flashlight"))//Are we looking at a flashlight
        {

            canSeeObject = true;
            if (Input.GetKeyDown(KeyCode.E) && SaveScript.hasFlashlight == false) //make sure we already don't have it - shouldnt happen unless dev puts two in the game because player loses it
            {
                hit.transform.gameObject.SetActive(false);
                SaveScript.hasFlashlight = true;
                soundPlayer.Play();
            }
        }
        else if (hit.transform.CompareTag("NightVision")) //Are we looking at NV goggles
        {

            canSeeObject = true;
            if (Input.GetKeyDown(KeyCode.E) && SaveScript.hasNightVision == false) //make sure we already don't have it - shouldnt happen unless dev puts two in the game because player loses it
            {
                hit.transform.gameObject.SetActive(false);
                SaveScript.hasNightVision = true;
                soundPlayer.Play();
            }
        }
        else if (hit.transform.CompareTag("Exit")) //Are we looking at something that will exit the stage if we interact?
        {
            canSeeObject = true;
            if (Input.GetKeyDown(KeyCode.E))
            {
                //Not used yet because we found alternate ways to trigger this. Keeping for future use
            }
        }
        else if (hit.transform.CompareTag("Crossbow")) //Did we find a crossbow?
        {
            if (SaveScript.hasCrossbow == true)
            {
                fullType = hit.transform.tag;
            }
            canSeeObject = true;
            if (Input.GetKeyDown(KeyCode.E) && SaveScript.hasCrossbow == false)
            {
                hit.transform.gameObject.SetActive(false);
                SaveScript.hasCrossbow = true;
                soundPlayer.Play();
            }
        }
        else if (hit.transform.CompareTag("Knife")) //Did we find a Knife
        {
            if (SaveScript.hasKnife == true)
            {
                fullType = hit.transform.tag;
            }
            canSeeObject = true;
            if (Input.GetKeyDown(KeyCode.E) && SaveScript.hasKnife == false)
            {
                hit.transform.gameObject.SetActive(false);
                SaveScript.hasKnife = true;
                soundPlayer.Play();
            }
        }
        else if (hit.transform.CompareTag("Bat")) //Did we find a Bat
        {
            if (SaveScript.hasBat == true)
            {
                fullType = hit.transform.tag;
            }
            canSeeObject = true;
            if (Input.GetKeyDown(KeyCode.E) && SaveScript.hasBat == false)
            {
                hit.transform.gameObject.SetActive(false);
                SaveScript.hasBat = true;
                soundPlayer.Play();
            }
        }
        else if (hit.transform.CompareTag("Axe")) //Did we find a Axe
        {
            if (SaveScript.hasAxe == true)
            {
                fullType = hit.transform.tag;
            }
            canSeeObject = true;
            if (Input.GetKeyDown(KeyCode.E) && SaveScript.hasAxe == false)
            {
                hit.transform.gameObject.SetActive(false);
                SaveScript.hasAxe = true;
                soundPlayer.Play();
            }
        }
        else if (hit.transform.CompareTag("Handgun")) //Did we find a Handgun
        {
            if (SaveScript.hasHandgun == true)
            {
                fullType = hit.transform.tag;
            }
            canSeeObject = true;
            if (Input.GetKeyDown(KeyCode.E) && SaveScript.hasHandgun == false)
            {
                hit.transform.gameObject.SetActive(false);
                SaveScript.hasHandgun = true;
                SaveScript.bulletCount += 8; //start with 8 bullets
                if (SaveScript.bulletCount > maxBullets) SaveScript.bulletCount = maxBullets; //If we already have within 8 of max bullets, set max bullets in inventory
                bulletText.GetComponent<Text>().text = SaveScript.bulletCount + ""; //Update UI HUD text
                soundPlayer.Play();
            }
        }
        else if (hit.transform.CompareTag("CabinKey")) //Did we look at the cabin key?
        {
            canSeeObject = true;
            if (Input.GetKeyDown(KeyCode.E) && SaveScript.hasCabinKey == false)
            {
                hit.transform.gameObject.SetActive(false);
                SaveScript.hasCabinKey = true;
                soundPlayer.Play();
            }
        }
        else if (hit.transform.CompareTag("MetalBar")) //Did we look at the metal prybar
        {
            canSeeObject = true;
            if (Input.GetKeyDown(KeyCode.E) && SaveScript.hasMetalPipe == false)
            {
                hit.transform.gameObject.SetActive(false);
                SaveScript.hasMetalPipe = true;
                soundPlayer.Play();
            }
        }
        else if (hit.transform.CompareTag("HouseKey")) //Did we look at the house key -- This is in our test level - may be deleted later.
        {
            canSeeObject = true;
            if (Input.GetKeyDown(KeyCode.E) && SaveScript.hasHouseKey == false)
            {
                hit.transform.gameObject.SetActive(false);
                SaveScript.hasHouseKey = true;
                soundPlayer.Play();
            }
        }
        else if (hit.transform.CompareTag("RoomKey")) //Did we look at the room key -- This is in our test level - may be deleted later.
        {
            canSeeObject = true;
            if (Input.GetKeyDown(KeyCode.E) && SaveScript.hasRoomKey == false)
            {
                hit.transform.gameObject.SetActive(false);
                SaveScript.hasRoomKey = true;
                soundPlayer.Play();
            }
        }
        else if (hit.transform.CompareTag("GateKey")) //Did we look at the gate key
        {
            canSeeObject = true;
            if (Input.GetKeyDown(KeyCode.E) && SaveScript.hasL1GateKey == false)
            {
                hit.transform.gameObject.SetActive(false);
                SaveScript.hasL1GateKey = true;
                soundPlayer.Play();
            }
        }
        else if (hit.transform.CompareTag("Bullets")) //are we looking at ammo?
        {
            if (SaveScript.bulletCount >= maxBullets)
            {
                bulletsFull = true;
                fullType = hit.transform.tag;
            }
            canSeeObject = true;
            if (Input.GetKeyDown(KeyCode.E) && SaveScript.bulletCount < 128)
            {
                hit.transform.gameObject.SetActive(false);
                SaveScript.bulletCount += 8;
                if (SaveScript.bulletCount > maxBullets) SaveScript.bulletCount = maxBullets;
                bulletText.GetComponent<Text>().text = SaveScript.bulletCount + "";
                soundPlayer.Play();
            }
        }
        else if (hit.transform.CompareTag("Bolts")) //Looking at Crossbow Bolts?
        {
            if (SaveScript.boltCount >= maxBolts)
            {
                boltsFull = true;
                fullType = hit.transform.tag;
            }
            canSeeObject = true;
            if (Input.GetKeyDown(KeyCode.E) && SaveScript.boltCount < 128)
            {
                hit.transform.gameObject.SetActive(false);
                SaveScript.boltCount += 4;
                if (SaveScript.boltCount > maxBolts) SaveScript.boltCount = maxBolts;
                boltText.GetComponent<Text>().text = SaveScript.boltCount + "";
                soundPlayer.Play();
            }
        }
        else if (hit.transform.CompareTag("Door") && hit.distance <= doorDistance) //Are we looking at a door
        {
            canSeeObject = true;
            if (Input.GetKeyDown(KeyCode.E) && hit.transform.gameObject.GetComponentInParent<DoorScript>().doorLocked == false) //make sure its not locked
            {
                hit.transform.gameObject.SendMessageUpwards("doorOpen"); //open the door

            }
            else if (Input.GetKeyDown(KeyCode.E) && hit.transform.gameObject.GetComponentInParent<DoorScript>().doorLocked == true) //Check if it's locked
            {
                soundPlayer.clip = hit.transform.gameObject.GetComponentInParent<DoorScript>().lockedClip; //Load the locked sound clip
                soundPlayer.Play(); //Load the locked sound clip

            }
        }
        else if (hit.transform.CompareTag("Note")) //Are we looking at a note object we can read?
        {
            canSeeObject = true;
            if (Input.GetKeyDown(KeyCode.E) && SaveScript.isReading == false) //make sure we arent already reading
            {
                SaveScript.isReading = true; //mark us as reading
                hit.transform.gameObject.GetComponent<InteractableAttributes>().noteToEnable.SetActive(true); //Each not has a reference to the note object to display, this called that and displays it to the player.
                soundPlayer.clip = noteSound; //load note sound
                soundPlayer.Play(); //play it
            }
        }
        else //If it isnt any of the above things...
        {
            canSeeObject = false; //we arent looking at anything interactable so dont show any of the interact prompts
            soundPlayer.clip = pickupSound; //resetting this to default
        }
    }
    private void checkIfFull() //used to see if and stackable items are maxed out
    {
        if (batteriesFull == true && fullType == "Battery")
        {
            fullMessage.gameObject.SetActive(true);
            rayDistance = 1000f; //this forces the ray to reset on something else immediatly after moving focus away causing the display to disappear. There are some minor negative side effects...may need to change
        }
        else if (applesFull == true && fullType == "Apple")
        {
            
            fullMessage.gameObject.SetActive(true);
            rayDistance = 1000f;
        }
        else if (fullType == "Crossbow")
        {
            fullMessage.gameObject.SetActive(true);
            rayDistance = 1000f;
        }
        else if (fullType == "Handgun")
        {
            fullMessage.gameObject.SetActive(true);
            rayDistance = 1000f;
        }
        else if (fullType == "Knife")
        {
            fullMessage.gameObject.SetActive(true);
            rayDistance = 1000f;
        }
        else if (fullType == "Axe")
        {
            fullMessage.gameObject.SetActive(true);
            rayDistance = 1000f;
        }
        else if (fullType == "Bat")
        {
            fullMessage.gameObject.SetActive(true);
            rayDistance = 1000f;
        }
        else if (bulletsFull==true && fullType == "Bullets")
        {
            fullMessage.gameObject.SetActive(true);
            rayDistance = 1000f;
        }
        else if (boltsFull == true && fullType == "Bolts")
        {
            fullMessage.gameObject.SetActive(true);
            rayDistance = 1000f;
        }
        else if (hit.distance >= 4f) //if we moved to far away hide the interact message
        {
            hideMessage();
        }
        else if (hit.transform.CompareTag("Door"))
        {
            doorMessage.gameObject.SetActive(true);
            rayDistance = 1000f;
        }
        else //otherwise we are good to pick this up
        {
            pickupMessage.gameObject.SetActive(true); //show the interact text
            rayDistance = 1000f;
        }
    }

}
//DEBUG BOX
//From Line 47: Debug.DrawRay(transform.position, transform.forward * 10, Color.red);
//Debug.Log("Hitting: " + hit.transform.name + " from distance: " + rayDistance);
//Debug.Log(hit.distance);

