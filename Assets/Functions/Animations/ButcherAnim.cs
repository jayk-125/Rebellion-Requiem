using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButcherAnim : MonoBehaviour
{
    // Reference butcher animator by self
    public Animation butcherAnim;

    // Start is called before the first frame update
    void Start()
    {
        // Set idle at start
        ButcherIdle();
    }

    // Play idle anim
    public void ButcherIdle()
    {
        // Start playing butcher idle
        butcherAnim.Play("Butcher_Idle");
    }

    // Play run anim
    public void ButcherRun()
    {
        // Start playing butcher run
        butcherAnim.Play("Butcher_Run");
    }

    // Play basic attack anim
    public void ButcherBasic()
    {
        // Start playing butcher basic
        butcherAnim.Play("Butcher_Basic");
    }

    // Play dash anim
    public void ButcherDash()
    {
        // Start playing butcher dash
        butcherAnim.Play("Butcher_Dash");
    }

    // Play grapple fire anim
    public void ButcherGrappleFire()
    {
        // Start playing butcher grapple fire
        butcherAnim.Play("Butcher_GrappleFire");
    }

    // Play grapple fly anim
    public void ButcherGrappleFly()
    {
        // Start playing butcher grapple fly
        butcherAnim.Play("Butcher_GrappleFly");
    }

    // Play lunge anim
    public void ButcherLunge()
    {
        // Start playing butcher lunge
        butcherAnim.Play("Butcher_Lunge");
    }

    // Update is called once per frame
    void Update()
    {
        // Reset to idle after animation ends
        if (butcherAnim.isPlaying == false)
        {
            butcherAnim.Play("Butcher_Idle");
        }
    }
}
