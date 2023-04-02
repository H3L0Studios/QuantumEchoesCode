//PURPOSE: Attached to triggers that when activated result in a sound being played.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneShotSound : MonoBehaviour
{
    [SerializeField] bool multiPlay = false; //if we want the trigger to play repeatedly forever
    [SerializeField] float pauseTime = 5.0f; //Use with "Multiplay" -how long to wait before the sound can be repeated again.Stops rapid spamming if a character keeps triggering something.
    [SerializeField] AudioClip soundToPlay; //what sound we want to play
    [SerializeField] bool permanent; //used to track if this ever gets disabled
    [SerializeField] AudioSource remoteAudioSource; // Do we want to use an audiosource on another object?
    private AudioSource oneShot; //where we are playing the audio.
    private Collider colliderObj; //used to store the attached TRIGGER collider to this object. If this is a wall, dont attach the regular collider, only sound trigger collider. (there should be two colliders)
    private bool recharging = false; //use to track if we are waiting to play again.

    void Start()
    {
        if (remoteAudioSource != null) //are we using an remote audio source?
        {
            oneShot = remoteAudioSource; //if so assign that as the audiosource to use
            //remoteAudioSource.
        }
        else
        {
            oneShot = GetComponent<AudioSource>(); //otherwise, use one attached to this object

        }
        colliderObj = GetComponent<Collider>(); //capture the collider attached to this object for later use
    }
    private void OnTriggerEnter(Collider other) //when something collides with the trigger collider
    {
        if (other.CompareTag("Player") && recharging == false) //was it the player, and is the sound NOT going through a wait period before playing again.
        {
            if (!oneShot.isPlaying) //Also check if it already is playing. If not...
            {
                recharging = true; //set it to recharging mode
                oneShot.clip = soundToPlay; //Load the clip
                oneShot.Play(); //play the sound
                
               if (permanent==false) colliderObj.gameObject.SetActive(false); //if this is not supposed to be permanent. disable the trigger collider
               else StartCoroutine(soundReset()); //otherwise launch the wait coroutine to wait the designated time until we can play the clip again.
            }
        }
    }
    IEnumerator soundReset() //Used to wait a period before the sound can be triggered again
    {
        yield return new WaitForSeconds(pauseTime); //wait the amount of time specified by pausetime
        recharging = false; //mark the sound as ready to play again
        colliderObj.enabled = true; //enable the trigger collider so the player can trigger again
    }
}
//DEBUG Box
//Debug.Log("collided with: " + collision.gameObject.tag);
