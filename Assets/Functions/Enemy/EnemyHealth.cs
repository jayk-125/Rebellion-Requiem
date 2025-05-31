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
    // Reference to rush enemy chase script
    private RushEnemyChase rushEnemyChase;
    // Reference to mid range enemy chase script
    private LongEnemyChase longEnemyChase;
    // Reference to kb on hurt script
    private RangedEnemyChase rangedEnemyChase;

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
            // Find the corresponding scripts for all enemy types
            rushEnemyChase = gameObject.GetComponent<RushEnemyChase>();
            longEnemyChase = gameObject.GetComponent<LongEnemyChase>();
            rangedEnemyChase = gameObject.GetComponent<RangedEnemyChase>();
            // Find the script that was not empty
            if (rushEnemyChase != null)
            {
                // Stun for 0.5 seconds
                rushEnemyChase.RushEnemyStunned(0.5f);
            }
            else if (longEnemyChase != null)
            {
                // Stun for 0.5 seconds
                longEnemyChase.LongEnemyStunned(0.5f);
            } 
            else if (rangedEnemyChase != null)
            {
                // Stun for 0.5 seconds
                rangedEnemyChase.RangedEnemyStunned(0.5f);
            }
            // Play the kb on hurt effect with player and hurting obj as arguments
            kbOnHurt.HurtKnockback(gameObject, sender);
        }
    }
}
