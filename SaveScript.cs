//Purpose: This script holds the game state & main character state attributes. much of this is used for saving the game, tracking certain objectives, and managing combat inventory attributes
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SaveScript : MonoBehaviour
{
    //For templating later, we could turn this in a few arrays to make it less static. Future improvement.
    //HUD status
    public static int playerHealth = 100;
    public static bool healthChanged = false;
    public static float batteryPower = 1.0f;
    public static bool batteryRefill = false;
    public static bool flashLightOn = false;
    public static bool nvLightOn = false;

    //inventory checks
    public static int appleCount = 0;
    public static int shellCount = 0;
    public static int waterBottleCount = 0;//set up in inventory - also add Playerprefs
    public static int firstAidCount = 0;//set up in inventory
    public static int batteryCount = 0;
    public static bool hasBackpack = false;
    public static bool hasCarKeys = false;//set up in inventory
    public static bool hasFlashlight = false;
    public static bool hasNightVision = false;
    public static bool hasKnife = false;
    public static bool hasHandgun = false;
    public static bool hasBat = false;
    public static bool hasCrossbow = false;
    public static bool hasAxe = false;

    //keys
    public static bool hasCabinKey = false;
    public static bool hasHouseKey = false;
    public static bool hasRoomKey = false;
    public static bool hasMetalPipe = false;
    public static bool hasL1GateKey = false;

    //ammos
    public static int bulletCount = 0;
    public static int boltCount = 4;
    
    //active weapon tracking
    public static bool knifeEquiped = false;
    public static bool batEquiped = false;
    public static bool axeEquiped = false;
    public static bool gunEquiped = false;
    public static bool crossbowEquiped = false;
    
    //player activity state tracking
    public static bool inInventory = false;
    public static bool isReading = false;
    public static bool isBeingAttacked = false;
    public static bool inOptions = false;

    //Door Status
    public static bool l1_RustyGateOpen = false;

    //Objective Tracking For Saving Game
    public static int[] activeObjectives = new int[0];
    public static int objArrayLength;

    //other
    public static bool newGame = false;
    public static int maxEnemys = 6;
    public static int enemiesOnScreen = 0;
    public static float stamina = 10f;
    public static float maxStamina = 10f;
    public static bool savedGame = false;
    public static int difficulty = 3;
    public static int currentStage = 1;
    public static int stageCheckpoint = 1;

   void Start() //checks if we have a saved game at start
    {
        if (PlayerPrefs.GetInt("SavedGame") == 1) savedGame = true; //if we have a saved game, used to display the load graphic on the main menu
        if (newGame == true) // check if we are starting a new game, if so reset all the game state attributes.
        {
            PlayerPrefs.DeleteAll(); //Clear all save data since we are starting a new game
            playerHealth = 100;
            healthChanged = false;
            batteryPower = 1.0f;
            batteryRefill = false;
            flashLightOn = false;
            nvLightOn = false;
            appleCount = 0;
            batteryCount = 0;
            hasKnife = false;
            hasHandgun = false;
            hasBat = false;
            hasBackpack = false; 
            hasCrossbow = false;
            hasAxe = false;
            hasCabinKey = false;
            hasHouseKey = false;
            hasRoomKey = false;
            bulletCount = 0;
            boltCount = 4;
            knifeEquiped = false;
            batEquiped = false;
            axeEquiped = false;
            gunEquiped = false;
            crossbowEquiped = false;
            inInventory = false;
            isBeingAttacked = false;
            newGame = false;
            currentStage = 1;
            stageCheckpoint = 0;
            //GetComponent<InteractablesTracker>().DifficultyAdjustment(); future use if we implement difficulty.
            savedGame = false;
        }

        if(savedGame == true) //This is used if the savedGame attribute is true - which means the player chose to load a game, so we load all the saved player prefs into the attributes.
        {
            
            playerHealth = PlayerPrefs.GetInt("PlayersHealth", SaveScript.playerHealth);
            healthChanged = true; //this is used by the health script to update the graphic.
            maxStamina =  PlayerPrefs.GetFloat("PlayersMaxStamina", SaveScript.maxStamina);
            batteryPower = PlayerPrefs.GetFloat("BatteriesPower", SaveScript.batteryPower);
            appleCount = PlayerPrefs.GetInt("ApplesCount", SaveScript.appleCount);
            shellCount = PlayerPrefs.GetInt("shellsCount", SaveScript.shellCount);
            batteryCount = PlayerPrefs.GetInt("BatteriesCount", SaveScript.batteryCount);
            bulletCount = PlayerPrefs.GetInt("BulletsCount", SaveScript.bulletCount);
            boltCount = PlayerPrefs.GetInt("BoltsCount", SaveScript.boltCount);
            difficulty = PlayerPrefs.GetInt("Difficult", SaveScript.difficulty);
            currentStage = PlayerPrefs.GetInt("Stage", SaveScript.currentStage);
            stageCheckpoint = PlayerPrefs.GetInt("Checkpoint", SaveScript.stageCheckpoint);
            if (PlayerPrefs.GetInt("HaveKnife") == 1) hasKnife = true;
            if (PlayerPrefs.GetInt("HaveAxe") == 1) hasAxe = true;
            if (PlayerPrefs.GetInt("HaveBat") == 1) hasBat = true;
            if (PlayerPrefs.GetInt("HaveGun") == 1) hasHandgun = true;
            if (PlayerPrefs.GetInt("HaveCrossbow") == 1) hasCrossbow = true;
            if (PlayerPrefs.GetInt("HaveBackpack") == 1) hasBackpack = true;
            if (PlayerPrefs.GetInt("HaveCabinKey") == 1) hasCabinKey = true;
            if (PlayerPrefs.GetInt("HaveHouseKey") == 1) hasHouseKey = true;
            if (PlayerPrefs.GetInt("HaveRoomKey") == 1) hasRoomKey = true;
            //GetComponent<InteractablesTracker>().LoadObjects();

            //Load Active Objectives
            
            if(stageCheckpoint == 0)
            {
                 //this makes 1st objective of the scene active since we are starting at the beginning of the level.
            }
            else  //Otherwise load the last objective(s) during the save checkpoint.
            {
                objArrayLength = PlayerPrefs.GetInt("ObjectiveArrayLength", SaveScript.activeObjectives.Length);
                int[] objs = new int[objArrayLength];
                for (int i = 0; i < objArrayLength; i++)
                {
                    objs[i] = PlayerPrefs.GetInt("Objective" + i, 0);
                }
                activeObjectives = objs;
            }   
        }   
    }
}
//DEBUG Box
//Debug.Log(objs.Length + " Objectives Loaded");
//Debug.Log(activeObjectives.Length + " Objectives Loaded");