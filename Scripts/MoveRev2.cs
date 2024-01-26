using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRev2 : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody rb;
    [SerializeField] float mainThrust = 1; // Force multiplier for the thrust
    //[SerializeField] float rotationThrust = 2; // Force multiplier for side rotation thrust
    [SerializeField] float torque = 200;
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
                          // ProcessRotation(); // Method to call the Rotation functionality
        SideThrusterFunctions();
    }

    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.W)) // new input method of unity
        {
            StartingThrustSpace();

        }
        else if (Input.GetKey(KeyCode.S))
        {
            ReverseThrustSpace();
        }
        else
        {
            StopThrusting();
        }
    }

    void StopThrusting()
    {
        audioSource.Stop(); // stop the main thrusters VFX and SFX
        mainThrusterVFX.Stop();
    }
    void StartingThrustSpace()
    {

        rb.AddRelativeForce(Vector3.up * Time.deltaTime * mainThrust * 490); // adding relative force in the upward direction along the Y axis
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine); // playing the added audio source
        }
        if (!mainThrusterVFX.isPlaying)
        {
            mainThrusterVFX.Play(); // playing the added VFX 
        }
    }

    void ReverseThrustSpace()
    {

        rb.AddRelativeForce(Vector3.up * Time.deltaTime * mainThrust * -390); // adding relative force in the upward direction along the Y axis
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine); // playing the added audio source
        }
        if (!mainThrusterVFX.isPlaying)
        {
            mainThrusterVFX.Play(); // playing the added VFX 
        }
    }

    void SideThrusterFunctions()
    {
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddRelativeForce(Vector3.left * Time.deltaTime * mainThrust * 200);
            if (!rightThrustersVFX.isPlaying)
            {
                rightThrustersVFX.Play();
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rb.AddRelativeForce(Vector3.right * Time.deltaTime * mainThrust * 200);
            if (!leftThrustersVFX.isPlaying)
            {
                leftThrustersVFX.Play();
            }
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            //ApplyRotation(rotationThrust);
            rb.AddTorque(transform.right * torque * Time.deltaTime * 2000);
            if (!rightThrustersVFX.isPlaying)
            {
                rightThrustersVFX.Play();
            }

        }
        else if (Input.GetKey(KeyCode.E))
        {
            //ApplyRotation(-rotationThrust);
            rb.AddTorque(-transform.right * torque * Time.deltaTime * 2000);
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

}
