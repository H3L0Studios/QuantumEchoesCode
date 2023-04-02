//PURPOSE: Currently used to store detrails on specific interactable objects. Currently only used for notes. Expandable for difficulty levels later.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableAttributes : MonoBehaviour
{
    public bool available = true;
    [Header("Difficulty Settings")]
    [SerializeField] bool Brutal;
    [SerializeField] bool Stressful;
    [SerializeField] bool Easy;
    [SerializeField] bool Story;
    [Header("Note Settings")]
    [SerializeField] int NoteNumber; //we can use this to track an achievment
    [SerializeField] public GameObject noteToEnable;

    public int GetDifficulty()
    {
        if (Brutal == true) return 4;
        else if (Stressful == true) return 3;
        else if (Easy == true) return 2;
        else if (Story == true) return 1;
        else return 0;
    }
}
