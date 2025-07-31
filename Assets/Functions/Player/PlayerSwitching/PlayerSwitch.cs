/* 
 * Author: Loh Shau Ern , Jaykin Lee
 * Date Created: 25/5/2025
 * Date Updated: 4/6/2025
 * Player switching character models
 * Handles player switching
 * Changes player attack type based on current character
 * Sends out info to change the main game's UI
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSwitch : MonoBehaviour
{
    // Array containing player models
    [SerializeField]
    public GameObject[] characterArray;
    // Switch effect vfx
    public ParticleSystem switchEffect;
    // Get the current character
    private GameObject currentCharacter;
    // Current character state
    public string currentState;
    // Reference to switch sfx
    public AudioSource switchSFX;

    // Reference player Basic Attack script
    private BasicAtk basicAtk;
    // Reference player Skill1Select script
    private Skill1Select skill1Select;
    // Reference player Skill2Select script
    private Skill2Select skill2Select;

    // Bool to allow player switching
    private bool allowSwitch = true;
    // Bool to check if its start of scene
    private bool isStart = true;

    // Awake is called before the scene is loaded
    void Awake()
    {
        // Set player basic attack script
        basicAtk = gameObject.GetComponent<BasicAtk>();
        // Set player skill 1 select script
        skill1Select = gameObject.transform.GetChild(1).GetComponent<Skill1Select>();
        // Set player skill 2 select script
        skill2Select = gameObject.transform.GetChild(2).GetComponent<Skill2Select>();
    }

    // Start is called before the first frame update, when the scene is loaded
    // There WILL be an error if this is put together with awake, so this needs to be put in Start instead
    void Start()
    {
        // Set player as Golem
        // Reset all models
        ResetSwitchStates();
        // Set current gameobject model
        currentCharacter = characterArray[0];
        // Do the switch
        SwitchEnd();
    }

    // When player is switched to Golem
    public void OnSwitchGolem(InputAction.CallbackContext context)
    {
        // When current button phase is performed
        if (context.phase == InputActionPhase.Performed)
        {
            // If player can switch
            if (allowSwitch && currentCharacter != characterArray[0])
            {
                // Reset all models
                ResetSwitchStates();
                // Set current gameobject model 
                currentCharacter = characterArray[0];
                // Do the switch
                SwitchEnd();
            }
        }
    }

    // When player is switched to Butcher
    public void OnSwitchButcher(InputAction.CallbackContext context)
    {
        // When current button phase is performed
        if (context.phase == InputActionPhase.Performed)
        {
            // If player can switch
            if (allowSwitch && currentCharacter != characterArray[1])
            {
                // Reset all models
                ResetSwitchStates();
                // Set current gameobject model 
                currentCharacter = characterArray[1];
                // Do the switch
                SwitchEnd();
            }
        }
    }

    // When player is switched to Golem
    public void OnSwitchSlinger(InputAction.CallbackContext context)
    {
        // When current button phase is performed
        if (context.phase == InputActionPhase.Performed)
        {
            // If player can switch
            if (allowSwitch && currentCharacter != characterArray[2])
            {
                // Reset all models
                ResetSwitchStates();
                // Set current gameobject model 
                currentCharacter = characterArray[2];
                // Do the switch
                SwitchEnd();
            }
        }
    }

    // Reset the current character
    private void ResetSwitchStates()
    {
        // Check thru all the current player models
        foreach (GameObject character in characterArray)
        {
            // Hide each model
            character.SetActive(false);
        }
    }

    // Last part of the switch
    private void SwitchEnd()
    {
        // Play effects if it's not start
        if (!isStart)
        {
            // Create switch effect
            switchEffect.Play();
            // Play switch sfx
            switchSFX.Play();
        }
        // If it is start
        else
        {
            // Allow future switches to play effects
            isStart = false;
        }

        //Debug.Log("Switched");
        // Show player model
        currentCharacter.SetActive(true);
        // Set current character state
        currentState = currentCharacter.name;

        // Set the current basic attack to newly switched character
        basicAtk.SwitchCharacter(currentState);
        // Set the current skill 1 to newly switched character
        skill1Select.SwitchCharacter(currentState);
        // Set the current skill 2 to newly switched character
        skill2Select.SwitchCharacter(currentState);
    }

    // Disallow switching
    public void SwitchDisableStart()
    {
        // Set switching to false
        allowSwitch = false;
    }
    // Allow switching
    public void SwitchDisableStop()
    {
        // Set switching to true
        allowSwitch = true;
    }
}
