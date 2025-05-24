/* 
 * Author: Loh Shau Ern Shaun
 * Date: 17/5/2025
 * When something takes dmg, this is called
 * Does a kb effect
 * From the sender to the receiver direction
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackOnHurt : MonoBehaviour
{
    // Rigidbody of hurt object
    private Rigidbody rb;

    // Amount of time affected by knockback
    public float delay = 0.15f;
    // Strength of knockback
    public float strength = 20f;

    // When object is hurt
    public void HurtKnockback(GameObject hurtObject, GameObject sender)
    {
        // Set the hurt object's rigidbody
        rb = hurtObject.GetComponent<Rigidbody>();
        // Play the kb effect
        PlayFeedback(sender);
    }

    // Play the kb effect
    public void PlayFeedback(GameObject sender)
    {
        // Stop all other coroutines
        StopAllCoroutines();

        // Set the kb direction and power
        Vector3 kbDir = (transform.position - sender.transform.position).normalized;
        Vector3 KBpower = kbDir * strength;


        Debug.DrawLine(transform.position, transform.position + kbDir * strength, Color.red, 3f);

        // Apply the kb force to hurt object
        rb.AddForce(KBpower.x, 0, KBpower.z, ForceMode.Impulse);

        // Reset the kb after a while
        StartCoroutine(Reset());
    }

    // Stop kb force
    private IEnumerator Reset()
    {
        // Wait for a little
        yield return new WaitForSeconds(delay);
        // Remove rigidbody momentum
        rb.velocity = Vector3.zero;
    }
}
