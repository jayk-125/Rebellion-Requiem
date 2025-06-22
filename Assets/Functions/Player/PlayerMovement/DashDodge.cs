/* 
 * Author: Loh Shau Ern Shaun
 * Date: 6/5/2025
 * Enables player dodging
 * Player will do a dash based on player mouse direction
 * Player is invincible during dash
 * Cannot be kb'd during dash
 * Cannot move during dash
 * Cannot attack during dash
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DashDodge : MonoBehaviour
{
    // Reference player stunned script
    public PlayerStunned playerStunned;
    // Reference player pointing direction script
    public PointingDirection pointingDirection;

    // Reference player rigidbody
    public Rigidbody rb;

    // Saved pointdir
    private Vector3 pointDirSaved;

    // Dash status
    private bool allowDodge = true;
    // Reference if cooling down
    private bool isCoolingDown = false;
    // Currently dashing
    private bool isDashing = false;

    // Dash power
    public float dodgePower = 60f;
    // Dash time
    public float dodgeTime = 0.35f;
    // Dash cooldown
    public float dodgeCD = 0.65f;

    // Update is called every frame
    void Update()
    {
        // If the player is dashing
        if (isDashing)
        {
            // Get the direction of the player
            Vector3 direction = new Vector3(pointDirSaved.x, 0f, pointDirSaved.z);
            // Make player face direction of saved facing direction
            gameObject.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 12f);
        }

    }

    // When player dashes
    public void OnDash(InputAction.CallbackContext context)
    {
        // When current button phase is performed
        if (context.phase == InputActionPhase.Performed)
        {
            // Check if player can dodge
            if (allowDodge)
            {
                // Get player direction based on mouse
                pointDirSaved = pointingDirection.pointDir;
                
                // Force of dash
                Vector3 dashForce = pointDirSaved * dodgePower;

                // Set as dashing
                isDashing = true;
                
                // Move player in that direction
                rb.AddForce(dashForce, ForceMode.Impulse);
                // Disallow dodging
                allowDodge = false;
                // Set as cooling down
                isCoolingDown = true;

                // Disable player movement
                playerStunned.PlayerActionDisable();

                // Reset momentum
                StartCoroutine(ResetMomentum());
            }
        }
    }

    // Reset momentum after some time
    private IEnumerator ResetMomentum()
    {
        // Wait for a little
        yield return new WaitForSeconds(dodgeTime);
        // Remove rigidbody momentum
        rb.velocity = Vector3.zero;

        // No longer dashing
        isDashing = false;

        // Start dash cd
        StartCoroutine(DashCooldown(dodgeCD));
    }

    // Reset momentum after some time
    private IEnumerator DashCooldown(float time)
    {
        // Enable player movement
        playerStunned.PlayerActionEnable();

        // Wait for a little
        yield return new WaitForSeconds(time);
        // Reenable dashing
        allowDodge = true;
        // Set as no longer cooling down
        isCoolingDown = false;
    }

    // Start atk disabler
    public void DashDisableStart()
    {
        if (!isCoolingDown)
        {
            // Stop all coroutines
            StopAllCoroutines();
            // Disallow dodging
            allowDodge = false;
        }
    }
    // Stop atk disabler
    public void DashDisableStop()
    {
        if (!isCoolingDown)
        {
            // Allow dodging
            allowDodge = true;
        }
    }
}
