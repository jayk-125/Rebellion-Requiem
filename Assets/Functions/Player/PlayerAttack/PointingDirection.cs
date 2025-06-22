/* 
 * Author: Loh Shau Ern Shaun
 * Date: 5/6/2025
 * Detects direction of mouse relative from player
 * Split off from original "BasicAtk" script
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointingDirection : MonoBehaviour
{
    // Reference player object
    public GameObject player;

    // Reference movement script
    public Movement movement;

    // Direction of mouse from player
    // Refer to this for normalized player mouse direction
    public Vector3 pointDir;
    // Reference player camera
    public Camera cam;

    // Check if player can look at 
    private bool canFace;

    // Update is called once per frame
    void Update()
    {
        // Every frame, get current player direction
        TurnToPoint();

        // Check if player is allowed to face mouse direction
        if (canFace)
        {
            LookAtDirection();
        }
    }

    // Get player direction
    private void TurnToPoint()
    {
        // Draw Ray
        // Get the current position of the mouse
        Vector3 mousePos = Input.mousePosition;

        // Create a virtual plane at the player's Y level
        // This plane will act as a new "floor" for the player to be able to point at at all time
        Plane groundPlane = new Plane(Vector3.up, new Vector3(0, transform.position.y, 0));

        // Get the world position based on where mouse is on the plane
        Ray ray = cam.ScreenPointToRay(mousePos);
        // Fire raycast ray
        if (groundPlane.Raycast(ray, out float hitdata))
        {
            // Get the world position
            Vector3 worldPos = ray.GetPoint(hitdata);

            // Get end point vector3 for top down only
            Vector3 endPoint = new Vector3(worldPos.x, transform.position.y, worldPos.z);
            // Get direction: Destination - Source
            pointDir = (endPoint - transform.position).normalized;

            Debug.DrawLine(transform.position, endPoint, Color.red, 5f);
        }
    }

    // Player face direction for set time
    public IEnumerator FaceForTime(float time)
    {
        // Set specific position
        Vector3 specificPos = new Vector3(pointDir.x, 0f, pointDir.z);

        // Make player face a direction
        FaceDirection();
        // Wait for time
        yield return new WaitForSeconds(time);
        // Stop facing direction
        StopFacing();
    }

    // Make player face direction
    public void FaceDirection()
    {
        // Disable facing mouse based on player movement
        movement.DisableFacing();
        // Player can face
        canFace = true;
    }

    // Player stop facing direction
    public void StopFacing()
    {
        // Enable facing mouse based on player movement
        movement.EnableFacing();
        // Player cannot face
        canFace = false;
    }

    // Direction of player mouse
    private void LookAtDirection()
    {
        // Get the direction of the player
        Vector3 direction = new Vector3(pointDir.x, 0f, pointDir.z);
        // Make player face direction of saved facing direction
        player.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 12f);
    }
}
