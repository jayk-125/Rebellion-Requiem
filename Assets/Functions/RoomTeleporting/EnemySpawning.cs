/*
* Author: Jaykin Lee
* Date: 17/6/2025
* Spawns a random enemy from a pre-set list when called
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawning : MonoBehaviour
{
    // List contains all the different enemies that can be spawned
    [SerializeField] private List<GameObject> enemiesToSpawn;
    // Enemy spawn warning sfx
    public AudioSource enemyWarningSFX;
    // Enemy spawn sfx
    public AudioSource enemySpawnSFX;
    // Enemy spawn warning effect
    public ParticleSystem spawnWarningEffect;
    // Spawn enemy effect
    public ParticleSystem spawnEnemyEffect;

    // When called, spawn enemies
    public void SpawnEnemies()
    {
        // Verify that list has all potential enemies
        if (enemiesToSpawn != null && enemiesToSpawn.Count > 0)
        {
            // Play delay
            StartCoroutine(WarningDelay());
        }
        else
        {
            Debug.LogWarning("List is empty or null.");
        }
    }

    // Delay initial spawning
    private IEnumerator WarningDelay()
    {
        // Play spawn warning effect
        spawnWarningEffect.Play();
        // Play spawn warning sfx
        enemyWarningSFX.Play();
        // Delay for 1.2 seconds
        yield return new WaitForSeconds(1.2f);
        // Spawn enemy
        SpawnEnemy();
    }
        
    // Spawn the enemy
    private void SpawnEnemy() 
    { 
        // Get a random enemy from list
        GameObject randomEnemy = enemiesToSpawn[Random.Range(0, enemiesToSpawn.Count)];
        // Spawn the enemy at the spawner's location
        Instantiate(randomEnemy, this.transform.position, Quaternion.identity);
        Debug.Log("Randomly selected: " + randomEnemy.name);

        // Play effect
        spawnEnemyEffect.Play();
        // Play sound
        enemySpawnSFX.Play();
        
    }


}
