/* 
 * Author: Loh Shau Ern Shaun
 * Date: 23/5/2025
 * Player attacking with LMB
 * Attacks in direction of player mouse from player position
 * Player cannot attack during cooldown of attack
 * Varies based on current character
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BasicAtk : MonoBehaviour
{
    // Reference to hitbox object
    [SerializeField]
    private GameObject[] atkList;
    // Reference to atk cds
    [SerializeField]
    private float[] cdList;
    // Reference to atk sfx
    public AudioSource[] atkSFX;
    // Curret sfx list num
    private int currentSFX;

    // Reference to hitbox object
    private GameObject currentAttack;
    // Reference the current character
    private string currentCharacter;

    // Checks if player is attacking
    private bool attacking = false;
    // Checks if player can attack
    public bool allowAtk = true;
    // Checks if continuous attack is active
    public bool continuousAtk = false;

    // Time attack can do dmg for
    private float attackActive = 0.1f;
    // Time attack is on cd after attack
    public float attackCooldown = 0.55f;

    // Direction of mouse from player
    public PointingDirection pointingDirection;

    // Reference to animation scripts
    // Butcher animations
    public ButcherAnim butcherAnim;

    // Start is called before the first frame update
    void Start()
    {
        // Hide all hitboxes
        ResetHitboxes();
        // Set butcher start
        SwitchCharacter("Golem");
    }

    // Update is called once per frame
    void Update()
    {
        // If continuous attack is on
        if (continuousAtk)
        {
            // Keep on attacking
            KeepAttacking();
        }
    }
    
    // When player attacks, give arguments of current click state
    public void OnAttack(InputAction.CallbackContext context)
    {
        // Clicking in new unity input system has 3 phases: Started (onClick), Performed (whilePressing), Canceled (onRelease)
        // This makes unity register it as 3 clicks if phase not specified
        // If current click phase is performed (Button held down)
        if (context.phase == InputActionPhase.Performed)
        {
            // Toggle on continuous attack
            continuousAtk = true;
            // Set as player face direction of mouse
            pointingDirection.FaceDirection();
        }
        // If current click phase is cancelled (Button released)
        if (context.phase == InputActionPhase.Canceled)
        {
            // Toggle off continuous attack
            continuousAtk = false;
            // Set as player no longer face direction of mouse
            pointingDirection.StopFacing();
        }
    }

    // Continuous attack function
    private void KeepAttacking()
    {
        // Keep trying to attack
        // If atk is allowed
        if (allowAtk)
        {
            //Debug.Log("Attacking!");
            // Disable attacking
            allowAtk = false;

            // Attack cooldown
            StartCoroutine(AtkActive());
        }
    }

    // Hide all the hitboxes
    private void ResetHitboxes()
    {
        // Go thru each saved hitbox
        foreach (GameObject hitbox in atkList)
        {
            // Hide each one
            hitbox.SetActive(false);
        }
    }

    // Get current character when switched
    public void SwitchCharacter(string currentChar)
    {
        // Stop all hitboxes
        ResetHitboxes();

        // Set currentCharacter as new switched player
        currentCharacter = currentChar;
        Debug.Log("BasicAtk: " + currentCharacter);

        // Set current hitbox type
        if (currentCharacter == "Golem")
        {
            // Set current sfx
            currentSFX = 0;
            // Set current hitbox as golem's
            currentAttack = atkList[0];
            // Set current attack cd as golem's
            attackCooldown = cdList[0];
        }
        else if (currentCharacter == "Butcher")
        {
            // Set current sfx
            currentSFX = 1;
            // Set current hitbox type as butcher's
            currentAttack = atkList[1];
            // Set current attack cd as butcher's
            attackCooldown = cdList[1];
        }
        else if (currentCharacter == "Slinger")
        {
            // Set current sfx
            currentSFX = 2;
            // Set current hitbox type as slinger's
            currentAttack = atkList[2];
            // Set current attack cd as slinger's
            attackCooldown = cdList[2];
        }
    }

    // Attack active
    private IEnumerator AtkActive()
    {
        // Set player attacking as true
        attacking = true;
        // Play corresponding atk sfx
        atkSFX[currentSFX].Play();
        // Carry out player attack 
        currentAttack.SetActive(attacking);

        // Get current pointing direction
        Vector3 pointDir = pointingDirection.pointDir;
        // Face the attack to the direction
        currentAttack.transform.LookAt(transform.position + pointDir);

        // Active frames for attack
        yield return new WaitForSeconds(attackActive);
        // Set player attacking as false
        attacking = false;
        // Stop player attack 
        currentAttack.SetActive(attacking);

        // Play attack animation
        butcherAnim.ButcherBasic();

        //Debug.Log("Attacked!");

        // Start attack cd
        StartCoroutine(AtkCd());
    }

    // Attack cooldown
    private IEnumerator AtkCd()
    {
        // Play attack cooldown
        yield return new WaitForSeconds(attackCooldown);
        // Allow player to attack after cooldown
        allowAtk = true;
    }

    // Start atk disabler
    public void AtkDisableStart()
    {
        // Stop all coroutines
        StopAllCoroutines();
        // Disable all hitboxes
        ResetHitboxes();
        // Disallow taking dmg
        allowAtk = false;
    }
    // Stop atk disabler
    public void AtkDisableStop()
    {
        // Disallow taking dmg
        allowAtk = true;
        // If continuous attack is on
        if (continuousAtk)
        {
            // Set as player face direction of mouse
            pointingDirection.FaceDirection();
        }
    }
}
