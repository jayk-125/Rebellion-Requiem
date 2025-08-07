/* 
 * Author: Loh Shau Ern Shaun
 * Date: 21/6/2025
 * Golem skill 2
 * Golem does a parry
 * When hit during parry state, carry out parry effect
 * Parrying resets cd, does dmg in an aoe
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemParrySkill : MonoBehaviour
{
    // Reference player pointing direction script
    public PlayerStunned playerStunned;
    // Reference player health script
    public PlayerHealth playerHealth;

    // Reference golem skill effect
    public ParticleSystem golemSkillEffect;
    // Reference pulse effect
    public ParticleSystem pulseEffect;
    // Reference parry stance sfx
    public AudioSource parryStanceSFX;
    // Reference parry hit sfx
    public AudioSource parryHitSFX; 

    // Parry hitbox
    public GameObject parryCollider;
    // Attack hitbox
    public GameObject surroundingParryHitbox;

    // Time parry is active for
    public float parryActive = 0.45f;
    // Time skill is active for
    public float skillActive = 0.15f;
    // Time skill on cd after skill
    public float skillCd = 2.0f;

    // Reference if skill is in use
    private bool allowSkill = false;
    // Reference if cooling down
    private bool isCoolingDown = false;
    // Reference if parried
    private bool didParry = false;

    // Start is called before the first frame update
    void Start()
    {
        // Hide the surrounding hitbox
        parryCollider.SetActive(false);
        // Hide the surrounding hitbox
        surroundingParryHitbox.SetActive(false);
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
            // Play parry stance sfx
            parryStanceSFX.Play();

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
        // Carry out parry 
        parryCollider.SetActive(true);
        // Play pulse effect
        pulseEffect.Play();
        // Disable player movement
        playerStunned.PlayerActionDisable();

        // Active frames for attack
        yield return new WaitForSeconds(parryActive);

        // Hide parry hitbox
        parryCollider.SetActive(false);
        // Enable player movement
        playerStunned.PlayerActionEnable();

        // If pulse if playing
        if (pulseEffect.isPlaying)
        {
            // Stop pulse effect
            pulseEffect.Stop();
        }

        // Skill cd
        StartCoroutine(SkillCd());
    }

    // Attack cooldown
    private IEnumerator SkillCd()
    {
        // Play attack cooldown
        yield return new WaitForSeconds(skillCd);

        // Reset player state after cd
        ResetParryStance();
    }

    // When parry activates
    public void ParryEffect()
    {
        // If parry not done yet
        if (!didParry)
        {
            Debug.Log("PARRY!");
            // Play parry hit sfx
            parryHitSFX.Play();

            // Set as did parry
            didParry = true;

            // Reset the attack cooldown
            ResetCooldown();

            // Heal player
            playerHealth.PlayerHeal();

            // Enable player movement
            playerStunned.PlayerActionEnable();

            // Disallow player to attack after cooldown
            allowSkill = false;
            // Start cooling down
            isCoolingDown = true;

            // If pulse is playing
            if (pulseEffect.isPlaying)
            {
                // Stop pulse effect
                pulseEffect.Stop();
            }

            // Play the parry attack
            StartCoroutine(ParryAttack());
        }
    }

    // Parry attack
    private IEnumerator ParryAttack()
    {
        // Activate parry attack
        surroundingParryHitbox.SetActive(true);
        // Disable player movement
        playerStunned.PlayerActionDisable();

        // Play skill particle effect
        golemSkillEffect.Play();

        // Play attack cooldown
        yield return new WaitForSeconds(skillActive);

        //Debug.Log("off");

        // Allow player to attack after cooldown
        allowSkill = true;
        // No longer cooling down
        isCoolingDown = false;

        // Set as no longer parrying
        didParry = false;
        // Deactivate parry attack
        surroundingParryHitbox.SetActive(false);
    }

    // Prematurely reset cooldown effects
    private void ResetCooldown()
    {
        // Stop all relevant coroutines
        StopCoroutine(SkillActive());
        StopCoroutine(SkillCd());

        // Hide parry hitbox
        parryCollider.SetActive(false);
        // Enable player movement
        playerStunned.PlayerActionEnable();

        // Get the reset 
        ResetParryStance();
    }

    // Called when reseting parry stance
    private void ResetParryStance()
    {
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
        if (!didParry)
        {
            // Disable
            allowSkill = false;
        }
    }
}
