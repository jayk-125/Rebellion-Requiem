/*
 * Author: Jaykin Lee
 * Date Created: 7/6/2025
 * Data Container for everything about the character
 * For optimisation and to simplify other scripts
 */
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "ScriptableObjects/CharacterData", order = 1)]
public class CharacterData : ScriptableObject
{
    //Character's name
    public string characterName;
    //All UI that involves the character
    public List<Sprite> characterUI;

    //PS Shaun, although it is up to you, I highly reccomend using this format as it is the foundation of many character-switch games out there,
    //and offers us more freedom in our choice of character and whatnot. you can add other functions in this like basicAtk scripts, 3d models, etc.
    //if need be, just ask me for assistance for any work if u want to rework it. okay thaaanks
    

    

}

