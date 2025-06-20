/* 
 * Author: Loh Shau Ern Shaun
 * Date: 19/6/2025
 * Slinger skill 1 warp projectile
 * When hitting a solid object, stop projectile prematurely
 * Gets the slinger script and sends message to set current location
 * Then destroy projectile
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpProjectile : MonoBehaviour
{
    // Reference to slinger warp skill
    private SlingerWarpSkill slingerWarpSkill;
    // Reference to this projectile's rigidbody
    private Rigidbody rb;

    // Awake is called before start
    void Awake()
    {
        // Slinger Warp Skill
        slingerWarpSkill = GameObject.Find("/Player/Skill1").GetComponent<SlingerWarpSkill>();
        // Set this object's rigidbody
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Stop the bullet after a bit of moving
        StartCoroutine(StopAfterTravel());
    }

    // Update is called every frame
    void Update()
    {
        // Send the current location to the slinger warp script
        slingerWarpSkill.CurrentLocation(transform.position);
    }

    // When projectile hits a solid object
    private void OnTriggerEnter(Collider other)
    {
        // If it is under object class
        if (other.gameObject.CompareTag("Object"))
        {
            // Projectile hit something valid
            StopBullet();
        }
    }

    // Stop after a travelling for a little
    private IEnumerator StopAfterTravel()
    {
        // Wait for a little
        yield return new WaitForSeconds(0.5f);

        // Stop bullet
        StopBullet();
    }

    // Stop the bullet from moving
    private void StopBullet()
    {
        // Stop bullet travel
        rb.velocity = Vector3.zero;
    }
}
