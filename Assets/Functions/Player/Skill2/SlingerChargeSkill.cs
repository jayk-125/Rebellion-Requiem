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
    // Reference slinger skill effect
    public ParticleSystem slingerSkillEffect;
    // Reference pulse effect
    public ParticleSystem pulseEffect;
    // Reference charge start sfx
    public AudioSource chargeStartSFX;
    // Reference charge fire sfx
    public AudioSource chargeFireSFX;
    // Reference charge max sfx
    public AudioSource chargeMaxSFX;

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
    // Charge Rate Value
    private int chargeRate = 2;
    // Max Charge Power Value
    private int maxCharge = 60;


    // Reference if skill is being held down
    public bool heldDown = false;
    // Reference if skill is in use
    private bool allowSkill = false;
    // Reference if cooling down
    private bool isCoolingDown = false;
    // Currently facing
    private bool isFacing = false;
    // Player attack charging status
    private bool isCharging = false;
    // Player charge is max
    private bool isMax = false;

    // Update is called every frame
    void Update()
    {
        //Debug.Log(heldDown);
        // If is held down
        if (heldDown)
        {
            // Check if can charge
            if (allowSkill)
            {
                // If not started charging yet
                if (!isCharging)
                {
                    // Allow charging skill
                    ChargeSkill();
                    // Disable non-charge atk skills
                    playerStunned.PlayerChargingDisabled();
                }
                // If charging started
                else
                {
                    // Allow face direction of mouse
                    pointingDirection.FaceDirection();
                    ChargingAttack();
                }
            }
        }

        // If the player is facing
        if (isFacing)
        {
            // Get the direction of the player
            Vector3 direction = new Vector3(pointDirSaved.x, 0f, pointDirSaved.z);
            // Make player face direction of saved facing direction
            player.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 12f);
        }
    }

    // When player is holding down skill button
    public void ChargeSkill()
    {
        Debug.Log("Charging");
        // Play charge start sfx
        chargeStartSFX.Play();
        // Play skill effect
        SkillActivate();
        // Play pulse effect
        pulseEffect.Play();
        // Set charging as true
        isCharging = true;
        // Set facing as true
        isFacing = true;
    }

    // Increase power while charging attack
    private void ChargingAttack()
    {
        // While charge power is less than 120f
        if (chargePow < maxCharge)
        {
            // Increase power
            chargePow += chargeRate;

            //Debug.Log(chargePow);
        }
        else if (!isMax)
        {
            // Set is max as true
            isMax = true;
            // Play charge max sfx
            chargeMaxSFX.Play();
        }
    }

    // When player attacks, give arguments of current click state
    public void FireSkill()
    {
        // If the player can use this skill
        if (allowSkill)
        {
            //Debug.Log("Skill1!");

            // Play skill effect
            SkillActivate();
            // Disable skill
            allowSkill = false;
            // Set charging as false
            isCharging = false;
            // Set facing as false
            isFacing = false;
            // Set max as false
            isMax = false;

            // Stop facing direction of mouse
            pointingDirection.StopFacing();

            // Stop charge start sfx
            chargeStartSFX.Stop();
            // Play charge fire sfx
            chargeFireSFX.Play();
            // Stop pulse effect
            pulseEffect.Stop();
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
        // Is cooling down
        isCoolingDown = true;

        // Enable non-charge atk skills
        playerStunned.PlayerChargingEnabled();

        // Reset momentum
        StartCoroutine(SkillCd());
    }

    // Play effect on skill use
    public void SkillActivate()
    {
        // If the player can use this skill
        if (allowSkill)
        {
            // Play skill particle effect
            slingerSkillEffect.Play();
        }
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
