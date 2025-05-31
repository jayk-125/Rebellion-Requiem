using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RushEnemyChase : MonoBehaviour
{
    // Reference to NavMesh agent
    public UnityEngine.AI.NavMeshAgent enemy;
    // Reference to player (assigned in awake)
    private Transform playerTarget;
    // Reference to enemy attack hitbox
    public GameObject hitbox;


    // Enemy allowed to chase player bool
    private bool allowChase = true;
    // Attack distance value
    public float distanceAway = 2f;
    // Time before attack
    public float attackStartup = 0.5f;
    // Time attack can do dmg for
    public float attackActive = 0.2f;
    // Time attack is on cd after attack
    public float attackCooldown = 0.5f;

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
            // Lets enemy chase after player
            enemy.SetDestination(playerTarget.position);
            // Always look at the player
            transform.LookAt(playerTarget);
            // If distance is near to player
            if (distance <= distanceAway)
            {
                // Stop enemy at current place
                enemy.SetDestination(this.transform.position);
                // Start enemy attack
                EnemyAttack();
            }
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
        StartCoroutine(RushEnemyWindUp());
    }

    // Start enemy windup
    private IEnumerator RushEnemyWindUp()
    {
        // Play attack startup
        yield return new WaitForSeconds(attackStartup);
        // Start enemy attack
        StartCoroutine(RushEnemyAtk());
    }
    
    // Start enemy attack
    private IEnumerator RushEnemyAtk()
    {
        // Show atk hitbox
        hitbox.SetActive(true);
        // Play attack atk
        yield return new WaitForSeconds(attackActive);
        // Hide atk hitbox
        hitbox.SetActive(false);
        // Start enemy cooldown
        StartCoroutine(RushEnemyCd());
    }

    // Start enemy cooldown
    private IEnumerator RushEnemyCd()
    {
        // Play attack cooldown
        yield return new WaitForSeconds(attackCooldown);
        // Allow enemy to chase again
        allowChase = true;
    }

    // Stunned enemy
    public IEnumerator RushEnemyStunned(float stunnedTime)
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
