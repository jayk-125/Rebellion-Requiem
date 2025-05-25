using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    // Reference player object
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        // Assign player object as player
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // Follow player position
        transform.position = player.transform.position;
    }
}
