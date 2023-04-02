//PURPOSE: Used For Fading in and out the screen. uses a Black square image stretched to the players screen size. This is applied to the Object holding the Square and Frame.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelFadeOut : MonoBehaviour
{
    private Image blackSquare; //The black square we fade to on fade out
    private RectTransform frame; //the frame for the image...The black square lives in this and this is what we resize to stretch it to fill the whole screen.
    private int high; //holds the Y value of screen size
    private int wide; //holds the X value of screen size
    [SerializeField] float fadeOutTime; //How long it takes to fade out
    [SerializeField] float fadeInTime; //How long it takes to fade in

    void Start()
    {
        blackSquare = GetComponent<Image>(); //get the image
        blackSquare.color = new Color(0, 0, 0, 0.0f); //start with the square being transparent
        frame = GetComponent<RectTransform>(); //get the frame to resize
        wide = Screen.currentResolution.width; //get the screen width
        high = Screen.currentResolution.height; //get the screen height
        frame.sizeDelta = new Vector2(wide, high); //set the frame to the recently caprtures screen width and height
    }

    public void fadeBlack() //Function that is called to fade to black
    {
        blackSquare.enabled = true; //turn on the black square
        StartCoroutine(fadeOut()); //run the fade out transition
    }

    public void fadeClear() //fades us back in making the black square transparent
    {
        StartCoroutine(fadeIn()); //run the fade in transition
    }

    private IEnumerator fadeIn() //fades the black screen out
    {
        float elapsedTime = 0.0f; //initialize a timer
        Color c = blackSquare.color; //get our squares color
        while (elapsedTime < fadeInTime) //while we are elapsing the fade in time
        {
            yield return null; //return null as we dont return anything for this
            elapsedTime += Time.deltaTime; //passes time against normal time
            c.a = 1.0f - Mathf.Clamp01(elapsedTime / fadeInTime); //set the alpha of the square to 100% minus the percentage of the time that as elapsed.
            blackSquare.color = c; //set the new color with 0 alpha as the color of the square as the time elapses allowing us to see the fade.
        }
    }
    private IEnumerator fadeOut() //fade the black screen in
    {
        float elapsedTime = 0.0f; //initialize our timer
        Color c = blackSquare.color; //store the black square color
        while (elapsedTime < fadeOutTime) //while time elapses
        {
            yield return null; //return nothing - coroutines must have a retuen
            elapsedTime += Time.deltaTime; //as real time passes
            c.a = Mathf.Clamp01(elapsedTime / fadeOutTime); //set the color of the square's alpha to the % of time elapsed. As time moves forward the square fades in more and more.
            blackSquare.color = c; //set the squares color to the change as time passes visualizing the fade.
        }
    }
}
//DEBUG BOX
//Debug.Log(wide + " " + high); //for checking that the black box matches the screen size
