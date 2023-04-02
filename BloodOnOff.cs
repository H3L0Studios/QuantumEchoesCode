//PURPOSE: Used to show the blood spray for 0.2 seconds during attacks.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodOnOff : MonoBehaviour
{
    [SerializeField] GameObject blood;
    void Start()
    {
        StartCoroutine(SwitchOff());
    }

    void Update()
    {
        StartCoroutine(SwitchOff());
    }
    IEnumerator SwitchOff()
    {
        yield return new WaitForSeconds(0.2f);
        blood.gameObject.SetActive(false);
    }
}
