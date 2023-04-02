//PURPOSE: Used to manage door related events
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    private AudioSource myPlayer;
    private Animator anim;
    public bool isOpen = false;
    public bool doorLocked;
    public string doorType;
    public AudioClip lockedClip;
    [SerializeField] AudioClip openSound;
    [SerializeField] AudioClip closeSound;
    [SerializeField] float doorCloseTime = 0.6f;

    //Below are to designate what door this is
    [SerializeField] bool cabinDoor;
    [SerializeField] bool roomDoor;
    [SerializeField] bool houseDoor;
    [SerializeField] bool l1_RustedGate;
    [SerializeField] bool l1_ServiceGate;
    [SerializeField] bool OpenOnceOnly; // for gates etc that wouldnt be closable once opened.

    void Start()
    {
        myPlayer = GetComponent<AudioSource>(); //get our audiosource 
        anim = GetComponent<Animator>(); //get door animator
        if (isOpen == true) //if the door is marked to be open on start...
        {
            anim.SetTrigger("Open"); //animate it open
            GetComponent<AudioSource>().clip = openSound; //play open sound
        }
    }

    void Update()
    {  //here we cycle through the door types to see if we treat the as locked or unlocked based on if we have keys or not.
        if (cabinDoor == true) //if this door is the "cabin door"...This was originally for our test level.
        {
            doorType = "cabin"; //set that type
            if (SaveScript.hasCabinKey == true) //and if we have the cabin key that unlocks it....
            {
                doorLocked = false; //The door can be opened normally.
            }
        }
        if (roomDoor == true) //same as above but for "locked room" door. This was originally for our test level.
        {
            doorType = "room";
            if (SaveScript.hasRoomKey == true)
            {
                doorLocked = false;
            }
        }
        if (houseDoor == true) //same as above but for "locked room" door. This was originally for our test level.
        {
            doorType = "house";
            if (SaveScript.hasHouseKey == true)
            {
                doorLocked = false;
            }
        }
        if (l1_RustedGate == true) //same as above but for "gate that needs to be pried open" door.
        {
            doorType = "L1_rustedgate";
            if (SaveScript.hasMetalPipe == true)
            {
                doorLocked = false;
            }
            if (isOpen == true)
            {
                SaveScript.hasMetalPipe = false; //In this case we remove the key once open since it cant be closed.
            }
        }
        if (l1_ServiceGate == true) //same as above but for the service road gate
        {
            doorType = "L1_servicegate";
            if (SaveScript.hasL1GateKey == true)
            {
                doorLocked = false;
            }
            if (isOpen == true)
            {
                SaveScript.hasL1GateKey = false; //In this case we remove the key once open since it cant be closed. may need to reverse this if we allow backtracking after loading the game.
            }
        }
    }

    public void doorOpen() //function to cause a door to open
    {
        if (isOpen == false)
        {
            anim.SetTrigger("Open");
            playSound();
            isOpen = true;  
            
        }
        else if (isOpen == true && OpenOnceOnly == false) //allows us to close the door, if its not marked to be permanently open once opened
        {
            anim.SetTrigger("Close");
            playSound();
            isOpen = false;
        }
    }

    private void playSound() //called to play the sound when a door is open or closed.
    {
        if (isOpen == false)
        {
            myPlayer.clip = openSound;
            myPlayer.Play();
        }
        if (isOpen == true)
        {
            StartCoroutine(WaitForDoorClose()); //wait a second or so before playing the close sound. Probably should move this to an animation trigger.
        }
    }
    private void OnTriggerEnter(Collider other) //This is used to let enemies open unlocked door
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (doorLocked == false)
            {
                if (isOpen == false)
                {
                    anim.SetTrigger("Open");
                    isOpen = true;
                    playSound();
                }
            }
        }
    }
    IEnumerator WaitForDoorClose() //waits to play the door closed sound...need to move this anmation trigger
    {
        yield return new WaitForSeconds(doorCloseTime);
        myPlayer.clip = closeSound;
        myPlayer.Play();
    }
}
