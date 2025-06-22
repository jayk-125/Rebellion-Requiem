/* 
 * Author: Loh Shau Ern Shaun
 * Date: 21/6/2025
 * Slinger skill 2
 * Slinger can charge attack
 * Massive hitbox
 * Kinda moves player forward on use
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingerChargeSkill : MonoBehaviour
{
    // Reference to player object
    public GameObject player;
    // Reference player pointing direction script
    public PointingDirection pointingDirection;
    // Reference player pointing direction script
    public PlayerStunned playerStunned;

    // Charged projectile
    public GameObject chargeProjectile;
    // Firing point
    public GameObject firingPoint;

    // Saved pointdir
    private Vector3 pointDirSaved;

    // Time skill active after skill
    public float skillActive = 0.5f;
    // Time skill on cd after skill
    public float skillCd = 3.0f;
    // Speed of bullet projectile
    public float projectileSpd = 100f;
    // Charge Power Value
    private int chargePow = 0;
    // Max Charge Power Value
    private float maxCharge = 60f;

    // Reference if skill is in use
    private bool allowSkill = false;
    // Reference if cooling down
    private bool isCoolingDown = false;
    // Currently facing
    private bool isFacing = false;
    // Player attack charging status
    private bool isCharging = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called every frame
    void Update()
    {
        // If the player is facing
        if (isFacing)
        {
            // Get the direction of the player
            Vector3 direction = new Vector3(pointDirSaved.x, 0f, pointDirSaved.z);
            // Make player face direction of saved facing direction
            player.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 12f);
        }

        // If the player is currently charging attack
        if (isCharging)
        {
            // Allow face direction of mouse
            pointingDirection.FaceDirection();
            ChargingAttack();
        }
    }

    // When player is holding down skill button
    public void ChargeSkill()
    {
        // If the player can use this skill
        if (allowSkill)
        {
            // Set charging as true
            isCharging = true;
        }
    }

    // Increase power while charging attack
    private void ChargingAttack()
    {
        // While charge power is less than 120f
        if (chargePow < maxCharge)
        {
            // Increase power
            chargePow += 1;

            Debug.Log(chargePow);
        }
    }

    // When player attacks, give arguments of current click state
    public void FireSkill()
    {
        // If the player can use this skill
        if (allowSkill)
        {
            //Debug.Log("Skill1!");
            // Disable skill
            allowSkill = false;
            // Coolding down
            isCoolingDown = true;
            // Set charging as false
            isCharging = false;

            // Stop facing direction of mouse
            pointingDirection.StopFacing();
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
        GameObject chargeProjectileCurrent = Instantiate(chargeProjectile, firingPoint.transform.position, Quaternion.LookRotation(pointDirSaved));

        // Get projectile's rigidbody
        Rigidbody projRb = chargeProjectileCurrent.GetComponent<Rigidbody>();
        if (projRb != null)
        {
            // Fire projectile in a direction
            projRb.velocity = pointDirSaved * projectileSpd;
        }
        // Get projectile's charge projectile script
        ChargeProjectile chargeProj = chargeProjectileCurrent.GetComponent<ChargeProjectile>();
        if (chargeProj != null)
        {
            // Add charge dmg
            chargeProj.ChargeDmg(chargePow);
        }

        // Remove the projectile after 0.5 seconds (reduces assets loaded)
        Destroy(chargeProjectileCurrent, skillActive);

        // Reset charge value
        chargePow = 0;
        // Allow player to attack after cooldown
        allowSkill = false;
        // No longer cooling down
        isCoolingDown = true;

        // Reset momentum
        StartCoroutine(SkillCd());
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
        // Is not cooling down
        if (!isCoolingDown)
        {
            // Disable
            allowSkill = false;
        }
    }
}
