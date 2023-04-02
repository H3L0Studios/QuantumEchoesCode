//PURPOSE: Runs a Cut Scene once a cutscene object this is attached to is enabled 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;


public class CutSceneRunner : MonoBehaviour
{
    [SerializeField] PlayableDirector director; //get the Timeline director from the related timeline container object 
    //player stuff
    [SerializeField] GameObject fpc_Camera; //Get the player camera, for returning to it at the end of the cutscene
    [SerializeField] bool moveCameraToPlayerAtStart; //
    [SerializeField] GameObject cutSceneCamera; //get the Cutscene camera object
    [SerializeField] GameObject fpc; //Get the player gameobject (stands for first person character)
    [SerializeField] GameObject canvas; //get the main UI Canvas
    //things to turn on and off
    [SerializeField] GameObject[] switchOnStuff; //things to switch on during a cutscene
    [SerializeField] GameObject[] switchOffStuff; //things to switch off diring a cutscene
    [SerializeField] GameObject[] switchOnStuff_End; //things to switch on at the end of a cutscene
    [SerializeField] GameObject[] switchOffStuff_End; //things to switch off at the end of a cutscene
    //change music
    [SerializeField] AudioClip exitWithNewMusic; //play new music at end of cut scene
    [SerializeField] AudioSource musicBox; //where music gets played from.

    void Awake() //when the cutscene is started
    {
        fpc_Camera.SetActive(false); //turn off the player camera
        canvas.SetActive(false); //hide the UI canvas (so HUD doesnt get in way)
        if (switchOnStuff.Length > 0) //check if we want to switch anything on at the start of the cut scene
        {
            for (int i = 0; i < switchOnStuff.Length; i++) //loop through the stuff to turn on
            {
                switchOnStuff[i].SetActive(true); //turn a thing on
            }
        }
        if (switchOffStuff.Length > 0) //check if we want to switch anything off at the start of the cut scene
        {
            for (int i = 0; i < switchOffStuff.Length; i++) //loop through the stuff to turn off
            {
                switchOffStuff[i].SetActive(false); //turn a thing off
            }
        }
        musicBox.Stop(); //stop any music
        CharacterControl.characterFreeze = true; //turns off the character controller so we cant move the player
        director.Play(); //play the timeline recording
    }

    void Update()
    {
      //maybe put a skip option in  
    }

    public void EndScene() //called at the end of the timeline by the director
    {
        if (switchOnStuff_End.Length > 0) //check if we want to switch anything on at the end of the cut scene
        {
            for (int i = 0; i < switchOnStuff_End.Length; i++) //loop through the stuff to turn on
            {
                switchOnStuff_End[i].SetActive(true); //turn a thing on
            }
        }
        if (switchOffStuff_End.Length > 0) //check if we want to switch anything off at the end of the cut scene
        {
            for (int i = 0; i < switchOffStuff_End.Length; i++) //loop through the stuff to turn off
            {
                switchOffStuff_End[i].SetActive(false); //turn a thing off
            }
        }
        fpc_Camera.SetActive(true); //re-enable the player camera
        canvas.SetActive(true); //re-enable the UI Canvas
        if (moveCameraToPlayerAtStart == true) //check if we need to move the player to where the cut scene camera ended
        {
            fpc.transform.position = new Vector3(cutSceneCamera.transform.position.x, cutSceneCamera.transform.position.y, cutSceneCamera.transform.position.y); //move player to position
            fpc.transform.rotation = new Quaternion(cutSceneCamera.transform.rotation.x, cutSceneCamera.transform.rotation.y, cutSceneCamera.transform.rotation.z, cutSceneCamera.transform.rotation.w); //move player rotation
        }
        musicBox.clip = exitWithNewMusic; //load new music to play
        musicBox.Play(); //play the music
        CharacterControl.characterFreeze = false; //unfreeze player controls
        gameObject.SetActive(false); //disable the cut scene object
    }
}
