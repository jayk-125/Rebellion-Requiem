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
using UnityEngine;

public class TeleportPads : MonoBehaviour
{
    // List of all current unused teleporter exits
    public List<GameObject> usableTeleports;
    // Reference camera controller
    private CameraController cameraController;
    //Checks if the player is in combat or not (might be added later)
    //public bool InCombat = false;

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
        GameObject roomLocation = usableTeleports[randRoomNum - 1];
        // Move camera borders
        cameraController.SetCameraBorders(roomLocation, false);
        // Remove the room from list
        usableTeleports.Remove(roomLocation);

        // Finds all teleporters, putting them in a list
        GameObject[] teleporters = GameObject.FindGameObjectsWithTag("Teleporter");
        // Locks room until all enemies are cleared (if shops/safe zones are added, will expand on this)
        foreach (GameObject teleporter in teleporters)
        {
            teleporter.SetActive(false);
            Debug.Log("teleporters have been deactivated");
        }


        Debug.Log("InCombat is true");
        // Return the room destination
        return roomLocation.transform;
    }
    public void OnSafe()
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
