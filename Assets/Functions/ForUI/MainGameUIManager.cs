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

public class MainGameUIManager : MonoBehaviour
{
    //Reference to UI interface elements
    [SerializeField]
    private Image Hue;
    [SerializeField]
    private Image HealthDisplay;

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
        //Put the script of the gameobject into playerswitch to extract the array
        playerSwitch = ObjectWithCharacterArray.GetComponent<PlayerSwitch>();

        //scans the array, finds the correct scriptable object to grab and reference
        for (int i = 0; i < playerSwitch.characterArray.Length; i++) //loops depending on how many items are in the array
        {
            CharacterName = playerSwitch.characterArray[i].name; //puts the name of the object in the array into a variable
            Debug.Log("GameObject Name at index " + i + ": " + CharacterName);

            Addressables.LoadAssetAsync<CharacterData>(CharacterName).Completed += OnLoaded; //loads the required character data


        }

        


    }



    private void OnLoaded(AsyncOperationHandle<CharacterData> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded) //error check
        {
            CharacterData.Add(handle.Result); //adds the SO into the list
            Debug.Log("Loaded SO: " + handle.Result.name);
            Debug.Log("Added" + CharacterData[0].characterName);
            Hue.sprite = CharacterData[0].characterUI[0]; //Loads the first character's UI (apparently, OnLoaded runs after Start, making it so that the charaacter data list is empty until Start finishes and OnLoaded starts)
            HealthDisplay.sprite = CharacterData[0].characterUI[1];
        }
        else
        {
            Debug.LogError("Failed to load ScriptableObject.");
        }
    }
}
