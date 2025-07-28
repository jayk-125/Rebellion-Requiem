using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    // Reference target object
    public GameObject target;

    // Update is called once per frame
    void Update()
    {
        // Follow target position
        transform.position = target.transform.position;
    }
}
