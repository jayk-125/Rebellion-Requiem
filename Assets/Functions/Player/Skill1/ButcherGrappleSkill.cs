/* 
 * Author: Loh Shau Ern Shaun
 * Date: 19/6/2025
 * Butcher skill 1
 * Butcher fires a projectile that either hits an object or opponent
 * Butcher will fly over to the object with invincibility
 * Does dmg in an AOE after reaching destination
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButcherGrappleSkill : MonoBehaviour
{
    // Reference player pointing direction script
    public PointingDirection pointingDirection;
    // Reference player pointing direction script
    public PlayerStunned playerStunned;

    // Dash hitbox
    public GameObject surroundingGrappleHitbox;
    // Grapple projectile
    public GameObject grappleProjectile;
    // Firing point
    public GameObject firingPoint;
    // Player object
    public GameObject player;
    // Reference player rigidbody
    public Rigidbody rb;

    // Speed of grapple projectile
    public float projectileSpd = 100f;
    // Time skill atk is active for
    public float skillActive = 0.165f;
    // Dash power
    public float skillFlyPower = 150f;
    // Time skill on cd after skill
    public float skillCd = 2.0f;
    // Saved pointdir
    private Vector3 pointDirSaved;

    // Reference if skill is in use
    private bool allowSkill = false;
    // Reference if cooling down
    private bool isCoolingDown = false;
    // Bool if currently in flight
    private bool inFlight = false;
    // Position of destination
    private Vector3 destination;

    // If timer can start
    private bool startTimer = false;
    // Back up timer
    public float maxFlyTime = 0.5f;
    // Current time
    private float currentTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        // Hide the surrounding hitbox
        surroundingGrappleHitbox.SetActive(false);
    }

    void Update()
    {
        // If player is in flight
        if (inFlight)
        {
            if (startTimer)
            {
                Vector3 playerPosition = player.transform.position;
                Debug.Log(playerPosition + " " + destination);
                // If player has reached position
                if (Vector3.Distance(playerPosition, destination) <= 2f || currentTime >= maxFlyTime)
                {
                    // Reset momentum
                    ResetMomentum();
                }
                else
                {
                    // Increase time
                    currentTime += Time.deltaTime;
                    // Move this to
                    //player.transform.Translate(pointDirSaved * skillFlyPower * Time.deltaTime);
                }
            }
        }
    }

    // When player attacks, give arguments of current click state
    public void OnSkill()
    {
        // If the player can use this skill
        if (allowSkill)
        {
            //Debug.Log("Skill1!");
            // Disable skill
            allowSkill = false;
            // Coolding down
            isCoolingDown = true;

            // Attack cooldown
            SkillActive();
        }
    }

    // Skill active
    private void SkillActive()
    {
        // Effect of grapple
        // Get player direction based on mouse
        pointDirSaved = pointingDirection.pointDir;
        // Fire grapple projectile in that direction
        GameObject grappleProjectileCurrent = Instantiate(grappleProjectile, firingPoint.transform.position, Quaternion.LookRotation(pointDirSaved));

        // Get projectile's rigidbody
        Rigidbody projRb = grappleProjectileCurrent.GetComponent<Rigidbody>();
        if (projRb != null)
        {
            projRb.velocity = pointDirSaved * projectileSpd;
        }

        // Remove the projectile after 0.5 seconds (reduces assets loaded)
        Destroy(grappleProjectileCurrent, skillActive);

        // Allow player to attack after cooldown
        allowSkill = false;
        // No longer cooling down
        isCoolingDown = true;

        // Reset momentum
        StartCoroutine(SkillCd());
    }

    // When projectile has hit wall
    public void ValidHit(Vector3 position)
    {
        // Set the destination
        destination = position;

        // Indefinite stun
        playerStunned.PlayerActionDisable();

        // Force of dash
        Vector3 dashForce = pointDirSaved * skillFlyPower;
        // Move player in that direction
        rb.AddForce(dashForce, ForceMode.Impulse);

        // Start timer
        startTimer = true;
        // Reset current time
        currentTime = 0f;
        // Set player as in flight
        inFlight = true;
    }

    // Reset momentum after some time
    private void ResetMomentum()
    {
        // Stop in flight
        inFlight = false;

        // Remove rigidbody momentum
        rb.velocity = Vector3.zero;

        Debug.Log("No longer in flight");

        // Activate hitbox for a lil
        StartCoroutine(ActiveSwing());
    }

    // Reset momentum after some time
    private IEnumerator ActiveSwing()
    {
        // Face the attack to the direction
        surroundingGrappleHitbox.SetActive(true);
        // Wait for a little
        yield return new WaitForSeconds(skillActive);
        // Stop player attack 
        surroundingGrappleHitbox.SetActive(false);

        // Undo stun
        playerStunned.PlayerActionEnable();

        //Debug.Log("Skill 1 Attacked!");
    }

    // Attack cooldown
    private IEnumerator SkillCd()
    {
        // Play attack cooldown
        yield return new WaitForSeconds(skillCd);
        // Allow player to attack after cooldown
        allowSkill = true;
        // No longer cooling down
        isCoolingDown = false;
    }

    // Enable this skill
    public void Enable()
    {
        // Is not coolding down
        if (!isCoolingDown)
        {
            // Enable
            allowSkill = true;
        }
    }

    // Disable this skill
    public void Disable()
    {
        // Disable
        allowSkill = false;
    }
}
