//PURPOSE: Use to disable the animator of whatever this is attached to. DisableAnim function can be called by various things 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAnimator : MonoBehaviour
{
    private Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

public void DisableAnim()
    {
        anim.enabled = false;
    }
}
