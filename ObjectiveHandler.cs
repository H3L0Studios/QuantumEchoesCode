//PURPOSE: To manage the objectives for each level. This script should be placed once in each level on the objective panel in the inventory canvas.
//Dev should populate the arrays for each objective with thenecessary data. Then drag the specific gameobjects to the UI objects regarding objectives.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveHandler : MonoBehaviour
{
    [SerializeField] public string[] objectiveTexts; //Texts displayed for each objective when they are started or ended.
    public int[] objectiveStatuses; //0=not started, 1 = started. 2 = Complete
    [SerializeField] AudioClip[] objClips; // audio clips that will be used for the respective objective
    [SerializeField] GameObject objPanel; // the objective panel in the player inventory
    [SerializeField] GameObject newObjText; //Text to be displayed when an objective goes active.
    [SerializeField] GameObject finishObjText; // Text to be displayed when an objective is complete.
    [SerializeField] AudioClip objNewSound; // SFX for new objective
    [SerializeField] AudioClip objCompleteSound; // SFX fo complete objective
    private float fadeDuration = 1f; // how long the objective notification fades away for
    private float textDuration = 3f; // how long the objective notification stays before it starts to fade away
    private int objNum; // used to track what objective number is being worked on for correlating objective to soundfx, etc
    private bool objFinish; //used to trigger finished objective notification.
         
    void Start()
    {
        if (SaveScript.savedGame == false) StartObjective(0); // makes sure we start with the first objective if its a new game.
        if (SaveScript.stageCheckpoint == 0) StartObjective(0); // makes sure we start with the first objective when starting at the beginning of a level
        if (SaveScript.savedGame == true) // loads the appropriate objectives based on what was active last time we saved
        {
            for (int i = 0; i < SaveScript.activeObjectives.Length; i++)
            {
                StartObjective(SaveScript.activeObjectives[i]);
            }
        }
    }

    public void StartObjective( int objectiveNumber) // puts an objective in the started state
    {
        if (objectiveStatuses[objectiveNumber] == 0) // makes sure the objective is in an unstarted state
        {
                newObjText.GetComponent<Text>().text = "New Objectives: "+ objectiveTexts[objectiveNumber]; //populate the objective notification box with the corresponding objective text
                newObjText.GetComponent<Text>().color = new Color(255, 0, 0, 1); // set the green color for the text
                objPanel.GetComponent<Image>().color = new Color(0, 0, 0, 0.596f); // set the background image alpha to 59.6% opaque 
                objPanel.GetComponent<AudioSource>().clip = objNewSound; // load the objective start sound
                objPanel.GetComponent<AudioSource>().Play(); // play the sound
                StartCoroutine(FadeOutPanel()); // fade out the notification window after certain time: fadeduration variable 
                objectiveStatuses[objectiveNumber] = 1; // set the corresponding objective to an active status           
        }
    }

    public void FinishObjective(int objectiveNumber) // puts an objective in the finished state
    {
        if (objectiveStatuses[objectiveNumber] == 1) // Make sure the objective being completed was in an active state
        {
            objFinish = true; //used by the fade function to determine if the fade is related to a finish or start
            finishObjText.GetComponent<Text>().text = "Objective Complete: " + objectiveTexts[objectiveNumber]; //populate the objective notification box with the corresponding objective text
            finishObjText.GetComponent<Text>().color = new Color(0, 255, 0, 1); // Set the finish text to red
            objPanel.GetComponent<Image>().color = new Color(0, 0, 0, 0.596f); // set the background image alpha to 59.6 % opaque
            objPanel.GetComponent<AudioSource>().clip = objNewSound; // load the objective start sound
            objPanel.GetComponent<AudioSource>().Play(); // play the sound
            StartCoroutine(FadeOutPanel()); // fade out the notification window after certain time: fadeduration variable
            objectiveStatuses[objectiveNumber] = 2; //set the status of the objective to complete
            objNum = objectiveNumber; // used for SFX matching to the corresponding objective
        }
    }

    IEnumerator FadeOutPanel() //fades out the notification panel
    {
        yield return new WaitForSeconds(textDuration); //wait before fading out
        yield return Fade(); // fade Out
    }

    private IEnumerator Fade() //Fades out the objective notification - called from start and finish notification functions above
    {
        //initialize
        Image panelColor = GetComponent<Image>();
        Text newText = newObjText.GetComponent<Text>();
        Text finishText = finishObjText.GetComponent<Text>();
        //Get Initial Colors
        Color initialNewTextColor = newText.color;
        Color initialFinishTextColor = finishText.color;
        Color initialColor = panelColor.color;
        //Set Target Colors
        Color targetColor = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);
        Color newtargetColor = new Color(initialNewTextColor.r, initialNewTextColor.g, initialNewTextColor.b, 0f);
        Color finishtargetColor = new Color(initialFinishTextColor.r, initialFinishTextColor.g, initialFinishTextColor.b, 0f);
        // Do the fade
        float elapsedTime = 0f; //set the fade timer to 0
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            panelColor.color = Color.Lerp(initialColor, targetColor, elapsedTime / fadeDuration);
            newText.color = Color.Lerp(initialNewTextColor, newtargetColor, elapsedTime / fadeDuration);
            finishText.color = Color.Lerp(initialFinishTextColor, finishtargetColor, elapsedTime / fadeDuration);
            yield return null;
        }
        if (objFinish == true) // check if this was a fade for a "finish objective" 
        {
            objPanel.GetComponent<AudioSource>().clip = objClips[objNum]; // load a related finish audio clip
            objPanel.GetComponent<AudioSource>().Play();// play a related audio clip
            objFinish = false;
        }
    }
    public bool ObjectivesCompleted(int thisObjective) // Used by triggers that end a level to make sure all of the objectives were finished. "thisobjective" is the final objective to leave the level.
    {
        bool status = true; //initialize the status as true until proven otherwise below
        for (int i = 0; i < objectiveStatuses.Length; i++)//loop through the objectives array
        {
            if (objectiveStatuses[i] == 2) //See if the current objective in the array is completed (2)
            {
                status = true;//keep the completion status as true
            }
            else if (objectiveStatuses[i] < 2 && thisObjective != i) // check if any objective is in a not completed state BESIDES the final objective
            {
                status = false; // if so, set the status to false, as in we are not ready to leave.
                i = objectiveStatuses.Length; //ends the loop so we can save some cycles and not re-mark the status as true.
            }
        } 
        return status;
    }

    public bool ObjectivesStarted(int thisObjective) // Used in AlterObjectsTrigger to check if the objective was already started meaning a trigger already altered the object in question so we do not repeat it.
    {
        bool status = true;//start initializing at true
        for (int i = 0; i < objectiveStatuses.Length; i++) //loop through the objectives array
        {

            if (objectiveStatuses[i] > 0)//if the objective is not unstarted, then conditions stays true
            {
                status = true;
            }
            else if (objectiveStatuses[i] == 0 && thisObjective != i) //if the objective is unstarted and not related to the objective being checked, return false and halt the check.
            {
                status = false;
                i = objectiveStatuses.Length;
            }

        }
        return status;
    }

    public void SaveObjectives() // used to save our objectives at a save checkpoint
    {
        //find out how big our array needs to be
        int startedCount = 0;
        for (int i = 0; i < objectiveStatuses.Length; i++)
        {
            if (objectiveStatuses[i] == 1)
            {
                startedCount++;
            }
        }
        //set the array size and capture the started objectives
        int[] startedObjectives = new int[startedCount];
        startedCount = 0;
        for (int i = 0; i < objectiveStatuses.Length; i++)
        {
            if (objectiveStatuses[i] == 1)
            {
                startedObjectives[startedCount] = i;
            }
        }

        SaveScript.activeObjectives = startedObjectives; //move the array to savescript where they will get used for the actual saving
    }
    
}
//DEBUG BOX - Used for storing commonly used debug statements for future troubleshooting.
//-------------------------------------------------------------
//Debug.Log("Objective " + objectiveNumber + " started.");
//Debug.Log("Obj " + i + " - status: " + objectiveStatuses[i]);
//Debug.Log("Status to exit is: " + status);
