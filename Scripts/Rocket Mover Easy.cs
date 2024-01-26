using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketMoverEasy : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody rb;
    [SerializeField] float mainThrust = 100; // Force multiplier for the thrust
    [SerializeField] float rotationThrust = 2; // Force multiplier for side rotation thrust
    [SerializeField] AudioClip mainEngine; // Audio clip to add for the thrusters
    [SerializeField] ParticleSystem rightThrustersVFX;  // VFX to add for the thrusters
    [SerializeField] ParticleSystem leftThrustersVFX; // VFX to add for the thrusters
    [SerializeField] ParticleSystem mainThrusterVFX; // VFX to add for the thrusters
    AudioSource audioSource; // referencing the audio source and perform fuctions like play 
    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Get the reference of the rigib body to the component\
        audioSource = GetComponent<AudioSource>(); // get the component of the the audiosource
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();  // Method to call the Thrust functionality
        ProcessRotation(); // Method to call the Rotation functionality
    }

    private void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space)) // new input method of unity
        {
            StartingThrustSpace();
        }
        else
        {
            StopThrusting();
        }
    }

    private void StopThrusting()
    {
        audioSource.Stop(); // stop the main thrusters VFX and SFX
        mainThrusterVFX.Stop();
    }

    private void ProcessRotation()
    {
        SideThrusterFunctions(); // method to do side thruster fcuntions
    }

    private void StartingThrustSpace()
    {
        rb.AddRelativeForce(Vector3.up * Time.deltaTime * mainThrust); // adding relative force in the upward direction along the Y axis

        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine); // playing the added audio source
        }
        if (!mainThrusterVFX.isPlaying)
        {
            mainThrusterVFX.Play(); // playing the added VFX 
        }
    }



    private void SideThrusterFunctions()
    {
        if (Input.GetKey(KeyCode.A))
        {
            ApplyRotation(rotationThrust);
            if (!rightThrustersVFX.isPlaying)
            {
                rightThrustersVFX.Play();
            }

        }
        else if (Input.GetKey(KeyCode.D))
        {
            ApplyRotation(-rotationThrust);
            if (!leftThrustersVFX.isPlaying)
            {
                leftThrustersVFX.Play();
            }
        }
        else
        {
            rightThrustersVFX.Stop();
            leftThrustersVFX.Stop();
        }
    }

    private void ApplyRotation(float rotationDirection)
    {
        rb.freezeRotation = true; // Freezing rotation so we can manually rotate
        transform.Rotate(Vector3.forward * Time.deltaTime * rotationDirection);
        rb.freezeRotation = false; // unfreezing rotattion
    }
}
