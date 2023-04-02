//PURPOSE: changes the level on entering a trigger collider. May be unused.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    [SerializeField] int nextScene; //what scene are we going to...
    
    private void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene(nextScene); //on colission go to the next scene
    }
}
