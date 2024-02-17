using System.Collections;
using UnityEngine;

public class TriggerLandingGear : MonoBehaviour
{
    public Animator animator;
    public AudioSource gearDownSFX;
    public AudioSource gearUpSFX;
    public bool extended = false;
    bool canToggle = true;

    bool ldgDownPlayed;
    bool ldgUpPlayed;

    void Start()
    {
        ldgDownPlayed = false;
        ldgUpPlayed = false;
    }
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
        yield return new WaitForSeconds(4f);
        canToggle = true;
    }
    void ToggleLandingGear()
    {
        if (!extended)
        {
            GearUpSFXPlay();
            animator.Play("LdgUp");
            extended = true;
            ldgDownPlayed = false;

        }
        else
        {
            GearDownSFXPlay();
            animator.Play("LdgDown");
            extended = false;
            ldgUpPlayed = false;
        }
    }

    void GearUpSFXPlay()
    {
        if (!ldgUpPlayed)
        {
            ldgUpPlayed = true;
            gearUpSFX.Play();
        }
    }
    void GearDownSFXPlay()
    {
        if (!ldgDownPlayed)
        {
            ldgDownPlayed = true;
            gearDownSFX.Play();
        }
    }
}

