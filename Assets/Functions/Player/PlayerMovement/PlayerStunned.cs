using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStunned : MonoBehaviour
{
    // Reference player health script
    public PlayerHealth playerHealth;
    // Reference player movement script
    public Movement movement;
    // Reference player switching script
    public PlayerSwitch playerSwitch;
    // Reference player movement script
    public DashDodge dashDodge;
    // Reference player basic atk script
    public BasicAtk basicAtk;
    // Reference player skill 1 script
    public Skill1Select skill1Select;
    // Reference player skill 2 script
    public Skill2Select skill2Select;

    // When player action is disabled
    public IEnumerator PlayerActionDisableTimed(float time)
    {
        // Start player as disable
        PlayerActionDisable();
        //Debug.Log("Disabled for " + time);
        // Time disabled
        yield return new WaitForSeconds(time);
        PlayerActionEnable();
        //Debug.Log("Renabled");
    }

    // When player action is disabled
    public void PlayerActionDisable()
    {
        //Debug.Log("Disable");
        // Disallow switching to other characters
        playerSwitch.SwitchDisableStart();
        // Disallow movement
        movement.DisableMovement();

        // Disallow attacking
        basicAtk.AtkDisableStart();
        // Disallow dash
        dashDodge.DashDisableStart();
        // Disallow skill 1
        skill1Select.DisableSkill1Start();
        // Disallow skill 2
        skill2Select.DisableSkill2Start();

        // Disallow kb
        playerHealth.KBImmuneStart();
        // Give i-frames
        playerHealth.DmgImmuneStart();
    }

    // When player action is enabled
    public void PlayerActionEnable()
    {
        //Debug.Log("Enable");
        // Allow switching to other characters
        playerSwitch.SwitchDisableStop();
        // Allow movement
        movement.EnableMovement();

        // Allow attacking
        basicAtk.AtkDisableStop();
        // Allow dash
        dashDodge.DashDisableStop();
        // Allow skill 1
        skill1Select.DisableSkill1Stop();
        // Allow skill 2
        skill2Select.DisableSkill2Stop();

        // Allow kb
        playerHealth.KBImmuneStop();
        // Stop i-frames
        playerHealth.DmgImmuneStop();
    }

    // When player action is disabled, no invincibility
    public void PlayerActionDisableNoIframe()
    {
        //Debug.Log("Disable");
        // Disallow switching to other characters
        playerSwitch.SwitchDisableStart();
        // Disallow movement
        movement.DisableMovement();

        // Disallow attacking
        basicAtk.AtkDisableStart();
        // Disallow dash
        dashDodge.DashDisableStart();
        // Disallow skill 1
        skill1Select.DisableSkill1Start();
        // Disallow skill 2
        skill2Select.DisableSkill2Start();
    }

    // When player action is enabled, no invicibility
    public void PlayerActionEnableNoIframe()
    {
        //Debug.Log("Enable");
        // Disallow switching to other characters
        playerSwitch.SwitchDisableStop();
        // Disallow movement
        movement.EnableMovement();

        // Allow attacking
        basicAtk.AtkDisableStop();
        // Allow dash
        dashDodge.DashDisableStop();
        // Allow skill 1
        skill1Select.DisableSkill1Stop();
        // Allow skill 2
        skill2Select.DisableSkill2Stop();
    }

    // When player basics is disabled
    public void PlayerChargingDisabled()
    {
        // Disallow attacking
        basicAtk.AtkDisableStart();
        // Disallow dash
        dashDodge.DashDisableStart();
        // Disallow skill 1
        skill1Select.DisableSkill1Start();
    }

    // When player basics is enabled
    public void PlayerChargingEnabled()
    {
        // Allow attacking
        basicAtk.AtkDisableStop();
        // Allow dash
        dashDodge.DashDisableStop();
        // Allow skill 1
        skill1Select.DisableSkill1Stop();
    }
}
