//PURPOSE: Manages what happens when an enemy attacks the player or takes a hit from the player. THIS IS NOT NEEDED ON BLAZE AI ENEMIES. This is if we want to use our homemade enemy AI.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttack : MonoBehaviour
{
    private NavMeshAgent nav;
    private NavMeshHit hit;
    private bool blocked;
    private bool runToPlayer = false;
    private float distanceToPlayer;
    private bool isChecking = true;
    private int failedChecks = 0;
    [SerializeField] EnemyDamage enemyDamage; 
    [SerializeField] Animator anim;
    private Transform playerCharacter;
    [SerializeField] GameObject enemy;
    [SerializeField] float maxRange = 35;
    [SerializeField] int maxChecks = 3;
    [SerializeField] float chaseSpeed = 8.5f;
    [SerializeField] float walkSpeed = 1.6f;
    [SerializeField] float attackDistance = 2.3f;
    [SerializeField] float attackRotateSpeed = 2.0f;
    [SerializeField] float checkTime = 3f;
    [SerializeField] bool hasKnife;
    [SerializeField] bool hasBlunt;
    [SerializeField] bool hasAxe;
    private bool booted = false;
    private GameObject hurtUI;

    void Start()
    {
        nav = GetComponentInParent<NavMeshAgent>();
        StartCoroutine(startElements());
    }

    void Update()
    {
        if (booted == true)
        {
            distanceToPlayer = Vector3.Distance(playerCharacter.position, enemy.transform.position);
            if (distanceToPlayer < maxRange)
            {
                if (isChecking == true)
                {
                    isChecking = false;
                    //send ray from enemy to player to see if there is line of sight
                    blocked = NavMesh.Raycast(transform.position, playerCharacter.position, out hit, NavMesh.AllAreas);

                    if (blocked == false)
                    {
                        // Debug.Log("ICanSeeThePlayer");
                        runToPlayer = true;
                        if (enemyDamage.hasDied == false)
                        {
                            SaveScript.isBeingAttacked = true;
                        }
                        failedChecks = 0;
                    }
                    if (blocked == true)
                    {
                        Debug.Log("Line Of Sight Lost");
                        runToPlayer = false;
                        anim.SetInteger("State", 1);
                        SaveScript.isBeingAttacked = false;
                        failedChecks++;
                    }

                    StartCoroutine(timedCheck());
                }
            }
            if (distanceToPlayer > maxRange)
            {
                runToPlayer = false;
            }
            if (runToPlayer == true)
            {
                enemy.GetComponent<EnemyMove>().enabled = false;
                if (enemyDamage.hasDied == false)
                {
                    SaveScript.isBeingAttacked = true;
                }

                if (distanceToPlayer > attackDistance)
                {
                    nav.isStopped = false;
                    anim.SetInteger("State", 2);
                    nav.acceleration = 24;
                    nav.SetDestination(playerCharacter.position);
                    nav.speed = chaseSpeed;
                    hurtUI.SetActive(false);
                }
                if (distanceToPlayer < attackDistance - 0.5f)
                {
                    nav.isStopped = true;
                    if (hasAxe == true)
                    {
                        anim.SetInteger("State", 3);
                    }
                    if (hasBlunt == true)
                    {
                        anim.SetInteger("State", 4);
                    }
                    if (hasKnife == true)
                    {
                        anim.SetInteger("State", 5);
                    }
                    //Debug.Log("Attacking!");
                    nav.acceleration = 180;
                    Vector3 pos = (playerCharacter.position - enemy.transform.position).normalized;
                    Quaternion posRotation = Quaternion.LookRotation(new Vector3(pos.x, 0, pos.z));
                    enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, posRotation, Time.deltaTime * attackRotateSpeed);
                    //still doesnt turn properly all the time....
                    hurtUI.SetActive(true);
                }
            }
            if (runToPlayer == false)
            {
                nav.isStopped = false;
                enemy.GetComponent<EnemyMove>().enabled = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            runToPlayer = true;
        }
        if (other.gameObject.CompareTag("P_Knife"))
        {
            anim.SetTrigger("LightHit");
        }
        if (other.gameObject.CompareTag("P_Axe"))
        {
            anim.SetTrigger("BigHit");
        }
        if (other.gameObject.CompareTag("P_Crossbow"))
        {
            anim.SetTrigger("BigHit");
        }
        if (other.gameObject.CompareTag("P_Bat"))
        {
            anim.SetTrigger("LightHit");
        }
    }


    IEnumerator timedCheck()
    {
        yield return new WaitForSeconds(checkTime);
        isChecking = true;
        if (failedChecks > maxChecks)
        {
            enemy.GetComponent<EnemyMove>().enabled = true;
            nav.isStopped = false;
            nav.speed = walkSpeed;
            failedChecks = 0;
            SaveScript.isBeingAttacked = false;
        }
    }
    IEnumerator startElements()
    {
        yield return new WaitForSeconds(0.1f);
        playerCharacter = NavPointLocations.playerTransform;
        hurtUI = NavPointLocations.hurtScreen;
        booted = true;
    }

}
