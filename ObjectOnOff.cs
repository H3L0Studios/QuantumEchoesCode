//PURPOSE: Generic functions can be called from various places to turn things on and off
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectOnOff : MonoBehaviour
{
    [SerializeField] GameObject[] thingsToEnable;
    [SerializeField] GameObject[] thingsToDisable;
    public void TurnOn()
    {
        for (int i = 0; i < thingsToEnable.Length; i++)
        {
            thingsToEnable[i].SetActive(true);
        }
    }
    public void TurnOff()
    {
        for (int i = 0; i < thingsToDisable.Length; i++)
        {
            thingsToDisable[i].SetActive(true);
        }
    }
}
