//Used to freeze the player controls for a specificed amount of time. Can be attached to object with trigger collider
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class FreezeControls : MonoBehaviour
{
    [SerializeField] FirstPersonController fpsController; //player character controller
    [SerializeField] float freezeLength; //how long to freeze controls
    [SerializeField] bool oneShot; //whatever this is attached to should be disabled after controls freeze and unfreeze if this is true.
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartFreeze();
        }
    }
    public void StartFreeze()
    {
        fpsController.enabled = false;
        StartCoroutine(WaitForIt());
    }

    IEnumerator WaitForIt()
    {
        yield return new WaitForSeconds(freezeLength);
        fpsController.enabled = true;
        if (oneShot == true) gameObject.SetActive(false);
    }

}
