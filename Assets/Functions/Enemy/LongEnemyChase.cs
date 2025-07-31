/* 
 * Author: Loh Shau Ern Shaun
 * Date: 30/5/2025
 * Handles AI for the long attack enemy
 * Enemy will stop to attack when near enough player
 * Also handles enemy stunning
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LongEnemyChase : MonoBehaviour
{
    // Reference to NavMesh agent
    public UnityEngine.AI.NavMeshAgent enemy;
    // Reference to player (assigned in awake)
    private Transform playerTarget;
    // Reference to enemy attack hitbox
    public GameObject hitbox;
    // Reference stab particle
    public ParticleSystem stabEffect;
    // Reference woosh particle
    public ParticleSystem wooshEffect;
    // Reference stab sfx
    public AudioSource stabSFX;


    // Enemy allowed to chase player bool
    private bool allowChase = true;
    // Attack distance value
    public float distanceAway = 2f;
    // Time before attack
    public float attackStartup = 0.75f;
    // Time attack can do dmg for
    public float attackActive = 0.5f;
    // Time attack is on cd after attack
    public float attackCooldown = 1f;

    // Awake is called before the first frame update
    void Awake()
    {
        // Find player target in scene
        playerTarget = GameObject.Find("Player").transform;
        // Hide the attack hitbox
        hitbox.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        // Get the current distance from the enemy to the player
        float distance = Vector3.Distance(gameObject.transform.position, playerTarget.transform.position);

        // If agent can chase player
        if (allowChase)
        {
            // Allow enemy to move
            enemy.isStopped = false;

            // Lets enemy chase after player
            enemy.SetDestination(playerTarget.position);
            // Adjust player target on same level
            Vector3 flatTarget = new Vector3(playerTarget.position.x, 0f, playerTarget.position.z);
            // Always look at the player
            transform.LookAt(flatTarget);
            // If distance is near to player
            if (distance <= distanceAway)
            {
                // Stop enemy at current place
                enemy.SetDestination(this.transform.position);
                // Start enemy attack
                EnemyAttack();
            }
        }
        // If cannot move
        else
        {
            // Stop the enemy from moving
            enemy.ResetPath();
        }
    }

    // Start enemy attacking
    private void EnemyAttack()
    {
        // Disallow player from chasing
        allowChase = false;
        // Pause other Coroutines
        StopAllCoroutines();
        // Attack windup
        StartCoroutine(LongEnemyWindUp());
    }

    // Start enemy windup
    private IEnumerator LongEnemyWindUp()
    {
        // Play attack startup
        yield return new WaitForSeconds(attackStartup);
        // Start enemy attack
        StartCoroutine(LongEnemyAtk());
    }

    // Start enemy attack
    private IEnumerator LongEnemyAtk()
    {
        // Show atk hitbox
        hitbox.SetActive(true);

        // Play stab effect
        stabEffect.Play();
        // Play woosh effect
        wooshEffect.Play();
        // Play stab sfx
        stabSFX.Play();

        // Play attack atk
        yield return new WaitForSeconds(attackActive);
        // Hide atk hitbox
        hitbox.SetActive(false);
        // Start enemy cooldown
        StartCoroutine(LongEnemyCd());
    }

    // Start enemy cooldown
    private IEnumerator LongEnemyCd()
    {
        // Play attack cooldown
        yield return new WaitForSeconds(attackCooldown);
        // Allow enemy to chase again
        allowChase = true;
    }

    // Stunned enemy
    public IEnumerator LongEnemyStunned(float stunnedTime)
    {
        // Stop chasing
        allowChase = false;
        // Hide atk hitbox
        hitbox.SetActive(false);
        // Stop attack process
        StopAllCoroutines();
        // Wait for a while
        yield return new WaitForSeconds(stunnedTime);
        // Continue chasing
        allowChase = true;
    }
}
