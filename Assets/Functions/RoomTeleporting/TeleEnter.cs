/* 
 * Author: Loh Shau Ern Shaun
 * Date: 13/5/2025
 * Teleports player when stepping on this teleporter entrance
 * Takes note of teleport pads
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleEnter : MonoBehaviour
{
    // Reference player point
    public Transform player;
    // Reference to teleport point
    private Transform telepoint;

    // Reference player character
    public GameObject playerChar;
    // Reference teleport manager
    private TeleportPads teleportPads;

    // Awake is called when scene is opened
    void Awake()
    {
        // Find the teleport manager
        teleportPads = FindObjectOfType<TeleportPads>();
    }

    // When entering player tp
    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject);
        // If player
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player Triggered");
            // Get random destination from teleport manager
            telepoint = teleportPads.CallWarpRandom().transform.Find("TP_Exit/TP_Point");
            // Hide player
            playerChar.SetActive(false);
            // Set player character position at new destination
            player.position = telepoint.position;
            // Show player
            playerChar.SetActive(true);
        }

    }

}
