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
    // Reference player health script
    public PlayerHealth playerHealth;
    // Reference player basic atk script
    public BasicAtk basicAtk;
    // Reference player pointing direction script
    public PointingDirection pointingDirection;
    // Reference player movement script
    public Movement movement;
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

                // Disallow movement
                StartCoroutine(movement.DisableMovement(dodgeTime));
                // Disallow attacking
                StartCoroutine(basicAtk.AtkDisable(dodgeTime));
                // Disallow kb
                StartCoroutine(playerHealth.KBImmune(dodgeTime));
                // Give i-frames
                StartCoroutine(playerHealth.HurtInvincibility(dodgeTime));
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
}
