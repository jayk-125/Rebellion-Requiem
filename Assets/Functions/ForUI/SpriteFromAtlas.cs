/*
 Author: Jaykin Lee
 Date Created: 20/5/2025
 Date Updated: 20/5/2025
 Grabs sprite from texture atlas
 Displays sprite on UI gameobject
 This is to optimise loading issues
*/
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class SpriteFromAtlas : MonoBehaviour
{
    [SerializeField] SpriteAtlas atlas;
    [SerializeField] string spriteName;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().sprite = atlas.GetSprite(spriteName);
    }

}
