//PUROPOSE: Handles Save and Load Acitivies.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoad : MonoBehaviour
{
    public int dataExists = 10;
    [SerializeField] GameObject loadButton;
    [SerializeField] GameObject difficultyPanel;
    [SerializeField] GameObject warningPanel;
    [SerializeField] GameObject menuPanel;

    void Start()
    {
        if (loadButton != null) //make sure a load button object is specified
        {
            
            loadButton.SetActive(false); //disable it if there int an object specified
            dataExists = PlayerPrefs.GetInt("PlayersHealth", 0); // Data will never be saved with player health 0 so we use it as a verification that there is save data or not.
            if (dataExists > 0)//check if there is save data
            {
                loadButton.SetActive(true); //if so set the load button to active on the title screen
            }
        }
        Cursor.visible = true; //make the cursor visible so player can click
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) == true && difficultyPanel!=null) //check if the player hit escape to back out of the difficulty menu
        {
            difficultyPanel.SetActive(false);//if so disable the difficulty panel
        }
    }

    public static void SaveGame() //called to save the game data
    {
        SaveScript.savedGame = true; //for use in the existing game to make the load button show on the main menu.
        //write the attributes to save to the playerprefs data
        PlayerPrefs.SetInt("PlayersHealth", SaveScript.playerHealth);
        PlayerPrefs.SetFloat("PlayersMaxStamina", SaveScript.maxStamina);
        PlayerPrefs.SetFloat("BatteriesPower", SaveScript.batteryPower);
        PlayerPrefs.SetInt("ApplesCount", SaveScript.appleCount);
        PlayerPrefs.SetInt("shellsCount", SaveScript.shellCount);
        PlayerPrefs.SetInt("BatteriesCount", SaveScript.batteryCount);
        PlayerPrefs.SetInt("BulletsCount", SaveScript.bulletCount);
        PlayerPrefs.SetInt("BoltsCount", SaveScript.boltCount);
        PlayerPrefs.SetInt("Difficult", SaveScript.difficulty);
        PlayerPrefs.SetInt("Stage", SaveScript.currentStage);
        PlayerPrefs.SetInt("Checkpoint", SaveScript.stageCheckpoint);

        //save objectives
        if (SaveScript.activeObjectives.Length > 0)
        {
            for (int i = 0; i < SaveScript.activeObjectives.Length; i++)
            {
                PlayerPrefs.SetInt("Objective" + i, SaveScript.activeObjectives[i]);
            }
            PlayerPrefs.SetInt("ObjectiveArrayLength", SaveScript.activeObjectives.Length);
        }
        //save bools - 0 = you don't have it, 1 = you have it.
        if (SaveScript.hasKnife == true) PlayerPrefs.SetInt("HaveKnife", 1);
        if (SaveScript.hasBat == true) PlayerPrefs.SetInt("HaveBat", 1);
        if (SaveScript.hasAxe == true) PlayerPrefs.SetInt("HaveAxe", 1);
        if (SaveScript.hasHandgun == true) PlayerPrefs.SetInt("HaveGun", 1);
        if (SaveScript.hasCrossbow == true) PlayerPrefs.SetInt("HaveCrossbow", 1);
        if (SaveScript.hasBackpack == true) PlayerPrefs.SetInt("HaveBackpack", 1);
        if (SaveScript.hasCabinKey == true) PlayerPrefs.SetInt("HaveCabinKey", 1);
        if (SaveScript.hasHouseKey == true) PlayerPrefs.SetInt("HaveHouseKey", 1);
        if (SaveScript.hasRoomKey == true) PlayerPrefs.SetInt("HaveRoomKey", 1);
        if (SaveScript.savedGame == true) PlayerPrefs.SetInt("SavedGame", 1);
    }
   
    public void LoadGame() //called when player clicks the load button on main menu
    {
        SaveScript.savedGame = true;//setting this makes savescript load saved playerprefs data when the game starts.   
    }

    public void ShowDifficultyMenu()//shows the difficulty menu when new game is clicked on main menu
    {
        
        difficultyPanel.SetActive(true); //enable the difficulty select menu
    }
    public void ShowNewGameWarning(int difficulty)//when a difficulty is selected for a new game, enables the save game wipe warning.
    {
        SaveScript.difficulty = difficulty; // also sets the difficulty that was selected. Might need to move this elsewhere later.
        warningPanel.SetActive(true); //show the warning panel
    }

    public void ReallyStartNew() //Called when player accepts the save wipe warning to actually start the new game.
    {
        PlayerPrefs.DeleteAll();//wipes previous save data
        SaveScript.savedGame = false; //used to signify there is currently no saved game, so the load button will show at the main menu
        warningPanel.SetActive(false);//hide warning panel
        difficultyPanel.SetActive(false);//hide the difficulty panel
        menuPanel.SetActive(false);//hide the rest of the main menu
    }
    public void GoBack()//called when escape is pressed to go back in the main menu
    {
        warningPanel.SetActive(false);//hide warning panel
        difficultyPanel.SetActive(false);//hide difficulty panel
    }
}
