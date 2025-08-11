/* 
 * Author: Loh Shau Ern Shaun, Jaykin Lee
 * Date Created: 13/5/2025
 * Date Updated: 16/6/2025
 * Keeps note of all teleportable rooms
 * Randomly teleports to certain room when initiated
 * Removes room after teleported to
 * 
 * If all rooms finished, move to next scene
 */

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportPads : MonoBehaviour
{
    // List of all current unused teleporter exits
    public List<GameObject> usableTeleports;

    // List of all spawners in a specific room 
    public new List<GameObject> enemySpawner;
    // List of all current teleporters
    public List<GameObject> teleporterList;

    // Reference camera controller
    private CameraController cameraController;
    //Checks if the player is in combat or not (might be added later)
    //public bool InCombat = false;
    // Reference enemies cleared sfx
    public AudioSource enemyClearedSFX;

    // Reference to the current referenced room
    private GameObject roomReference;
    // Number of Enemies that are spawned in
    private int presentEnemies = 0;
    // Current number of rooms
    private int currentRooms = 0;

    // Awake is called when scene is opened
    void Awake()
    {
        // Find the camera controller
        cameraController = FindObjectOfType<CameraController>();

        // For all established rooms
        foreach (GameObject obj in usableTeleports)
        {
            // For each object in rooms
            foreach (Transform tp in obj.transform)
            {
                // If the object has TP tag
                if (tp.CompareTag("TP"))
                {
                    // Add to list of teleporter entrances
                    teleporterList.Add(tp.gameObject);
                    Debug.Log(tp.gameObject);
                }
            }
        }
    }

    // Start is called before the 1st frame
    void Start()
    {
        // Starting room enemies
        CheckForEnemies();
    }

    // Check for enemy spawns in the room
    public void CheckForEnemies()
    {
        // SPAWN ENEMIES IN NEW ROOM STUFF
        // Each room can have multiple spawners
        // If currently has room referenced
        if (roomReference != null)
        {
            // Gets all Enemy Spawners from specific room and put them in a list, then spawn enemies
            foreach (Transform obj in roomReference.transform)
            {
                // If found Spawner object
                if (obj.CompareTag("Spawner"))
                {
                    // Add that Spawner to enemy spawner list
                    enemySpawner.Add(obj.gameObject);
                    // Spawn enemy
                    obj.gameObject.GetComponent<EnemySpawning>().SpawnEnemies();
                    // Increase current enemies for each spawner
                    presentEnemies += 1;
                }
            }

            // If there are enemy spawners
            if (enemySpawner.Count != 0)
            {
                Debug.Log("InCombat is true");
                // Get each teleporter
                foreach (GameObject tp in teleporterList)
                {
                    // Locks room until all enemies are cleared (if shops/safe zones are added, will expand on this)
                    tp.SetActive(false);
                }
                Debug.Log("Teleporters have been deactivated");
            }
            // If no enemy spawners
            else
            {
                Debug.Log("No enemies");
            }
        }
    }

    // When called to teleport to random room
    public Transform CallWarpRandom()
    {
        // Get total number of roomstest
        int roomNum = usableTeleports.Count;
        //Debug.Log("Remaining rooms " + roomNum);

        //Initialize randRoomNum
        int randRoomNum = 0;

        // Check if total rooms moved to is less than 5
        if (currentRooms < 5)
        {
            // Increase rooms visited by 1 
            currentRooms += 1;
            // Check if there is more than 1
            if (roomNum > 1)
            {
                // Get a random number from 1 to number of rooms
                randRoomNum = Random.Range(1, roomNum);
            }
            // If 1 left
            else if (roomNum == 1)
            {
                // Set last room
                randRoomNum = 1;
            }
            // If no rooms left
            else
            {
                // Get current scene number
                int scene = SceneManager.GetActiveScene().buildIndex;
                // Move to next scene
                SceneManager.LoadScene(scene + 1);
            }
        }
        // If moved to different rooms 5 times alr
        else
        {
            // Get current scene number
            int scene = SceneManager.GetActiveScene().buildIndex;
            // Move to next scene
            SceneManager.LoadScene(scene + 1);
        }

            // Get the corresponding room element
            roomReference = usableTeleports[randRoomNum - 1];

        // TELEPORTING PLAYER STUFF
        // Move camera borders
        cameraController.SetCameraBorders(roomReference, false);
        // Find the room's teleport exit point
        GameObject roomLocation = roomReference.transform.Find("TP_Exit").Find("TP_Point").gameObject;
        
        // Remove the room from list
        usableTeleports.Remove(roomReference);

        // Return the room destination
        return roomLocation.transform;
    }

    // When an enemy dies, their count decreases
    public void ReduceEnemyCount()
    {
        // Reduce when enemy dies
        presentEnemies -= 1;
        Debug.Log(presentEnemies);
        // If there are no more enemies, runs OnSafe()
        if (presentEnemies == 0)
        {
            // Reopen teleporters
            OnSafe();
        }
    }

    // Reopens teleporters
    private void OnSafe()
    {
        // Get each teleporter
        foreach (GameObject tp in teleporterList)
        {
            // Reactivate teleporters
            tp.SetActive(true);
        }

        // Remove all spawners
        enemySpawner.Clear();

        // Play enemies cleared audio
        enemyClearedSFX.Play();
    }
}
