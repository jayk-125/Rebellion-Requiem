using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LightningAtk : MonoBehaviour
{
    // Reference to player
    private Transform playerRef;
    // Reference to lightning strike hitbox
    public GameObject hitbox;

    // Wind up time
    public float lightningWindUp = 10f;
    // Current timer
    private float currentTime = 0f;
    // Can chase player
    private bool allowChase = true;

    // Reference to NavMesh agent
    public UnityEngine.AI.NavMeshAgent lightning;

    // Awake is called when 
    void Awake()
    {
        // Find player target in scene
        playerRef = GameObject.Find("Player").transform;
        // Disable hitbox on start
        hitbox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Get the updated target position of the player
        Vector3 playerTarget = playerRef.position;
        // If can chase
        if (allowChase)
        {
            // Lets lightning chase after player
            lightning.SetDestination(playerTarget);
        }

        // Timer for wind up
        if (currentTime < lightningWindUp)
        {
            // Increase timer
            currentTime += Time.deltaTime;
        }
        // Wind up time over
        else
        {
            // Disallow chase
            allowChase = false;
            // Stop the lightning from moving
            lightning.ResetPath();
            // Set hitbox as active
            hitbox.SetActive(true);
        }
    }
}
