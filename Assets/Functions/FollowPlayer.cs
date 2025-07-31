using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    // Reference target object
    private GameObject target;

    // Awake is called before scene is started
    void Awake()
    {
        // Set target as player object
        target = GameObject.Find("/Player");
    }

    // Update is called once per frame
    void Update()
    {
        // Follow target position
        transform.position = target.transform.position;
    }
}
