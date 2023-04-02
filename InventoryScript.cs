//PURPOSE: Manages the Backpack and things that we have. Manages weapon usage. manages usage of inventory items.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryScript : MonoBehaviour
{
    
    [SerializeField] GameObject inventoryOverlay;
    [SerializeField] AudioClip appleBite;
    [SerializeField] AudioClip batterySwap;

    //Apples
    [SerializeField] GameObject appleImage;
    [SerializeField] GameObject appleButton;
    [SerializeField] GameObject appleText;
    
    //Batteries
    [SerializeField] GameObject batteryImage;
    [SerializeField] GameObject batteryButton;
    [SerializeField] GameObject batteryText;

    //Weapons
    [SerializeField] GameObject knifeImage;
    [SerializeField] GameObject batImage;
    [SerializeField] GameObject axeImage;
    [SerializeField] GameObject handgunImage;
    [SerializeField] GameObject crossbowImage;

    //Keys
    [SerializeField] GameObject cabinKeyImage;
    [SerializeField] GameObject houseKeyImage;
    [SerializeField] GameObject roomKeyImage;
    [SerializeField] GameObject metalPipeImage;

    //shells
    [SerializeField] GameObject shellImage;
    [SerializeField] GameObject shellText;

    //Ammo
    [SerializeField] GameObject bulletImage;
    [SerializeField] GameObject boltImage;
    [SerializeField] GameObject bulletText;
    [SerializeField] GameObject boltText;
    [SerializeField] GameObject bulletAmountText;
    [SerializeField] GameObject gunUIImage;
    [SerializeField] GameObject boltAmountText;
    [SerializeField] GameObject crossbowUIImage;
    private AudioSource soundPlayer;

    //Arms-Objects
    [SerializeField] GameObject armsObject;
    [SerializeField] GameObject armsKnife;
    [SerializeField] GameObject armsBat;
    [SerializeField] GameObject armsAxe;
    [SerializeField] GameObject armsGun;
    [SerializeField] GameObject armsCrossbow;
    [SerializeField] AudioClip weaponChange;
    [SerializeField] AudioClip unholsterSound;
    [SerializeField] AudioClip crossbowSound;

    //Animations
    [SerializeField] Animator animHands;

    //Objectives
    [SerializeField] Text objText;
    [SerializeField] ObjectiveHandler objHandler;

    // Start is called before the first frame update
    void Start()
    {
        //initialize everything
        inventoryOverlay.SetActive(false);
        appleButton.SetActive(false);
        appleImage.SetActive(false);
        appleText.SetActive(false);
        batteryButton.SetActive(false);
        batteryImage.SetActive(false);
        batteryText.SetActive(false);
        bulletImage.SetActive(false);
        if (shellImage!=null) shellImage.SetActive(true);
        bulletText.SetActive(false);
        boltImage.SetActive(false);
        boltText.SetActive(false);
        gunUIImage.SetActive(false);
        bulletAmountText.SetActive(false);
        crossbowUIImage.SetActive(false);
        boltAmountText.SetActive(false);
        soundPlayer = GetComponent<AudioSource>();
        Cursor.visible = false;
        Time.timeScale = 1f;
    }

    void Update()
    {
        //Check if Inventory Opened
        if (Input.GetKeyDown(KeyCode.I) && !SaveScript.inOptions && SaveScript.hasBackpack == true)
        {
            if (inventoryOverlay.activeSelf == false)
            {
                inventoryOverlay.SetActive(true); //enable inmventory screen
                Time.timeScale = 0f; //stop time
                Cursor.visible = true; //show cursor
                SaveScript.inInventory = true; //mark that we are in enventory. used to do other things while there.
                BuildObjectiveList(); //We call this to compile all the active objectives to display in the objective panel
            }
            else //we need to close the inventory
            {
                inventoryOverlay.SetActive(false);
                Time.timeScale = 1f; //time is back to normal
                Cursor.visible = false;
                SaveScript.inInventory = false;
            }
            checkInventory(); //used to see if we have or dont have certain things in inventory that we need to display or not
        }
        //Check if Weapon Selected
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (SaveScript.hasKnife == true)
            {
                equipKnife();
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (SaveScript.hasBat == true)
            {
                equipBat();
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (SaveScript.hasAxe == true)
            {
                equipAxe();
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (SaveScript.hasHandgun == true)
            {
                equipGun();
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            if (SaveScript.hasCrossbow == true)
            {
                equipCrossbow();
            }
        }
    }

    void checkInventory()
    {
        //Check if we can display apples in inventory
        if (SaveScript.appleCount >= 1)
        {
            appleButton.SetActive(true);
            appleImage.SetActive(true);
            appleText.SetActive(true);
            appleText.GetComponent<Text>().text = SaveScript.appleCount.ToString();
        }
        else
        {
            appleButton.SetActive(false);
            appleImage.SetActive(false);
            appleText.SetActive(false);
        }
        //Check Batteries
        if (SaveScript.batteryCount >= 1)
        {
            batteryButton.SetActive(true);
            batteryImage.SetActive(true);
            batteryText.SetActive(true);
            batteryText.GetComponent<Text>().text = SaveScript.batteryCount.ToString();
        }
        else
        {
            batteryButton.SetActive(false);
            batteryImage.SetActive(false);
            batteryText.SetActive(false);
        }

        //Check Shell count
        shellText.GetComponent<Text>().text = SaveScript.shellCount.ToString();
        
        //Check Weapons
        if (SaveScript.hasKnife == true) knifeImage.SetActive(true); 
        else knifeImage.SetActive(false);
        if (SaveScript.hasBat == true) batImage.SetActive(true);
        else batImage.SetActive(false);
        if (SaveScript.hasAxe == true) axeImage.SetActive(true);
        else axeImage.SetActive(false);
        if (SaveScript.hasHandgun == true) handgunImage.SetActive(true);
        else handgunImage.SetActive(false);
        if (SaveScript.hasCrossbow == true) crossbowImage.SetActive(true);
        else crossbowImage.SetActive(false);

        //Check Keys
        if (SaveScript.hasCabinKey == true) cabinKeyImage.SetActive(true);
        else cabinKeyImage.SetActive(false);
        if (SaveScript.hasHouseKey == true) houseKeyImage.SetActive(true);
        else houseKeyImage.SetActive(false);
        if (SaveScript.hasRoomKey == true) roomKeyImage.SetActive(true);
        else roomKeyImage.SetActive(false);
        if (SaveScript.hasL1GateKey == true) roomKeyImage.SetActive(true);
        else roomKeyImage.SetActive(false);
        if (metalPipeImage != null)
        {
            if (SaveScript.hasMetalPipe == true) metalPipeImage.SetActive(true);
            else metalPipeImage.SetActive(false);
        }
        //Check Ammo
        if ( SaveScript.bulletCount >= 1)
        {
            bulletImage.SetActive(true);
            bulletText.SetActive(true);
            bulletText.GetComponent<Text>().text = SaveScript.bulletCount.ToString();
        }
        else
        {
            bulletImage.SetActive(false);
            bulletText.SetActive(false);
        }
        if (SaveScript.boltCount >= 1)
        {
            boltImage.SetActive(true);
            boltText.SetActive(true);
            boltText.GetComponent<Text>().text = SaveScript.boltCount.ToString();
        }
        else
        {
            boltImage.SetActive(false);
            boltText.SetActive(false);
        }
    }

    public void HealthUpdate() //used to update our health when using something that restores it. Currently hardcoded for Apple. Need to add water bottle and first aid kit 
    {
        if (SaveScript.playerHealth < 100)
        {
            if (SaveScript.playerHealth + 25 > 100) //make sure we don't go over 100 health
            {
                SaveScript.playerHealth = 100;
            }
            else
            {
                SaveScript.playerHealth += 25; //add 25 health

            }
            SaveScript.healthChanged = true; //this signals to update the health graphic in HUD
            SaveScript.appleCount -= 1; //subracts 1 apple
            soundPlayer.clip = appleBite;
            soundPlayer.Play();
        }
        else
        {
            //play error sound
        }
        checkInventory(); //update out inventory again to reflect the -1 apple
    }
    public void BatteryUpdate() //handles using a new battery
    {
       
        if (SaveScript.batteryPower < 1.0f)
        {
            SaveScript.batteryRefill = true;
            SaveScript.batteryPower = 1.0f;
            SaveScript.batteryCount -= 1;
            soundPlayer.clip = batterySwap;
            soundPlayer.Play();
        }
        else
        {
            //play error sound
        }
        checkInventory(); //update out inventory again to reflect the -1 apple
    }

    public void equipKnife() //equip the knife
    {
        dequipAll(); //put away all weapons that may be out
        armsObject.SetActive(true); //show the arms if they already arent
        armsKnife.SetActive(true); //show the knife object held by arms
        soundPlayer.clip = weaponChange; //load a sound for weapon swaps
        soundPlayer.Play();//play it
        animHands.SetBool("Melee", true); //used to track if we are doing melee or not
        SaveScript.knifeEquiped = true; //track that we are holding the knife
        SaveScript.batEquiped = false;
        SaveScript.axeEquiped = false;
        SaveScript.gunEquiped = false;
        SaveScript.crossbowEquiped = false;
        gunUIImage.SetActive(false); //turn off UI images related to the gun
        bulletAmountText.SetActive(false); //turn off UI images related to the gun
        crossbowUIImage.SetActive(false); //turn off UI images related to the crossbow
        boltAmountText.SetActive(false); //turn off UI images related to the crossbow
    }
    public void equipBat() //same as knife, but for bat
    {
        dequipAll();
        armsObject.SetActive(true);
        armsBat.SetActive(true);
        soundPlayer.clip = weaponChange;
        soundPlayer.Play();
        animHands.SetBool("Melee", true);
        SaveScript.knifeEquiped = false;
        SaveScript.batEquiped = true;
        SaveScript.axeEquiped = false;
        SaveScript.gunEquiped = false;
        SaveScript.crossbowEquiped = false;
        gunUIImage.SetActive(false);
        bulletAmountText.SetActive(false);
        crossbowUIImage.SetActive(false);
        boltAmountText.SetActive(false);
    }
    public void equipAxe() //same as knife, but for the axe
    {
        dequipAll();
        armsObject.SetActive(true);
        armsAxe.SetActive(true);
        soundPlayer.clip = weaponChange;
        soundPlayer.Play();
        animHands.SetBool("Melee", true);
        SaveScript.knifeEquiped = false;
        SaveScript.batEquiped = false;
        SaveScript.axeEquiped = true;
        SaveScript.gunEquiped = false;
        SaveScript.crossbowEquiped = false;
        gunUIImage.SetActive(false);
        bulletAmountText.SetActive(false);
        crossbowUIImage.SetActive(false);
        boltAmountText.SetActive(false);
    }
    public void equipGun() //equip the gun and show related HUD stuff
    {
        {
            dequipAll();
            armsObject.SetActive(true);
            armsGun.SetActive(true);
            soundPlayer.clip = unholsterSound;
            soundPlayer.Play();
            animHands.SetBool("Melee", false);
            SaveScript.knifeEquiped = false;
            SaveScript.batEquiped = false;
            SaveScript.axeEquiped = false;
            SaveScript.gunEquiped = true;
            SaveScript.crossbowEquiped = false;
            gunUIImage.SetActive(true);
            bulletAmountText.SetActive(true);
            bulletAmountText.GetComponent<Text>().text = SaveScript.bulletCount + "";
            crossbowUIImage.SetActive(false);
            boltAmountText.SetActive(false);
        }
    }
    public void equipCrossbow() //equip the crossbow and show related HUD stuff
    {
        dequipAll();
        armsObject.SetActive(true);
        armsCrossbow.SetActive(true);
        soundPlayer.clip = unholsterSound;
        soundPlayer.Play();
        animHands.SetBool("Melee", false);
        SaveScript.knifeEquiped = false;
        SaveScript.batEquiped = false;
        SaveScript.axeEquiped = false;
        SaveScript.gunEquiped = false;
        SaveScript.crossbowEquiped = true;
        gunUIImage.SetActive(false);
        bulletAmountText.SetActive(false);
        boltAmountText.GetComponent<Text>().text = SaveScript.boltCount + "";
        crossbowUIImage.SetActive(true);
        boltAmountText.SetActive(true);
    }
    public void dequipAll() // puts all weapons away
    {
        armsAxe.SetActive(false);
        armsBat.SetActive(false);
        armsKnife.SetActive(false);
        armsGun.SetActive(false);
        armsCrossbow.SetActive(false);
    }
    public void BuildObjectiveList() //used to compile all active objects to display on the objective panel of the inventory
    {
        string textBlob = ""; //initialize what is written in the objective text object
        for (int i = 0; i < objHandler.objectiveStatuses.Length; i++) //loop thorugh objectives
        {
            if (objHandler.objectiveStatuses[i] == 1) //if objective is active (1)...
            {
                textBlob = textBlob + objHandler.objectiveTexts[i] + "\n"; //add a line to the objective text blob with the objective
            }
        }
        objText.text = textBlob; //set the objective text field to show all the objectives we just compiled into one big string.
    }
}
