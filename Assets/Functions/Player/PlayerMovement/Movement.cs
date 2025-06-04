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
    // Takes all directional inputs
    private Vector2 move;

    // When receiving input from player
    public void OnMove(InputAction.CallbackContext context)
    {
        // Obtain direction based on player input
        move = context.ReadValue<Vector2>();
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
            Debug.Log("Stopped");
        }
    }

    // Every frame, move the player
    public void movePlayer()
    {
        // Set the movement direction in 3D space to be based on directional influence
        Vector3 movement = new Vector3(move.x, 0f, move.y);

        // Only when player input is detected
        if (movement != Vector3.zero)
        {
            // Make character face direction they're moving in, with a turning speed relative to player movement speed
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), speed/35);
        }
        
        // Translate directional movement in character movement (direction * speed)
        transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }

    // When called, stop player from moving
    public IEnumerator DisableMovement(float time)
    {
        Debug.Log("Disabling for " + time);
        // Disallow taking dmg
        allowMove = false;
        // Wait for a little
        yield return new WaitForSeconds(time);
        // Allow taking dmg again
        allowMove = true;
        Debug.Log("Continue");
    }
}
