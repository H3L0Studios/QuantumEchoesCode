//PURPOSE: Use to War the field of view into a cone shape. Gives a trippy effect like warpspeed.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVWarp : MonoBehaviour
{
    [SerializeField] Animator warpObj; //get the animator of the object that as the warp animation.

    void Start()
    {
        warpObj.enabled = true; //enable the object causing the warp animation to the camera
    }
}
