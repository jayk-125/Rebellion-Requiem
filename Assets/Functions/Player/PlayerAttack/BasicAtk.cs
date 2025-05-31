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
    // Reference to golem hitbox object
    [SerializeField]
    private GameObject[] atkList;
    // Reference to atk cds
    [SerializeField]
    private float[] cdList;
    // Reference to golem hitbox object
    private GameObject currentAttack;
    // Reference player camera
    public Camera cam;

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
    private Vector3 pointDir;

    // Start is called before the first frame update
    void Start()
    {
        // Hide all hitboxes
        ResetHitboxes();
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
        // Every frame, get current player direction
        TurnToPoint();
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
        }
        // If current click phase is cancelled (Button released)
        if (context.phase == InputActionPhase.Canceled)
        {
            // Toggle off continuous attack
            continuousAtk = false;
        }
    }

    // Continuous attack function
    private void KeepAttacking()
    {
        // Keep trying to attack
        // If atk is allowed
        if (allowAtk)
        {
            Debug.Log("Attacking!");
            // Disable attacking
            allowAtk = false;

            // Attack cooldown
            StartCoroutine(GolemAtkActive());
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
            // Set current hitbox as golem's
            currentAttack = atkList[0];
            // Set current attack cd as golem's
            attackCooldown = cdList[0];
        }
        else if (currentCharacter == "Butcher")
        {
            // Set current hitbox type as butcher's
            currentAttack = atkList[1];
            // Set current attack cd as butcher's
            attackCooldown = cdList[1];
        }
        else if (currentCharacter == "Slinger")
        {
            // Set current hitbox type as slinger's
            currentAttack = atkList[2];
            // Set current attack cd as slinger's
            attackCooldown = cdList[2];
        }
    }

    // Attack active
    private IEnumerator GolemAtkActive()
    {
        // Set player attacking as true
        attacking = true;
        // Carry out player attack 
        currentAttack.SetActive(attacking);

        // Face the attack to the direction
        currentAttack.transform.LookAt(transform.position + pointDir);

        // Active frames for attack
        yield return new WaitForSeconds(attackActive);
        // Set player attacking as false
        attacking = false;
        // Stop player attack 
        currentAttack.SetActive(attacking);

        Debug.Log("Attacked!");

        // Start attack cd
        StartCoroutine(GolemAtkCd());
    }

    // Attack cooldown
    private IEnumerator GolemAtkCd()
    {
        // Play attack cooldown
        yield return new WaitForSeconds(attackCooldown);
        // Allow player to attack after cooldown
        allowAtk = true;
    }

    // Get player direction
    private void TurnToPoint()
    {
        // Draw Ray
        // Get the current position of the mouse
        Vector3 mousePos = Input.mousePosition;

        // Create a virtual plane at the player's Y level
        // This plane will act as a new "floor" for the player to be able to point at at all time
        Plane groundPlane = new Plane(Vector3.up, new Vector3(0, transform.position.y, 0));

        // Get the world position based on where mouse is on the plane
        Ray ray = cam.ScreenPointToRay(mousePos);
        // Fire raycast ray
        if (groundPlane.Raycast(ray, out float hitdata))
        {
            // Get the world position
            Vector3 worldPos = ray.GetPoint(hitdata);
        
            // Get end point vector3 for top down only
            Vector3 endPoint = new Vector3(worldPos.x, transform.position.y, worldPos.z);
            // Get direction: Destination - Source
            pointDir = endPoint - transform.position;

            Debug.DrawLine(transform.position, endPoint, Color.red, 5f);
        }
    }
}
