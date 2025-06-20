/* 
 * Author: Loh Shau Ern Shaun, Jaykin Lee
 * Date Created: 17/5/2025
 * Date Updated: 19/5/2025
 * Updates player health to how much they currently have
 * Handles calling other effects when taking dmg (EG: Knockback, i-frames)
 * I-frames are done here
 * When reaching 0, reset the scene
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class PlayerHealth : MonoBehaviour
{
    // Value to handle amount of player health
    public int healthValue;

    // Reference to player health UI
    public Image healthPieChart;
    // Reference to player health text
    public TextMeshProUGUI healthText;

    // Reference to i-frame timer
    public float iFrameTime = 0.8f;
    // Reference to i-frame status
    public bool allowIFrames = false;
    // Reference to kb status
    public bool allowKB = true;

    // Reference to kb on hurt script
    [SerializeField]
    private KnockbackOnHurt kbOnHurt;

    // Awake is called before the first frame update, every scene start
    void Awake()
    {
        // Reset health
        ResetHealth();
    }

    // When player hits something
    private void OnTriggerEnter(Collider other)
    {
        // If it is under dmg class
        if (other.gameObject.CompareTag("Damage"))
        {
            // Check if player currently has i-frames
            if (!allowIFrames)
            {
                Debug.Log("Incoming dmg!");
                // Player takes dmg
                PlayerDamage(other.gameObject);
                // Player gets knocked back
                KnockbackPlayer(other.gameObject);
            }
            else
            {
                Debug.Log("i-frames!");
                // Player gets knocked back
                KnockbackPlayer(other.gameObject);
            }
        }
    }

    // When player takes dmg, with game object that hit player in arguments
    private void PlayerDamage(GameObject hurtingObj)
    {
        // Get the type of dmg from the game object
        HurtingObject dmgBy = hurtingObj.GetComponent<HurtingObject>();

        // Get the damage type
        string dmgType = dmgBy.type;
        // Init dmg variable
        int dmg = 0;
        // Check for dmg type
        // If melee dmg
        if (dmgType == "Melee")
        {
            // Deal 33 dmg
            dmg = 33;
        }
        // If projectile dmg
        else if (dmgType == "Projectile")
        {
            // Deal 25 dmg
            dmg = 25;
        }
        // If environmental dmg
        else if (dmgType == "Environment")
        {
            // Deal 20 dmg
            dmg = 20;
        }
        
        // Deduct respective health from value
        healthValue -= dmg;
        // Update the player's current hp
        UpdateHealthValue();

        if (hurtingObj.gameObject.GetComponent<HurtingObject>().type == "Projectile")
        {
            Destroy(hurtingObj.gameObject);
        }

        // If player out of health
        if (healthValue <= 0)
        {
            // Reset the current scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    
    // Knock the player back
    private void KnockbackPlayer(GameObject hurtingObj)
    {
        // If KB is allowed
        if (allowKB)
        {
            // Play the kb on hurt effect with player and hurting obj as arguments
            kbOnHurt.HurtKnockback(gameObject, hurtingObj);

            // Trigger i-frames
            StartCoroutine(HurtInvincibility(iFrameTime));
        }
    }

    // Start kb immune
    public void KBImmuneStart()
    {
        // Disallow taking dmg
        allowKB = false;
    }
    // Stop kb immune
    public void KBImmuneStop()
    {
        // Allow taking dmg again
        allowKB = true;
    }

    // Start i-frame timer
    private IEnumerator HurtInvincibility(float time)
    {
        // Disallow taking dmg
        DmgImmuneStart();
        // Wait for a little
        yield return new WaitForSeconds(time);
        // Allow taking dmg again
        DmgImmuneStop();
    }

    // Start i-frames
    public void DmgImmuneStart()
    {
        allowIFrames = true;
    }
    // Stop i-frames
    public void DmgImmuneStop()
    {
        allowIFrames = false;
    }

    // Reset player health
    private void ResetHealth()
    {
        // Set player health to max
        healthValue = 100;
        // Update the player's current hp
        UpdateHealthValue();
    }

    // Update the player health value
    private void UpdateHealthValue()
    {
        // Set the health UI to current hp
        healthPieChart.fillAmount = Convert.ToSingle(healthValue)/100;
        // Set the player health text
        healthText.text = healthValue.ToString();
    }
}
