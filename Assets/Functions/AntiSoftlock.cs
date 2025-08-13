using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiSoftlock : MonoBehaviour
{
    // Reference to teleport pads
    private TeleportPads teleportPads;
    // Reference to camera
    private CameraController cameraController;
    // Reference to player
    private GameObject player;

    // First room reference
    public GameObject firstRoomTP;
    // Current room saved
    private GameObject currentRoomTP;

    // Awake is called when the scene first starts
    void Awake()
    {
        // Assign player object
        player = GameObject.Find("Player");
        // Assign camera controller script
        cameraController = GameObject.Find("Player Camera").GetComponent<CameraController>();
        // Assign teleport pads script from scene
        teleportPads = GameObject.Find("TeleportManager").GetComponent<TeleportPads>();
    }

    // When touching object with this script
    private void OnTriggerEnter(Collider other)
    {
        // If it is under player class
        if (other.gameObject.CompareTag("Player"))
        {
            // Get current room from teleport pads script
            GameObject currentRoom = teleportPads.roomReference;

            // Check if the current room is not empty
            if (currentRoom != null)
            {
                // Get TP point of current room
                currentRoomTP = currentRoom.transform.Find("TP_Exit").Find("TP_Point").gameObject;
            }
            // If current room is null
            else
            {
                // Set current room tp as first room tp
                currentRoomTP = firstRoomTP;
            }

            GetUnstuck();
        }
    }

    // Player warp to safety
    private void GetUnstuck()
    {
        // TELEPORT PLAYER BACK
        // Reset player momentum
        Rigidbody rb = player.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;

        // Warp player camera into position
        cameraController.MoveCamera(currentRoomTP.transform.position);

        // Warp player back to room start
        player.transform.position = currentRoomTP.transform.position;
    }
}
