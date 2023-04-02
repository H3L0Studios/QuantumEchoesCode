//PURPOSE: NOT USED YET
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablesTracker : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject[] objects;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveObjects()
    {
        //make sure all collectable objects in game get disabled, not destroyed
        //then register them all in the array above
        //then loop through array and send states to playerprefs - 1 = enabled, 0 = disabled
        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i].activeInHierarchy == true)
            {
                PlayerPrefs.SetInt("Object" + i, 1);
            }
        }
    }

    public void LoadObjects()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            if(PlayerPrefs.GetInt("Object"+i,0) == 1)
            {
                objects[i].SetActive(true);
            }
            else
            {
                objects[i].SetActive(false);
            }
        }
    }

    public void DifficultyAdjustment()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i].GetComponent<InteractableAttributes>().GetDifficulty() == 1 && SaveScript.difficulty <= 1) objects[i].SetActive(true);
            if (objects[i].GetComponent<InteractableAttributes>().GetDifficulty() == 2 && SaveScript.difficulty <= 2) objects[i].SetActive(true);
            if (objects[i].GetComponent<InteractableAttributes>().GetDifficulty() == 3 && SaveScript.difficulty <= 3) objects[i].SetActive(true);
            if (objects[i].GetComponent<InteractableAttributes>().GetDifficulty() == 4 && SaveScript.difficulty <= 4) objects[i].SetActive(true);
            //make sure all interactables start as OFF!

            //add enemy difficulty adjustments here
        }
    }
    public void LoadDoorState()
    {
        //for invisible walls and doors to be switched on and off depending on save progress
        //may or may not need to add puzzle states...
        
    }

    public void LoadEnemyState()
    {
        //for loading which enemies are dead or alive
    }
}
