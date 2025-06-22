using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryCollider : MonoBehaviour
{
    // Reference to butcher grapple skill
    private GolemParrySkill golemParrySkill;

    // Start is called before the first frame update
    void Awake()
    {
        // Butcher Grapple Skill
        golemParrySkill = GameObject.Find("/Player/Skill2").GetComponent<GolemParrySkill>();
    }

    // When player hits something
    private void OnTriggerStay(Collider other)
    {
        // If it is under object class
        if (other.gameObject.CompareTag("Damage"))
        {
            // Projectile hit something valid
            RegisterParry();

            // Get dmg type
            string dmgType = other.gameObject.GetComponent<HurtingObject>().type;
            // Check if is projectile type attack
            if (dmgType == "Projectile")
            {
                // Destroy projectile
                Destroy(other.gameObject);
            }
        }
    }

    // When the projectile hit is valid
    public void RegisterParry()
    {
        //Debug.Log("hit!");
        // Call the parry effect function in GolemParrySkill
        golemParrySkill.ParryEffect();
    }
}
