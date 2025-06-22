/* 
 * Author: Loh Shau Ern Shaun
 * Date: 22/6/2025
 * Projectile charge dmg
 * Holds base dmg amount in this code
 * Add dmg based on charge
 * 
 * When player hitbox is active, check if enemy was hit
 * If enemy was hit, do dmg to enemy
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeProjectile : MonoBehaviour
{
    // Base dmg dealt by attack
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

            Debug.Log("- " + dmgVal);
        }
    }

    // Add the charging value to the projectile
    public void ChargeDmg(int chargeVal)
    {
        // Add chargeVal to projectile dmg value
        dmgVal += chargeVal;
    }
}
