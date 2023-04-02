//PURPOSE: To place this on objects that can trigger an objective change. Currently doors, object pickups, objects disabling (like notes we just read), and trigger colliders.
//The variables set in the GUI help the script make calls to the objective handler object for each level to manipulate objective statuses.
//--------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectiveTrigger : MonoBehaviour
{
    [SerializeField] ObjectiveHandler objHandler;
    [SerializeField] bool finishObjective = false;
    [SerializeField] int objectiveFinishedNumber;
    [SerializeField] bool nextObjective = false; 
    [SerializeField] public int[] nextObjectiveNumber;
    [SerializeField] bool startUpObjective;
    [SerializeField] bool NoteObjective; //Not used yet - future use for achievements and more detailed object tracking
    [SerializeField] bool doorObjective;
    [SerializeField] bool collectObjective; //Not used yet - future use for achievements and more detailed object tracking
    [SerializeField] bool endScene;
    [SerializeField] int nextScene;
    [SerializeField] LoadingScreenBarSystem loadScreenScript;
    private bool triggered = false;

    void Start()
    {
        if(startUpObjective == true)//If this objective on the instance of the trigger has been marked as a start-up objective at the beginning of a stage, activate it when the level starts. 
        {
           changeObj(); //Sets the Objective(s) associated with this specific trigger as active
        }
    }
    void Update()
    {
        if (doorObjective == true && GetComponent<DoorScript>().isOpen == true && objHandler.objectiveStatuses[objectiveFinishedNumber] == 1) //Watch if a door with an objective trigger is opened and active, then end the objective
        {
            changeObj(); //Kicks off the objective state change process for the associated objectives.

        }  
    }

    private void changeObj() //initiates an objective state change by calling the objective handler functions to start and/or finish and objective
    {
        if (finishObjective == true) //check to see if this is a objective finished event.
        {
             objHandler.FinishObjective(objectiveFinishedNumber); // If so, Finish the objective. Objective Handler checks for the right pre-reqs.
        }
        if (nextObjective == true)//If there is a new follow-on objective
        {
            for (int i = 0; i < nextObjectiveNumber.Length; i++)//cycle through the array of new objectives
            {
                objHandler.StartObjective(nextObjectiveNumber[i]);//for each new objective, activate it
            }
        }  
    }
    private void OnTriggerEnter(Collider other) // For trigger objectives when the player walks through a trigger collider
    {
        if (other.CompareTag("Player") == true && triggered == false && endScene == false && doorObjective==false)//We want the player to trigger this objective, make sure it already isnt triggered, that its not a trigger to end the level, and that its not a door objective
        {
            changeObj();//process the objective change
            triggered = true; //set it as triggered so we cant repeat it.
        }
        if (other.CompareTag("Player") == true && endScene == true && objHandler.ObjectivesCompleted(objectiveFinishedNumber) == true) // This check if this is an end scene trigger and makes sure all objectives are completed.
        {
            SaveScript.savedGame = true; // marks the pesence of a saved game if there arelready isnt
            SaveScript.currentStage = nextScene; //Sets the stage we are moving to for saving purposes
            SaveScript.stageCheckpoint = 0; //sets the stage checkpoint to the first one
            SaveLoad.SaveGame();//saves all the playerprefs including stage infro we just set.
            loadScreenScript.loadingScreen(); //display the loading screen
            SceneManager.LoadScene(nextScene); //Load the next scene
        }
    }
    private void OnDisable() //used to trigger objectives when an object gets disabled - useful when collecting items
    {
        if (finishObjective == true && objHandler.ObjectivesCompleted(objectiveFinishedNumber) == false) // if this is a finish objective, check to make sure it already wasnt finished. Shouldnt happen but checking in case.
        {  
            objHandler.FinishObjective(objectiveFinishedNumber); //finish the objective
        }
        if (nextObjective == true)//check to see if there is a new objective to start
        {
            for (int i = 0; i < nextObjectiveNumber.Length; i++)//loop through the array of new objectives
            {
                objHandler.StartObjective(nextObjectiveNumber[i]);//start them
            }
        }
    }
}
//DEBUG BOX - Stores commonly use debug statements
