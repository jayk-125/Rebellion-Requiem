/* 
 * Author: Loh Shau Ern Shaun
 * Date: 17/6/2025
 * Golem skill 1
 * Golem does a dash that does dmg
 * Cannot be used when stunned
 * Invincible dash attack
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemDashSkill : MonoBehaviour
{
    // Reference to player object
    public GameObject player;
    // Reference player pointing direction script
    public PointingDirection pointingDirection;
    // Reference player pointing direction script
    public PlayerStunned playerStunned;
    // Reference golem skill effect
    public ParticleSystem golemSkillEffect;
    // Reference looping dash effect
    public ParticleSystem loopingDashEffect;
    // Reference golem dash sfx
    public AudioSource dashSFX;

    // Dash hitbox
    public GameObject surroundingDashHitbox;
    // Reference player rigidbody
    public Rigidbody rb;

    // Saved pointdir
    private Vector3 pointDirSaved;

    // Time skill is active for
    public float skillActive = 0.15f;
    // Dash power
    public float skillDashPower = 80f;
    // Time skill on cd after skill
    public float skillCd = 2.0f;
    
    // Reference if skill is in use
    private bool allowSkill = false;
    // Reference if cooling down
    private bool isCoolingDown = false;
    // Currently dashing
    private bool isDashing = false;

    // Start is called before the first frame update
    void Start()
    {
        // Hide the surrounding hitbox
        surroundingDashHitbox.SetActive(false);
    }

    // Update is called every frame
    void Update()
    {
        // If the player is dashing
        if (isDashing)
        {
            // Get the direction of the player
            Vector3 direction = new Vector3(pointDirSaved.x, 0f, pointDirSaved.z);
            // Make player face direction of saved facing direction
            player.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 12f);
        }
    }

    // When player attacks, give arguments of current click state
    public void OnSkill()
    {
        // If the player can use this skill
        if (allowSkill)
        {
            //Debug.Log("Skill1!");

            // Play skill particle effect
            golemSkillEffect.Play();
            // Disable skill
            allowSkill = false;
            // Coolding down
            isCoolingDown = true;

            // Attack cooldown
            StartCoroutine(SkillActive());
        }
    }

    // Skill active
    private IEnumerator SkillActive()
    {
        // Carry out player attack 
        surroundingDashHitbox.SetActive(true);
        // Play the dash effect
        loopingDashEffect.Play();
        // Play the dash sfx
        dashSFX.Play();

        // Effect of dash
        // Get player direction based on mouse
        pointDirSaved = pointingDirection.pointDir;
        // Force of dash
        Vector3 dashForce = pointDirSaved * skillDashPower;
        // Move player in that direction
        rb.AddForce(dashForce, ForceMode.Impulse);

        // Disable player movement
        playerStunned.PlayerActionDisable();

        // Set as dashing
        isDashing = true;

        // Active frames for attack
        yield return new WaitForSeconds(skillActive);

        // Reset momentum
        StartCoroutine(ResetMomentum());
    }

    // Reset momentum after some time
    private IEnumerator ResetMomentum()
    {
        // Wait for a little
        yield return new WaitForSeconds(skillActive);
        
        // Stop player attack 
        surroundingDashHitbox.SetActive(false);

        //Debug.Log("Skill 1 Attacked!");

        // Remove rigidbody momentum
        rb.velocity = Vector3.zero;

        // No longer dashing
        isDashing = false;

        // Stop the dash effect
        loopingDashEffect.Stop();

        // Enable player movement
        playerStunned.PlayerActionEnable();

        // Start dash cd
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
        // Is not coolding down
        if (!isCoolingDown)
        {
            // Disable
            allowSkill = false;
        }
    }
}
