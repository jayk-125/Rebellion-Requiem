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
    // Reference to knight enemy chase script
    private RushEnemyChase rushEnemyChase;
    // Reference to spearman range enemy chase script
    private LongEnemyChase longEnemyChase;
    // Reference to archer enemy chase script
    private RangedEnemyChase rangedEnemyChase;
    // Reference to evil king behaviour script
    private EvilKingBehaviour evilKingBehaviour;
    // Reference to player health script
    private PlayerHealth playerHealth;
    // Reference to teleport pads
    private TeleportPads teleportPads;
    // Reference to enemy dmg sfx
    public AudioSource enemyDmgSFX;

    // When scene is called
    void Awake()
    {
        // Assign the kb on hurt script
        kbOnHurt = GameObject.Find("KnockbackManager").GetComponent<KnockbackOnHurt>();
        // Assign the player health script
        playerHealth = GameObject.Find("Player").GetComponent<PlayerHealth>();
        // Assign the teleport pads script
        teleportPads = GameObject.Find("TeleportManager").GetComponent<TeleportPads>();
    }

    // When taking dmg
    public void TakeDamage(int dmg, GameObject sender)
    {
        // Play enemy dmg sfx
        enemyDmgSFX.Play();
        // Reduce dmg from health
        health -= dmg;
        // If health less than 0
        if (health <= 0)
        {
            // Heal player
            playerHealth.PlayerHeal();
            // Remove 1 enemy from the enemy count
            teleportPads.ReduceEnemyCount();
            // Destroy enemy object
            Destroy(gameObject);
            Debug.Log("Destroyed");
        }
        else
        {
            // Find the corresponding scripts for all enemy types
            rushEnemyChase = gameObject.GetComponent<RushEnemyChase>();
            longEnemyChase = gameObject.GetComponent<LongEnemyChase>();
            rangedEnemyChase = gameObject.GetComponent<RangedEnemyChase>();
            evilKingBehaviour = gameObject.GetComponent<EvilKingBehaviour>();
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
            else if (evilKingBehaviour != null)
            {
                int halfHp = evilKingBehaviour.maxHealth / 2;
                // Check if below half
                if (health <= halfHp)
                {
                    // Set the change phase as true
                    evilKingBehaviour.currentPhase = "Phase2";
                }
            }
            // Play the kb on hurt effect with player and hurting obj as arguments
            kbOnHurt.HurtKnockback(gameObject, sender);
        }
    }
}
