/* 
 * Author: Loh Shau Ern Shaun
 * Date: 19/6/2025
 * Slinger skill 1
 * Slinger fires a projectile that stops a little after being shot
 * Slinger will fly over to the object with invincibility
 * Does dmg in an AOE after reaching destination
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingerWarpSkill : MonoBehaviour
{
    // Reference player pointing direction script
    public PointingDirection pointingDirection;
    // Reference player pointing direction script
    public PlayerStunned playerStunned;

    // Warp hitbox
    public GameObject surroundingWarpHitbox;
    // Warp projectile
    public GameObject warpProjectile;
    // Firing point
    public GameObject firingPoint;
    // Player object
    public GameObject player;
    // Player camera
    public GameObject playerCamera;

    // Speed of warp projectile
    public float projectileSpd = 60f;
    // Time skill atk is active for
    public float skillActive = 0.25f;
    // Time skill on cd after skill
    public float skillCd = 4.0f;
    // Saved pointdir
    private Vector3 pointDirSaved;
    // Saved current projectile
    private GameObject warpProjectileCurrent;

    // Reference if skill is in use
    private bool allowSkill = false;
    // Reference if cooling down
    private bool isCoolingDown = false;
    // Bool if warp currently exists
    private bool bulletExists = false;
    // Position of destination
    private Vector3 destination;

    // Start is called before the first frame update
    void Start()
    {
        // Hide the surrounding hitbox
        surroundingWarpHitbox.SetActive(false);
    }

    // When player attacks, give arguments of current click state
    public void OnSkill()
    {
        // If the player can use this skill
        if (allowSkill)
        {
            //Debug.Log("Skill1!");

            // When player first uses the skill
            if (!bulletExists)
            {
                // Activate cast effect
                // Play skill effect
                SkillActive();
            }
            // When player recasts skill
            else
            {
                // Activate recast effect
                RecastSkill();
            }
        }
    }

    // Skill active
    private void SkillActive()
    {
        // Effect of warp projectile
        // Get player direction based on mouse
        pointDirSaved = pointingDirection.pointDir;
        // Fire grapple projectile in that direction
        warpProjectileCurrent = Instantiate(warpProjectile, firingPoint.transform.position, Quaternion.LookRotation(pointDirSaved));

        // Get projectile's rigidbody
        Rigidbody projRb = warpProjectileCurrent.GetComponent<Rigidbody>();
        // If bullet has rb
        if (projRb != null)
        {
            // Set force to be applied
            Vector3 bulletForce = pointDirSaved * projectileSpd;
            // Apply force to bullet object
            projRb.AddForce(bulletForce, ForceMode.Impulse);
        }

        // Set bullet as exists
        bulletExists = true;

        // Remove the projectile after half of cooldown
        Destroy(warpProjectileCurrent, (skillCd));

        // Is cooling down
        isCoolingDown = true;

        // Reset momentum
        StartCoroutine(SkillCd());
    }

    // Set the current projectile position
    public void CurrentLocation(Vector3 position)
    {
        // Set saved position as the projectile's position
        destination = position;
    }

    // When projectile has hit wall
    public void RecastSkill()
    {
        // TP camera to pos, without going over borders
        playerCamera.GetComponent<CameraController>().MoveCamera(destination);
        // TP to bullet pos
        player.transform.position = destination;

        // Start stun
        playerStunned.PlayerActionDisable();

        // Set bullet exists as false
        bulletExists = false;

        // Delete the bullet
        Destroy(warpProjectileCurrent);

        // Disallow skill
        allowSkill = false;
        
        // Do swing attack
        StartCoroutine(ActiveSwing());
    }

    // Reset momentum after some time
    private IEnumerator ActiveSwing()
    {
        // Face the attack to the direction
        surroundingWarpHitbox.SetActive(true);
        // Wait for a little
        yield return new WaitForSeconds(skillActive);
        // Stop player attack 
        surroundingWarpHitbox.SetActive(false);

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
