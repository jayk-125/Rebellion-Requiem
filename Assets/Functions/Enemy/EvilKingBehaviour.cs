using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilKingBehaviour : MonoBehaviour
{
    // Saved current health
    private int healthRemaining;
    // Phase change state
    public string currentPhase = "Phase1";
    // Delay between attacks
    public float kingCd = 0.65f;
    // Speed of projectile
    public float projectileSpd = 10f;
    // Max hp value
    public int maxHealth = 750;
    
    // Current attack number
    // 1 is Consecutive Arrows
    // 2 is Lightning Strike
    // 3 is Call of the Knight
    private int currentAtk = 1;

    // Allow king to attack
    private bool canAtk = true;
    // Allow the enemy to shoot
    private bool allowShoot = true;

    // Reference to health script
    private EnemyHealth enemyHealth;
    // Reference to player object
    private GameObject playerRef;
    // Reference to projectile object
    public GameObject projectileObj;
    // Reference to lightning objectile
    public GameObject lightningObj;
    // Reference to enemy list
    public GameObject[] summonEnemyList;
    
    // Reference arrow shot effect
    public ParticleSystem arrowShotEffect;
    // Reference arrow shot sfx
    public AudioSource arrowShotSFX;
    // Reference summon sfx
    public AudioSource enemySummonSFX;

    // Reference to projectile spawner
    public GameObject arrowSpawner;
    // Reference to enemy spawner
    public GameObject enemySpawner;

    // When scene is called
    void Awake()
    {
        // Assign the enemy health script
        enemyHealth = gameObject.GetComponent<EnemyHealth>();
        // Assign the player objects
        playerRef = GameObject.Find("Player");
    }

    // Called every frame
    void Update()
    {
        // Always look at the player
        transform.LookAt(playerRef.transform.position);

        // If king can attack
        if (canAtk)
        {
            // Carry out next atk in cycle
            AtkCycle();
            // Set as cannot attack
            canAtk = false;
            // Wait for a bit before can attack again
            StartCoroutine(KingCooldown());
        }
    }

    // If king is in phase 1
    private void AtkCycle()
    {
        // Look at the player on Y axis only
        Vector3 flatDir = playerRef.transform.position - transform.position;
        flatDir.y = 0f;
        transform.rotation = Quaternion.LookRotation(flatDir);

        Debug.Log(currentAtk);

        // Atk 1
        if (currentAtk == 1)
        {
            // Carry out attack 1
            Attack1();
            // Move to next atk
            currentAtk += 1;
        }
        // Atk 2
        else if (currentAtk == 2)
        {
            // Carry out attack 2
            Attack2();
            // Move to next atk
            currentAtk += 1;
        }
        // Atk 3
        else if (currentAtk == 3)
        {
            // Carry out attack 
            Attack3();
            // Move to next atk
            currentAtk = 1;
        }
    }

    // Carry out attack 1
    private void Attack1()
    {
        // Init number of arrows to shoot
        int arrowNum;
        // If phase 1
        if (currentPhase == "Phase1")
        {
            // Set arrows to shoot as 3
            arrowNum = 3;
        }
        // If phase 2
        else
        {
            // Set arrows to shoot as 5
            arrowNum = 5;
        }

        // Short wait in between firing projectiles
        StartCoroutine(FireProjectile(arrowNum));
    }
    
    // Delay between arrows shot
    private IEnumerator FireProjectile(int arrowNum)
    {
        // Loop to fire each arrow
        for (int i = 0; i < arrowNum; i++)
        {
            // Shoot projectile
            FireProjectile();
            // Play attack cooldown
            yield return new WaitForSeconds(0.33f);
        }
    }

    // Fire a projectile
    private void FireProjectile()
    {
        // Disallow enemy to shoot
        allowShoot = false;

        // Play arrow shot effect
        arrowShotEffect.Play();
        // Play arrow shot sfx
        arrowShotSFX.Play();

        // Fire projectile
        Vector3 shootDirection = (playerRef.transform.position - arrowSpawner.transform.position).normalized;

        GameObject bulletClone = Instantiate(projectileObj, arrowSpawner.transform.position, Quaternion.LookRotation(shootDirection));
        // Stop from copying the movements of parent
        bulletClone.transform.SetParent(null);

        Rigidbody rb = bulletClone.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = shootDirection * projectileSpd;
        }
        Debug.Log("Shots fired!");
        // Remove the turret bullets after 2 seconds (reduces assets loaded)
        Destroy(bulletClone, 4f);
    }

    // Carry out attack 2
    private void Attack2()
    {
        // Init the wind up for lightning strike
        float lightningWindUp;
        // If phase 1
        if (currentPhase == "Phase1")
        {
            // Longer wind up
            lightningWindUp = 2f;
        }
        // If phase 2
        else
        {
            // Shorter wind up
            lightningWindUp = 1.5f;
        }

        // Attack with lightning
        GameObject lightningClone = Instantiate(lightningObj, playerRef.transform.position, Quaternion.identity);
        // Stop from copying the movements of parent
        lightningClone.transform.SetParent(null);

        LightningAtk lightningAtk = lightningClone.GetComponent<LightningAtk>();
        if (lightningAtk != null)
        {
            lightningAtk.lightningWindUp = lightningWindUp;
        }
        

        //Debug.Log("Shots fired!");
        // Remove the turret bullets after 2 seconds (reduces assets loaded)
        Destroy(lightningClone, lightningWindUp + 1f);
    }

    // Carry out attack 3
    private void Attack3()
    {
        // Init the number of enemies to spawn
        int enemyNum;
        // If phase 1
        if (currentPhase == "Phase1")
        {
            // Spawn 1 enemy
            enemyNum = 1;
        }
        // If phase 2
        else
        {
            // Spawn 2 enemies
            enemyNum = 2;
        }

        // Loop to spawn each enemy
        for (int i = 0; i < enemyNum; i++)
        {
            // Play enemy summon sfx
            enemySummonSFX.Play();
            // Get a random number
            int randomNum = Random.Range(0, 3);
            // Summon an enemy
            GameObject spawnedEnemy = Instantiate(summonEnemyList[randomNum], enemySpawner.transform);
            // Stop from copying the movements of parent
            spawnedEnemy.transform.SetParent(null);
            // Short wait in between
            StartCoroutine(ShortDelay());
        }
    }

    // Delay between arrows shot
    private IEnumerator ShortDelay()
    {
        // Play attack cooldown
        yield return new WaitForSeconds(0.33f);
        // Allow enemy to shoot again
        allowShoot = true;
    }

    // Start king cooldown
    private IEnumerator KingCooldown()
    {
        // Play attack cooldown
        yield return new WaitForSeconds(kingCd);
        // Allow enemy to shoot again
        canAtk = true;
    }
}
