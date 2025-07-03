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
    //list of enemies to spawn
    [SerializeField] private List<GameObject> enemiesToSpawn;

    public void SpawnEnemies()
    {
        if (enemiesToSpawn != null && enemiesToSpawn.Count > 0)
        {
            GameObject randomEnemy = enemiesToSpawn[Random.Range(0, enemiesToSpawn.Count)];
            Instantiate(randomEnemy, this.transform.position, Quaternion.identity);
            Debug.Log("Randomly selected: " + randomEnemy.name);
        }
        else
        {
            Debug.LogWarning("List is empty or null.");
        }
    }
}
