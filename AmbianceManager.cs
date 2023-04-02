//PURPOSE: Used to manage Ambiance sounds/music changes
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbianceManager : MonoBehaviour
{
    [SerializeField] AudioSource musicBox; //1 exists in each level to handle 2D sounds for music

    public void ChangeMusic(AudioClip track) //called to change music to the passed audio clip
    {     
        StartCoroutine(Fade(track)); //Fade old music out and new music in
    }

    IEnumerator Fade(AudioClip newClip)  //Fade old music out and new music in over 6 seconds
    {
        float startVolume = musicBox.volume;
        while (musicBox.volume > 0)
        {
            musicBox.volume -= startVolume * Time.deltaTime / 3f;
            yield return null;
        }
        musicBox.Stop();
        musicBox.clip = newClip;
        musicBox.Play();
        while (musicBox.volume < startVolume)
        {
            musicBox.volume += startVolume * Time.deltaTime / 3f;
            yield return null;
        }
    }
}
//DEBUG BOX
//        Debug.Log("ChangingTrack To: " + newClip.name);
