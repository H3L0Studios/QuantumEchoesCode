//PURPOSE: Used to disable the enemy attack hit box which is usually attached to an enemy body part or object held by an enemy.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DisableEnemyAttackHitBox : MonoBehaviour
{
    [SerializeField] EnemyWeaponDamage hitBox;
    public void disableAttack()
    {
        hitBox.hitActive = false;
    }
}
