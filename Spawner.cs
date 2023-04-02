//PURPOSE: Can be used to spawn up to 3 enemies from 3 different locations. gets triggered when a player walks through a trigger collider on something.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject enemySpawn1;
    [SerializeField] GameObject enemySpawn2;
    [SerializeField] GameObject enemySpawn3;
    [SerializeField] Transform spawnPoint1;
    [SerializeField] Transform spawnPoint2;
    [SerializeField] Transform spawnPoint3;
    private bool canSpawn = true;
    [SerializeField] bool retriggerable = false; //Can this be triggered multiple times...use with caution. At least set a long spawn wait time.

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("Player")) //when player collides with this trigger collider...
        {
            if (SaveScript.enemiesOnScreen < SaveScript.maxEnemys) //make sure we havent hit max enemies
            {
                if (canSpawn == true) //make sure we arent waiting the grace period to spawn again
                {
                    canSpawn = false; //start the grace period because we are about to spawn enemies again
                    Instantiate(enemySpawn1, spawnPoint1.position, spawnPoint1.rotation); //spawn the first enemy
                    
                    SaveScript.enemiesOnScreen++; //increase enemies counter
                    if (SaveScript.enemiesOnScreen < SaveScript.maxEnemys) //if we havent reached max enemies...
                    {
                        Instantiate(enemySpawn2, spawnPoint2.position, spawnPoint2.rotation); //spawn the 2nd enemy
                        SaveScript.enemiesOnScreen++;
                    }
                    if (SaveScript.enemiesOnScreen < SaveScript.maxEnemys) //if we still havent hit max enemies...
                    {
                        Instantiate(enemySpawn3, spawnPoint3.position, spawnPoint3.rotation); //spawn the 3rd enemy
                        SaveScript.enemiesOnScreen++;
                    }
                    if (retriggerable == true) //if we can trigger this again...
                    {
                        StartCoroutine(WaitToSpawn()); //wait 
                    }
                }
            }
        }
    }
    IEnumerator WaitToSpawn()
    {
        yield return new WaitForSeconds(60f); //wait 60 seconds
        canSpawn = true; //mark spawn to be allowed again.
    }
}
