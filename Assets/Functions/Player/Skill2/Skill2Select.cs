/* 
 * Author: Loh Shau Ern Shaun
 * Date: 21/6/2025
 * Player skill 2 use
 * Handles player switching between different skill 2s
 * Calls effect from seperate corresponding skill script
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Skill2Select : MonoBehaviour
{
    // Reference the current character
    private string currentCharacter;
    // Reference current skill 2 in use
    private string currentSkill2;

    // Reference all relevant skill 1 ability scripts here
    // Golem Dash Skill
    private GolemParrySkill golemParrySkill;
    // Butcher Lunge Skill
    private ButcherLungeSkill butcherLungeSkill;
    // Slinger Warp Skill
    private SlingerChargeSkill slingerChargeSkill;

    // Start is called before the first frame update
    void Awake()
    {
        // Link all relevant skill 2 ability scripts here
        // Golem Parry Skill
        golemParrySkill = gameObject.GetComponent<GolemParrySkill>();
        // Butcher Lunge Skill
        butcherLungeSkill = gameObject.GetComponent<ButcherLungeSkill>();
        // Slinger Charge Skill
        slingerChargeSkill = gameObject.GetComponent<SlingerChargeSkill>();
    }

    // When player attacks, give arguments of current click state
    public void OnSkill2(InputAction.CallbackContext context)
    {
        // Clicking in new unity input system has 3 phases: Started (onClick), Performed (whilePressing), Canceled (onRelease)
        // This makes unity register it as 3 clicks if phase not specified
        // If current click phase is performed (Button held down)
        if (context.phase == InputActionPhase.Performed)
        {
            //Debug.Log("Skill 2");
            // Check if golem skill 2 active
            if (currentSkill2 == "golemParrySkill")
            {
                // Do the golem parry skill effect
                golemParrySkill.OnSkill();
            }
            // Check if butcher skill 2 active
            else if (currentSkill2 == "butcherLungeSkill")
            {
                // Do the butcher lunge skill effect
                butcherLungeSkill.OnSkill();
            }
            // Check if slinger skill 2 active
            else if (currentSkill2 == "slingerChargeSkill")
            {
                // Do the slinger charge skill effect
                slingerChargeSkill.ChargeSkill();
            }
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            // Check if slinger skill 2 active
            if (currentSkill2 == "slingerChargeSkill")
            {
                // Do the slinger charge skill effect
                slingerChargeSkill.FireSkill();
            }
        }
    }

    // Get current character when switched
    public void SwitchCharacter(string currentChar)
    {
        // Set currentCharacter as new switched player
        currentCharacter = currentChar;
        Debug.Log("Skill 2: " + currentCharacter);

        // Set current character type
        if (currentCharacter == "Golem")
        {
            // Set current skill 2 as this skill
            currentSkill2 = "golemParrySkill";
            // Golem skill 2 enable
            golemParrySkill.Enable();
        }
        else if (currentCharacter == "Butcher")
        {
            // Set current skill 2 as this skill
            currentSkill2 = "butcherLungeSkill";
            // Butcher skill 2 enable
            butcherLungeSkill.Enable();
        }
        else if (currentCharacter == "Slinger")
        {
            // Set current skill 2 as this skill
            currentSkill2 = "slingerChargeSkill";
            // Slinger skill 2 enable
            slingerChargeSkill.Enable();
        }

        Debug.Log(currentSkill2);
    }

    // Disable all the skill 2s
    public void DisableSkill2Start()
    {
        // Golem skill 2 disable
        golemParrySkill.Disable();
        // Butcher skill 2 disable
        butcherLungeSkill.Disable();
        // Slinger skill 2 disable
        slingerChargeSkill.Disable();
    }

    // Enable last the skill 2
    public void DisableSkill2Stop()
    {
        // CHeck if golem skill 2 active
        if (currentSkill2 == "golemParrySkill")
        {
            // Golem skill 2 enable
            golemParrySkill.Enable();
        }
        // Check if butcher skill 2 active
        else if (currentSkill2 == "butcherLungeSkill")
        {
            // Butcher skill 2 enable
            butcherLungeSkill.Enable();
        }
        // Check if slinger skill 2 active
        else if (currentSkill2 == "slingerChargeSkill")
        {
            // Slinger skill 2 enable
            slingerChargeSkill.Enable();
        }
    }
}
