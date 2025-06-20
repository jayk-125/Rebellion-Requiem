/* 
 * Author: Loh Shau Ern Shaun
 * Date: 19/6/2025
 * Butcher skill 1 grapple projectile
 * When hitting something valid, trigger valid hit effect
 * Gets the butcher script and sends message to pull player to location hit
 * Then destroy projectile
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleProjectile : MonoBehaviour
{
    // Reference to butcher grapple skill
    private ButcherGrappleSkill butcherGrappleSkill;

    // Start is called before the first frame update
    void Awake()
    {
        // Butcher Grapple Skill
        butcherGrappleSkill = GameObject.Find("/Player/Skill1").GetComponent<ButcherGrappleSkill>();
    }

    // When player hits something
    private void OnTriggerEnter(Collider other)
    {
        // If it is under object class
        if (other.gameObject.CompareTag("Object"))
        {
            // Projectile hit something valid
            RegisterValidHit();
        }
        // If it is under enemy object
        else if (other.gameObject.CompareTag("Enemy"))
        {
            // Projectile hit something valid
            RegisterValidHit();
        }
    }

    // When the projectile hit is valid
    public void RegisterValidHit()
    {
        Debug.Log("hit!");
        // Get position of grapple projectile
        Vector3 grapplePosition = gameObject.transform.position;
        // Call the valid hit function in ButcherGrappleSkill
        butcherGrappleSkill.ValidHit(grapplePosition);
        // Remove this object
        Destroy(gameObject);
    }
}
