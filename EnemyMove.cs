//PURPOSE: Handles Enemy Movement. THIS IS NOT NEEDED IF USING BLAZEAI.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    private NavMeshAgent nav;
    private Animator anim;
    private Transform theTarget;
    private float distanceToTarget;
    private int targetNumber = 1;
    private int nextTargetNumber;
    private bool isStopped = false;
    [SerializeField] bool randomizeTargets;
    private Transform[] targets;
    [SerializeField] float stopDistance = 2.0f;
    [SerializeField] int waitTime = 2;
    private bool booted = false;

    void Start()
    {
        nav = GetComponent<NavMeshAgent>(); //get enemy nav mesh agent
        anim = GetComponent<Animator>(); //get enemy animator
        StartCoroutine(startElements());  //initialize things
    }
    public void OnEnable() //when the enemy is enabled
    {
        theTarget = NavPointLocations.NavTarget_level1_Set1[Random.Range(1, NavPointLocations.NavTarget_level1_Set1.Length)];   //set the first nav point target. Random in the set of nav points.     
    }
    void Update()
    {
        if (booted == true) //if the enemy is fully intiialized
        {
            distanceToTarget = Vector3.Distance(theTarget.position, this.gameObject.transform.position);  //get the distance of the enemy to the nav point to go to
            if (distanceToTarget > stopDistance) //if distance is greater than the stop distance near the target...
            {
                nav.isStopped = false; //keep the enemy navigating
                nav.SetDestination(theTarget.position); 
                nextTargetNumber = targetNumber;
                anim.SetInteger("State", 0); //change state of animator to walking
                nav.speed = 1.6f; //speed enemy moves...this value is best for natural walking
            }
            if (distanceToTarget < stopDistance) //if we are close to the target...
            {
                nav.isStopped = true; //stop
                anim.SetInteger("State", 1); //change state of animator to standing
                StartCoroutine(lookAround()); //have the enemy look around for a short bit
            }
        }
    }
  
    IEnumerator lookAround() //makes the enemy stop and look around when hitting a nav point
    {
        yield return new WaitForSeconds(waitTime);
        if (isStopped == false)
        {
            isStopped = true;
            if (randomizeTargets == true)
            {
                nextTargetNumber = Random.Range(1, NavPointLocations.NavTarget_level1_Set1.Length);
                if (targetNumber == nextTargetNumber)
                {
                    nextTargetNumber++;
                    if (nextTargetNumber > NavPointLocations.NavTarget_level1_Set1.Length)
                    {
                        nextTargetNumber = nextTargetNumber - 2;
                    }
                }
                theTarget = NavPointLocations.NavTarget_level1_Set1[nextTargetNumber-1];
                
            }
            else
            {
                targetNumber++;
                if (targetNumber > NavPointLocations.NavTarget_level1_Set1.Length) targetNumber = 1;
                theTarget = NavPointLocations.NavTarget_level1_Set1[targetNumber - 1];
            }
            
            yield return new WaitForSeconds(waitTime);
            isStopped = false;
        }
       
    }
    IEnumerator startElements()
    {
        //Delayed start for when spawning enemy, need to wait for other script to fetch navpoints
        yield return new WaitForSeconds(0.2f);
        nav.avoidancePriority = Random.Range(5, 60);
        booted = true;
    }
}
