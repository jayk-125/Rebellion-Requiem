/* 
 * Author: Loh Shau Ern Shaun
 * Date: 23/5/2025
 * Player attacking with LMB
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
    private GameObject golemAttack;
    // Reference player camera
    public Camera cam;

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
        // Find hitbox gameobject
        golemAttack = transform.Find("GBasHitbox").gameObject;
        // Hide hitboxes
        golemAttack.SetActive(false);
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

    // Attack active
    private IEnumerator GolemAtkActive()
    {
        // Set player attacking as true
        attacking = true;
        // Carry out player attack 
        golemAttack.SetActive(attacking);

        // Face the attack to the direction
        golemAttack.transform.LookAt(pointDir);

        // Active frames for attack
        yield return new WaitForSeconds(attackActive);
        // Set player attacking as false
        attacking = false;
        // Stop player attack 
        golemAttack.SetActive(attacking);

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
        mousePos.z = 10f;

        // Set the mouse position based on screen to world point
        mousePos = cam.ScreenToWorldPoint(mousePos);
        // Get direction for top down only
        Vector3 directionOnly = new Vector3(mousePos.x, transform.position.y, mousePos.z);
        // Get initial direction
        Vector3 initPointDir = directionOnly - transform.position;
        // Set final position
        pointDir = new Vector3(initPointDir.x, transform.position.y, initPointDir.z);
        //Debug.DrawLine(transform.position, pointDir, Color.red, 5f);
    }
}
