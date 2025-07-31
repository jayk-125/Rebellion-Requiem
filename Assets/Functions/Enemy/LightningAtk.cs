using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LightningAtk : MonoBehaviour
{
    // Reference to player
    private Transform playerRef;
    // Reference to lightning strike hitbox
    public GameObject hitbox;
    // Reference to lightning anticipation effect
    public ParticleSystem anticipateEffect;
    // Reference to lightning strike effect
    public ParticleSystem lightningEffect;
    // Reference to lightning anticipation sfx
    public AudioSource anticipateSFX;
    // Reference to lightning strike sfx
    public AudioSource lightningSFX;

    // Wind up time
    public float lightningWindUp = 10f;
    // Current timer
    private float currentTime = 0f;
    // Can chase player
    private bool allowChase = true;
    // Can strike
    private bool allowLightning = true;

    // Reference to NavMesh agent
    public UnityEngine.AI.NavMeshAgent lightning;

    // Awake is called when scene is started
    void Awake()
    {
        // Find player target in scene
        playerRef = GameObject.Find("Player").transform;
        // Disable hitbox on start
        hitbox.SetActive(false);
    }

    // Start is called before the first frame
    void Start()
    {
        // Play anticipation effect
        anticipateEffect.Play();
        // Play anticipation sfx
        anticipateSFX.Play();
    }

    // Update is called once per frame
    void Update()
    {
        // Get the updated target position of the player
        Vector3 playerTarget = playerRef.position;
        // If can chase
        if (allowChase)
        {
            // Lets lightning chase after player
            lightning.SetDestination(playerTarget);
        }

        // Timer for wind up
        if (currentTime < lightningWindUp)
        {
            // Increase timer
            currentTime += Time.deltaTime;
        }
        // Wind up time over
        else
        {
            // If lightning is allowed to be struck
            if (allowLightning)
            {
                // Carry out lightning strike effect
                LightningStrike();
            }
        }
    }

    // Activate strike effect
    private void LightningStrike()
    {
        // Disallow lightning
        allowLightning = false;

        // Stop anticipation effect
        anticipateEffect.Stop();
        // Stop anticipation sfx
        anticipateSFX.Stop();
        // Play lightning strike effect
        lightningEffect.Play();
        // Play lightning strike sfx
        lightningSFX.Play();

        // Disallow chase
        allowChase = false;
        // Stop the lightning from moving
        lightning.ResetPath();
        // Set hitbox as active
        hitbox.SetActive(true);
    }
}
