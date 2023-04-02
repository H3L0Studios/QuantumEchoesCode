//PURPOSE: Attached to Note UI Canvas objects to handle what happens after its displayed to the player
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class NoteReader : MonoBehaviour
{
    [SerializeField] bool CutScenePivot = false; // If enabled will tell a cutscene to start when the note is closed
    [SerializeField] GameObject cutScene; // what cut scene to start when a note is closed
    [SerializeField] bool isReadingAtStart = false; //used to display a note when a level starts
    
    void Start()
    {
        if (isReadingAtStart == true) //Do we want to display a note at the level start?
        {
            SaveScript.isReading = true;//if so set us to a reading state.
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SaveScript.isReading == true && Input.anyKeyDown) // If we are reading and the player hits any key
        {
            SaveScript.isReading = false; //mark us as not reading
            this.gameObject.SetActive(false); //disable the not UI display
            if (CutScenePivot == true) cutScene.SetActive(true); //if we are supposed to dsiplay a cutscene after closing, enable the cutscene object.
        }
    }

    
}
