/* 
 * Author: Loh Shau Ern Shaun, Jaykin Lee
 * Date Created: 13/5/2025
 * Date Updated: 16/6/2025
 * Keeps note of all teleportable rooms
 * Randomly teleports to certain room when initiated
 * Removes room after teleported to
 */

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TeleportPads : MonoBehaviour
{
    // List of all current unused teleporter exits
    public List<GameObject> usableTeleports;

    // List of all spawners in a specific room 
    public new List<GameObject> enemySpawner;

    // Reference camera controller
    private CameraController cameraController;
    //Checks if the player is in combat or not (might be added later)
    //public bool InCombat = false;

    // Number of Enemies that are spawned in
    private int presentEnemies = 0;

    // Awake is called when scene is opened
    void Awake()
    {
        // Find the camera controller
        cameraController = FindObjectOfType<CameraController>();
    }

    // When called to teleport to random room
    public Transform CallWarpRandom()
    {
        // Get total number of roomstest
        int roomNum = usableTeleports.Count;
        Debug.Log("Remaining rooms " + roomNum);

        //Initialize randRoomNum
        int randRoomNum;
        // Check if there is more than 1
        if (roomNum > 1)
        {
            // Get a random number from 1 to number of rooms
            randRoomNum = Random.Range(1, roomNum);
        }
        // If 1 left
        else
        {
            // Set last room
            randRoomNum = 1;
        }

        // Get the corresponding room element
        GameObject childReference = usableTeleports[randRoomNum - 1];
        //Get the room's teleport exit
        GameObject roomLocation = childReference.transform.Find("TP_Exit").gameObject;
        // Move camera borders
        cameraController.SetCameraBorders(roomLocation, false);
        // Remove the room from list
        usableTeleports.Remove(roomLocation);

        // Gets Enemy Spawners from specific room and put them in a list, then spawn enemies
        foreach (Transform child in childReference.transform)
        {
            if (child.CompareTag("Spawner"))
            {
                enemySpawner.Add(child.gameObject);
                // spawns enemies
                child.gameObject.GetComponent<EnemySpawning>();
                presentEnemies += 1;
            }
        }


        if (enemySpawner.Count != 0)//if there are enemy spawners
        {
            // Finds all teleporters, putting them in a list
            GameObject[] teleporters = GameObject.FindGameObjectsWithTag("Teleporter");
            // Locks room until all enemies are cleared (if shops/safe zones are added, will expand on this)
            foreach (GameObject teleporter in teleporters)
            {
                teleporter.SetActive(false);
                Debug.Log("teleporters have been deactivated");
            }
            Debug.Log("InCombat is true");
        }

        // Return the room destination
        return roomLocation.transform;
    }

    // When an enemy dies, their count decreases
    public void ReduceEnemyCount()
    {
        presentEnemies -= 1;
        // If there are no more enemies, runs OnSafe()
        if (presentEnemies == 0)
        {
            OnSafe();
        }
    }

    // Reopens teleporters
    private void OnSafe()
    {
        GameObject[] inactiveteleporters = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (GameObject obj in inactiveteleporters)
        {
            // Only process objects that are not hidden and match the tag
            if (obj.CompareTag("Teleporter") && !obj.activeInHierarchy && obj.hideFlags == HideFlags.None)
            {
                obj.SetActive(true);
            }
        }
    }
}
