/* 
 * Author: Loh Shau Ern Shaun
 * Date: 25/5/2025
 * Hold enemy hp amount in this code
 * If enemy was hit, do dmg to enemy
 * Knockback enemy based on player attack position
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    // Enemy health value
    public int health = 50;

    // Reference to kb on hurt script
    private KnockbackOnHurt kbOnHurt;

    // When scene is called
    void Awake()
    {
        // Assign the kb on hurt script
        kbOnHurt = GameObject.Find("KnockbackManager").GetComponent<KnockbackOnHurt>();
    }

    // When taking dmg
    public void TakeDamage(int dmg, GameObject sender)
    {
        // Reduce dmg from health
        health -= dmg;
        // If health less than 0
        if (health <= 0)
        {
            // Destroy enemy object
            Destroy(gameObject);
        }
        else
        {
            // Play the kb on hurt effect with player and hurting obj as arguments
            kbOnHurt.HurtKnockback(gameObject, sender);
        }
    }
}
