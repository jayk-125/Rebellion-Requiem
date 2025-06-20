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

    // Dash status
    private bool allowDodge = true;
    // Dash power
    public float dodgePower = 60f;
    // Dash time
    public float dodgeTime = 0.35f;
    // Dash cooldown
    public float dodgeCD = 0.65f;

    // When player dashes
    public void OnDash(InputAction.CallbackContext context)
    {
        // When current button phase is performed
        if (context.phase == InputActionPhase.Performed)
        {
            if (allowDodge)
            {
                // Get player direction based on mouse
                Vector3 pointDir = pointingDirection.pointDir;
                // Force of dash
                Vector3 dashForce = pointDir * dodgePower;
                // Move player in that direction
                rb.AddForce(dashForce, ForceMode.Impulse);
                // Disallow dodging
                allowDodge = false;

                // Disable player movement
                playerStunned.PlayerActionDisableTimed(dodgeTime);

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
        // Start dash cd
        StartCoroutine(DashCooldown(dodgeCD));
    }

    // Reset momentum after some time
    private IEnumerator DashCooldown(float time)
    {
        // Wait for a little
        yield return new WaitForSeconds(time);
        // Reenable dashing
        allowDodge = true;
    }

    // Start atk disabler
    public void DashDisableStart()
    {
        // Stop all coroutines
        StopAllCoroutines();
        // Disallow dodging
        allowDodge = false;
    }
    // Stop atk disabler
    public void DashDisableStop()
    {
        // Allow dodging
        allowDodge = true;
    }
}
