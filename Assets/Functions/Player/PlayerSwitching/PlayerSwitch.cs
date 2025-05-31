/* 
 * Author: Loh Shau Ern Shaun
 * Date: 25/5/2025
 * Player switching character models
 * Handles player switching
 * Changes player attack type based on current character
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSwitch : MonoBehaviour
{
    // Array containing player models
    [SerializeField]
    private GameObject[] characterArray;
    // Get the current character
    private GameObject currentCharacter;
    // Current character state
    public string currentState;
    // Reference player Basic Attack script
    private BasicAtk basicAtk;

    // Awake is called before the first frame update, when the scene is loaded
    void Awake()
    {
        // Set player basic attack script
        basicAtk = gameObject.GetComponent<BasicAtk>();
        // Set player as Golem
        // Reset all models
        ResetSwitchStates();
        // Set current gameobject model 
        currentCharacter = characterArray[0];
        // Do the switch
        SwitchEnd();
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

    // When player is switched to Golem
    public void OnSwitchGolem(InputAction.CallbackContext context)
    {
        // When current button phase is performed
        if (context.phase == InputActionPhase.Performed)
        {
            // Reset all models
            ResetSwitchStates();
            // Set current gameobject model 
            currentCharacter = characterArray[0];
            // Do the switch
            SwitchEnd();
        }
    }

    // When player is switched to Butcher
    public void OnSwitchButcher(InputAction.CallbackContext context)
    {
        // When current button phase is performed
        if (context.phase == InputActionPhase.Performed)
        {
            // Reset all models
            ResetSwitchStates();
            // Set current gameobject model 
            currentCharacter = characterArray[1];
            // Do the switch
            SwitchEnd();
        }
    }

    // When player is switched to Golem
    public void OnSwitchSlinger(InputAction.CallbackContext context)
    {
        // When current button phase is performed
        if (context.phase == InputActionPhase.Performed)
        {
            // Reset all models
            ResetSwitchStates();
            // Set current gameobject model 
            currentCharacter = characterArray[2];
            // Do the switch
            SwitchEnd();
        }
    }

    // Last part of the switch
    private void SwitchEnd()
    {
        // Show player model
        currentCharacter.SetActive(true);
        // Set current character state
        currentState = currentCharacter.name;
        // Set the current basic attack to newly switched character
        basicAtk.SwitchCharacter(currentState);
    }
}
