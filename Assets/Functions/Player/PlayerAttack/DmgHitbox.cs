/* 
 * Author: Loh Shau Ern Shaun
 * Date: 25/5/2025
 * Hold dmg amount in this code
 * When player hitbox is active, check if enemy was hit
 * If enemy was hit, do dmg to enemy
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DmgHitbox : MonoBehaviour
{
    // Dmg dealt by attack
    public int dmgVal = 25;

    // When something is in hitbox
    private void OnTriggerEnter(Collider other)
    {
        // If it is under enemy class
        if (other.gameObject.CompareTag("Enemy"))
        {
            // Get EnemyHealth script from enemy
            // Carry out take dmg script based on stated dmg
            other.gameObject.GetComponent<EnemyHealth>().TakeDamage(dmgVal, gameObject);
        }
    }
}
