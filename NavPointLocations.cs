//PURPOSE: Used to manage attributes related to nav targets for enemies. This also includes other random objects needed by enemy nav.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavPointLocations : MonoBehaviour
{
    //nav points
    [SerializeField] public Transform[] level1NavTargets_Set1;
    public static Transform[] NavTarget_level1_Set1;

    //OtherObjectsNeeded By Enemies
    [SerializeField] Transform playerChar;
    public static Transform playerTransform;
    [SerializeField] GameObject chaseMusicObject;
    public static GameObject chaseMusic;
    [SerializeField] GameObject hurtScreenObject;
    public static GameObject hurtScreen;
    [SerializeField] GameObject batBloodObject;
    public static GameObject batBloodSpray;
    [SerializeField] GameObject knifeBloodObject;
    public static GameObject knifeBloodSpray;
    [SerializeField] GameObject axeBloodObject;
    public static GameObject axeBloodSpray;
    [SerializeField] AudioSource stabSoundObject;
    public static AudioSource stabSound;
    [SerializeField] GameObject fpsArmsObject;
    public static GameObject fpsArms;
    [SerializeField] Animator hurtAnimObject;
    public static Animator hurtAnim;


    private void Start()
    {
        NavTarget_level1_Set1 = level1NavTargets_Set1;
        playerTransform = playerChar;
        chaseMusic = chaseMusicObject;
        hurtScreen = hurtScreenObject;
        batBloodSpray = batBloodObject;
        knifeBloodSpray = knifeBloodObject;
        axeBloodSpray = axeBloodObject;
        stabSound = stabSoundObject;
        fpsArms = fpsArmsObject;
        hurtAnim = hurtAnimObject;
    }
}
