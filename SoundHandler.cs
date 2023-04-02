//PURPOSE: So far manages attack music going in and out. may be redundant with ambiance manager. Probably can combine in future versions. 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHandler : MonoBehaviour
{
    [SerializeField] GameObject attackMusic;
    [SerializeField] bool fadeIn;
    [SerializeField] bool fadeOut;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = attackMusic.GetComponent<AudioSource>();
        
    }

    void Update()
    {
        if (SaveScript.isBeingAttacked == true)
        {
            attackMusic.SetActive(true);
        }
        if (SaveScript.isBeingAttacked == false && attackMusic.activeSelf == true)
        {
            StartCoroutine (FadeOut());
        }
    }

    IEnumerator FadeOut()
    {
        float startVolume = audioSource.volume;
        while (audioSource.volume > 0 && attackMusic.activeSelf == true)
        {
            audioSource.volume -= startVolume * Time.deltaTime / 250f;
            yield return null;
        }
        audioSource.Stop();
        attackMusic.SetActive(false);
        audioSource.volume = startVolume;
    }
}
