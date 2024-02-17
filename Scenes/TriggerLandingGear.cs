using System.Collections;
using UnityEngine;

public class TriggerLandingGear : MonoBehaviour
{
    public Animator animator;
    public bool extended = false;
    bool canToggle = true;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L) && canToggle)
        {
            StartCoroutine(ToggleDelay());
        }
    }
    IEnumerator ToggleDelay()
    {
        canToggle = false;
        ToggleLandingGear();
        yield return new WaitForSeconds(1f);
        canToggle = true;
    }
    void ToggleLandingGear()
    {
        if (!extended)
        {
            animator.Play("LandingGearUp");
            extended = true;
        }
        else
        {
            animator.Play("LandingGearDown");
            extended = false;
        }
    }
}


/* else
{
    animator.Play("LandingGearRetracted");
    extended = false;
} */