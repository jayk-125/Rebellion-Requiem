/* 
 * Author: Loh Shau Ern Shaun
 * Date: 30/5/2025
 * Handles AI for the ranged enemy
 * Enemy will stop to attack when far away enough from player
 * Enemy runs away from player when they are too close
 * Also handles enemy stunning
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedEnemyChase : MonoBehaviour
{
    // Reference to NavMesh agent
    public UnityEngine.AI.NavMeshAgent enemy;
    // Reference to player (assigned in awake)
    private Transform playerTarget;

    // Reference to projectile object
    public GameObject projectileObj;
    // Reference to projectile spawner
    public GameObject spawner;

    // Position for enemy to run to
    public Vector3 tempNewPos;

    // Enemy allowed to shoot player bool
    private bool allowShoot = true;
    // Enemy allowed to flee from player
    private bool allowFlee = true;
    // Check if enemy is alr attacking
    private bool isAttacking = false;

    // Attack distance value
    public float distanceAway = 5f;
    // Time before attack
    public float attackStartup = 1f;
    // Time attack is on cd after attack
    public float attackCooldown = 1f;
    // Speed of projectile
    public float projectileSpd = 10f;

    // Awake is called before the first frame update
    void Awake()
    {
        // Find player target in scene
        playerTarget = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    private void Update()
    {
        // Get position of player on same lvl as enemy
        Vector3 flatPos = new Vector3(playerTarget.position.x, transform.position.y, playerTarget.position.z);
        // Always look at player
        transform.LookAt(flatPos);
        
        // If agent can shoot player
        if (allowShoot)
        {

            // Get the current distance from the enemy to the player
            float distance = Vector3.Distance(gameObject.transform.position, playerTarget.transform.position);
            // If distance is far enough from player
            if (distance >= distanceAway)
            {
                //Debug.Log("Attack allowed");
                // Stop enemy at current place
                enemy.isStopped = true;
                // Start enemy attack
                EnemyAttack();
            }
            // If too close to player
            else if (allowFlee)
            {
                // Pick a random point within a range
                Vector3 randomDirection = Random.insideUnitSphere * 5f + transform.position;
                // Init NavMeshHit
                UnityEngine.AI.NavMeshHit hit;
                // Check if area can be moved to within range
                if (UnityEngine.AI.NavMesh.SamplePosition(randomDirection, out hit, 5f, 1))
                {
                    // If new distance is far enough from player
                    if (Vector3.Distance(playerTarget.position, hit.position) >= 3f)
                    {
                        // Set variable position
                        tempNewPos = hit.position;
                        // Disallow enemy to shoot
                        allowShoot = false;
                        // Disallow enemy to run (and repick a new running location)
                        allowFlee = false;
                        Debug.DrawLine(transform.position, tempNewPos, Color.blue, 5f);
                    }
                }
            }
        }
        // If enemy cannot flee
        if (!allowFlee)
        {
            Debug.Log(tempNewPos);
            // Move to new area
            enemy.SetDestination(tempNewPos);
            // Until enemy finishes moves to that area
            if (transform.position == tempNewPos)
            {
                // Allow to flee
                allowFlee = true;
                // Allow to shoot
                allowShoot = true;
            }
        }
    }

    // Start enemy attacking
    private void EnemyAttack()
    {
        // Stop this if already attacking
        if (isAttacking)
        {
            return;
        }
        //Debug.Log("Attacking");
        // Set enemy as alr attacking
        isAttacking = true;
        // Disallow from shooting
        allowShoot = false;
        // Disallow from fleeing
        allowFlee = false;
        // Attack windup
        StartCoroutine(RangedEnemyWindUp());
    }

    // Start enemy windup
    private IEnumerator RangedEnemyWindUp()
    {
        //Debug.Log("Windup");
        // Play attack startup
        yield return new WaitForSeconds(attackStartup);
        // Start enemy attack
        RangedEnemyAtk();
    }

    // Start enemy attack
    private void RangedEnemyAtk()
    {
        // Look at the player on Y axis only
        Vector3 flatDir = playerTarget.position - transform.position;
        flatDir.y = 0f;
        transform.rotation = Quaternion.LookRotation(flatDir);

        // Fire projectile
        Vector3 shootDirection = (playerTarget.position - spawner.transform.position).normalized;

        GameObject bulletClone = Instantiate(projectileObj, spawner.transform.position, Quaternion.LookRotation(shootDirection));

        Rigidbody rb = bulletClone.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = shootDirection * projectileSpd;
        }
        Debug.Log("Shots fired!");
        // Remove the turret bullets after 2 seconds (reduces assets loaded)
        Destroy(bulletClone, 4f);
        // Start enemy cooldown
        StartCoroutine(RangedEnemyCd());
    }

    // Start enemy cooldown
    private IEnumerator RangedEnemyCd()
    {
        // Play attack cooldown
        yield return new WaitForSeconds(attackCooldown);
        // Allow enemy to shoot again
        allowShoot = true;
        // Allow enemy to flee again
        allowFlee = true;
        // Enemy is not attacking right now anymore
        isAttacking = false;
        // Ai can continue to move
        enemy.isStopped = false;
    }

    // Stunned enemy
    public IEnumerator RangedEnemyStunned(float stunnedTime)
    {
        // Stop shooting
        allowShoot = false;
        // Stop fleeing
        allowFlee = false;
        // Cannot attack
        isAttacking = true;
        // Ai cannot move
        enemy.isStopped = true;
        // Stop attack process
        StopAllCoroutines();
        // Wait for a while
        yield return new WaitForSeconds(stunnedTime);
        // Ai can continue to move
        enemy.isStopped = false;
        // Continue shooting
        allowShoot = true;
        // Continue fleeing
        allowFlee = true;
        // Allow attack again
        isAttacking = false;
    }
}
