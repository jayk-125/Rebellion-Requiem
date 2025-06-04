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
    // Reference to player object
    public GameObject player;

    // Amount of time affected by knockback
    public float delay = 0.15f;
    // Strength of knockback
    public float strength = 20f;

    // When object is hurt
    public void HurtKnockback(GameObject hurtObject, GameObject sender)
    {
        // Set the hurt object's rigidbody
        rb = hurtObject.GetComponent<Rigidbody>();

        // Check if hurtobject is player
        if (hurtObject == player) 
        {
            // Disable player from actions here
            // Disable movement
            Movement movement = player.GetComponent<Movement>();
            StartCoroutine(movement.DisableMovement(0.25f));
            // Disable basic atk
            BasicAtk basicAtk = player.GetComponent<BasicAtk>();
            StartCoroutine(basicAtk.AtkDisable(0.25f));
        }
        // Check if hurt object is enemy
        else if (hurtObject.CompareTag("Enemy"))
        {
            // Apply stun when hitting enemy for a while
            try
            {
                Debug.Log("Trying rush stun");
                StartCoroutine(hurtObject.GetComponent<RushEnemyChase>().RushEnemyStunned(delay + 0.2f));
            }
            catch
            {
                try
                {
                    Debug.Log("Trying long stun");
                    StartCoroutine(hurtObject.GetComponent<LongEnemyChase>().LongEnemyStunned(delay + 0.2f));
                }
                catch
                {
                    try
                    {
                        Debug.Log("Trying ranged stun");
                        StartCoroutine(hurtObject.GetComponent<RangedEnemyChase>().RangedEnemyStunned(delay + 0.2f));
                    }
                    catch
                    {
                        Debug.Log("huh");
                    }
                }
            }
        }

        // Play the kb effect
        PlayFeedback(hurtObject,sender);
    }

    // Play the kb effect
    public void PlayFeedback(GameObject hurtObject, GameObject sender)
    {
        // Stop all other coroutines
        StopCoroutine(Reset());

        // Set the kb direction and power
        Vector3 kbDir = (hurtObject.transform.position - sender.transform.position).normalized;
        Vector3 KBpower = kbDir * strength;


        Debug.DrawLine(hurtObject.transform.position, hurtObject.transform.position + kbDir * strength, Color.blue, 3f);

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
