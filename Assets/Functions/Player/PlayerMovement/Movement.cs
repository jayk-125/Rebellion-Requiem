/* 
 * Author: Loh Shau Ern Shaun
 * Date: 13/5/2025
 * Handles the player movement inputs
 * Player character faces the direction of movement
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    // Controls speed of character
    public float speed = 10f;
    // Allow player movement
    public bool allowMove = true;
    // Bool for player moving
    private bool isMoving = false;
    // Allow player facing direction
    private bool allowFace = true;
    // Takes all directional inputs
    private Vector2 move;

    // Reference to animation scripts
    // Butcher animations
    public ButcherAnim butcherAnim;

    // When receiving input from player
    public void OnMove(InputAction.CallbackContext context)
    {
        // While player is pressing movement
        if (context.phase == InputActionPhase.Performed)
        {
            // Obtain direction based on player input
            move = context.ReadValue<Vector2>();
            // Set bool is moving
            isMoving = true;
        }
        // When player releases movement
        else if (context.phase == InputActionPhase.Canceled)
        {
            // Stop player movement
            move = Vector2.zero; 
            // Set bool is not moving
            isMoving = false;
            // Set butcher as idling
            butcherAnim.ButcherIdle();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // If moving allowed
        if (allowMove)
        {
            // Move the player every frame
            movePlayer();
        }
        else
        {
            //Debug.Log("Stopped");
        }

        // Check if isMoving
        if (isMoving)
        {
            // Set butcher as running
            butcherAnim.ButcherRun();
        }
    }

    // Every frame, move the player
    public void movePlayer()
    {
        // Set the movement direction in 3D space to be based on directional influence
        Vector3 movement = new Vector3(move.x, 0f, move.y);

        // When player can face direction
        if (allowFace)
        {
            // Only when player input is detected
            if (movement != Vector3.zero)
            {
                // Make character face direction they're moving in, with a turning speed relative to player movement speed
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), speed/25);
            }
        }
        
        // Translate directional movement in character movement (direction * speed)
        transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }

    // When called, stop player from moving
    public void DisableMovement()
    {
        // Disallow moving
        allowMove = false;
    }
    // When called, allow player to move
    public void EnableMovement()
    {
        // Allow moving
        allowMove = true;
    }
    
    // When called, disable player from facing direction
    public void DisableFacing()
    {
        allowFace = false;
    }
    // When called, enable player to 
    public void EnableFacing()
    {
        allowFace = true;
    }
}

 