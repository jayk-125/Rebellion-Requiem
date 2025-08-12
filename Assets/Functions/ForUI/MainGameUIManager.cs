/* 
 * Author: Jaykin Lee
 * Date Created: 7/6/2025
 * Finds the appropriate scriptableObjects from the character names on start
 * Switches out the UI when called
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System;

public class MainGameUIManager : MonoBehaviour
{
    //Reference to UI interface elements
    [SerializeField]
    private Image Hue;
    [SerializeField]
    private Image HealthDisplay;
    [SerializeField]
    private Image WeaponDisplay;

    //Reference to script that holds the character's names
    [SerializeField]
    public GameObject ObjectWithCharacterArray;

    //Variable to hold the script to extract variables
    private PlayerSwitch playerSwitch;

    //Name of character to find
    private string CharacterName;

    //Array of scriptables to grab
    public List<CharacterData> CharacterData;

    // Start is called before the first frame update
    void Start()
    {
        // Grab PlayerSwitch reference
        playerSwitch = ObjectWithCharacterArray.GetComponent<PlayerSwitch>();

        //Load all required ScriptableObjects synchronously
        LoadAllCharacterData();

        //Only set UI if data exists
        if (CharacterData.Count > 0)
        {
            Hue.sprite = CharacterData[0].characterUI[0];
            HealthDisplay.sprite = CharacterData[0].characterUI[1];
            WeaponDisplay.sprite = CharacterData[0].characterUI[2];
            Debug.Log("UI Updated with: " + CharacterData[0].characterName);
        }
        else
        {
            Debug.LogError("No CharacterData loaded, UI not updated.");
        }
    }

    private void LoadAllCharacterData()
    {
        for (int i = 0; i < playerSwitch.characterArray.Length; i++)
        {
            CharacterName = playerSwitch.characterArray[i].name;
            Debug.Log("Loading ScriptableObject for: " + CharacterName);

            // Load synchronously
            AsyncOperationHandle<CharacterData> handle = Addressables.LoadAssetAsync<CharacterData>(CharacterName);
            CharacterData loadedData = handle.WaitForCompletion();

            if (loadedData != null)
            {
                CharacterData.Add(loadedData);
                Debug.Log("Loaded SO: " + loadedData.name);
            }
            else
            {
                Debug.LogError("Failed to load ScriptableObject: " + CharacterName);
            }
        }
    }

    public void UIChange(int characterNumber)
    {
        Hue.sprite = CharacterData[characterNumber].characterUI[0];
        HealthDisplay.sprite = CharacterData[characterNumber].characterUI[1];
        WeaponDisplay.sprite = CharacterData[characterNumber].characterUI[2];
        Debug.Log("UI Updated with: " + CharacterData[characterNumber].characterName);
    }
}