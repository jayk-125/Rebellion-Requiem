/* 
 * Author: Loh Shau Ern Shaun
 * Date: 17/6/2025
 * Player skill 1 use
 * Handles player switching between different skill 1s
 * Calls effect from seperate corresponding skill script
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Skill1Select : MonoBehaviour
{
    // Reference the current character
    private string currentCharacter;
    // Reference current skill 1 in use
    private string currentSkill1;

    // Reference all relevant skill 1 ability scripts here
    // Golem Dash Skill
    private GolemDashSkill golemDashSkill;
    // Butcher Grapple Skill
    private ButcherGrappleSkill butcherGrappleSkill;
    // Slinger Warp Skill
    private SlingerWarpSkill slingerWarpSkill;

    // Start is called before the first frame update
    void Awake()
    {
        // Link all relevant skill 1 ability scripts here
        // Golem Dash Skill
        golemDashSkill = gameObject.GetComponent<GolemDashSkill>();
        // Butcher Grapple Skill
        butcherGrappleSkill = gameObject.GetComponent<ButcherGrappleSkill>();
        // Slinger Skill
        slingerWarpSkill = gameObject.GetComponent<SlingerWarpSkill>();
    }

    // When player attacks, give arguments of current click state
    public void OnSkill1(InputAction.CallbackContext context)
    {
        // Clicking in new unity input system has 3 phases: Started (onClick), Performed (whilePressing), Canceled (onRelease)
        // This makes unity register it as 3 clicks if phase not specified
        // If current click phase is performed (Button held down)
        if (context.phase == InputActionPhase.Performed)
        {
            //Debug.Log("Skill 1");
            // Check if golem skill 1 active
            if (currentSkill1 == "golemDashSkill")
            {
                // Do the golem dash skill effect
                golemDashSkill.OnSkill();
            }
            // Check if butcher skill 1 active
            else if (currentSkill1 == "butcherGrappleSkill")
            {
                // Do the butcher grapple skill effect
                butcherGrappleSkill.OnSkill();
            }
            // Check if slinger skill 1 active
            else if (currentSkill1 == "slingerWarpSkill")
            {
                // Do the slinger warp skill effect
                slingerWarpSkill.OnSkill();
            }
        }
    }

    // Get current character when switched
    public void SwitchCharacter(string currentChar)
    {
        // Set currentCharacter as new switched player
        currentCharacter = currentChar;
        //Debug.Log("Skill 1: " + currentCharacter);

        // Delete warp projectile
        slingerWarpSkill.DeleteWarp();

        // Set current character type
        if (currentCharacter == "Golem")
        {
            // Set current skill 1 as this skill
            currentSkill1 = "golemDashSkill";
            // Golem skill 1 enable
            golemDashSkill.Enable();
        }
        else if (currentCharacter == "Butcher")
        {
            // Set current skill 1 as this skill
            currentSkill1 = "butcherGrappleSkill";
            // Butcher skill 1 enable
            butcherGrappleSkill.Enable();
        }
        else if (currentCharacter == "Slinger")
        {
            // Set current skill 1 as this skill
            currentSkill1 = "slingerWarpSkill";
            // Slinger skill 1 enable
            slingerWarpSkill.Enable();
        }

        Debug.Log(currentSkill1);
    }

    // Disable all the skill 1s
    public void DisableSkill1Start()
    {
        // Golem skill 1 disable
        golemDashSkill.Disable();
        // Butcher skill 1 disable
        butcherGrappleSkill.Disable();
        // Slinger skill 1 disable
        slingerWarpSkill.Disable();
    }

    // Enable last the skill 1
    public void DisableSkill1Stop()
    {
        // CHeck if golem skill 1 active
        if (currentSkill1 == "golemDashSkill")
        {
            // Golem skill 1 enable
            golemDashSkill.Enable();
        }
        // Check if butcher skill 1 active
        else if (currentSkill1 == "butcherGrappleSkill")
        {
            // Butcher skill 1 enable
            butcherGrappleSkill.Enable();
        }
        // Check if slinger skill 1 active
        else if (currentSkill1 == "slingerWarpSkill")
        {
            // Slinger skill 1 enable
            slingerWarpSkill.Enable();
        }
    }
}
