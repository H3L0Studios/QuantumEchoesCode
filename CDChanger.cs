//PURPOSE: Add this to an object with a trigger collider to make it change the music.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDChanger : MonoBehaviour
{
    [SerializeField] AmbianceManager musicSource; //object with the audio player
    [SerializeField] AudioClip track; //music file to play

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player")) musicSource.ChangeMusic(track);//If triggered by a player, change the music
    }
}
