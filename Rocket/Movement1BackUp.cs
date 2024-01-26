using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Movement1BackUp : MonoBehaviour
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
    [SerializeField] float maxFuel = 200f; // Maximum fuel capacity
    [SerializeField] float fuelConsumptionRate = 10f; // Rate at which fuel is consumed
    float currentFuel;
    float currentSpeed;
    AudioSource audioSource; // referencing the audio source and perform fuctions like play 

    public TextMeshProUGUI fuelText;
    public TextMeshProUGUI speedText;
    //public GameObject capsuleWithCube;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Get the reference of the rigib body to the component\
        audioSource = GetComponent<AudioSource>(); // get the component of the the audiosource
        currentFuel = maxFuel;
        if (fuelText != null)
        {
            UpdateFuelText();
        }
        currentSpeed = 0f;
        if (speedText != null)
        {
            UpdateSpeedText();
        }
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();  // Method to call the Thrust functionality                         //ProcessRotation(); // Method to call the Rotation functionality
        SideThrusterFunctions();
        CalculateSpeed();
        if (fuelText != null)
        {
            UpdateFuelText();
        }
        if (speedText != null)
        {
            UpdateSpeedText();
        }
    }



    void UpdateFuelText()
    {
        // Update the TextMeshPro Text with the current fuel level
        fuelText.text = "Fuel: " + Mathf.Round(currentFuel).ToString();
    }

    void ConsumeFuel()
    {
        currentFuel = Mathf.Max(0f, currentFuel - fuelConsumptionRate * Time.deltaTime);
    }

    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.W) && currentFuel > 0f) // new input method of unity
        {
            StartingThrustSpace();
            ConsumeFuel();

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

    void CalculateSpeed()
    {
        // Calculate the speed in the same relative direction as the thrust
        currentSpeed = Vector3.Dot(rb.velocity, transform.up) * 10;
    }

    void UpdateSpeedText()
    {
        // Update the TextMeshPro Text with the current speed
        string speedString = Mathf.Round(currentSpeed).ToString();
        if (currentSpeed < -30f)
        {
            speedText.text = "Speed: <color=red>" + speedString + "</color>";
        }
        else
        {
            speedText.text = "Speed: <color=green>" + speedString + "</color>";
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
        else if (Input.GetKey(KeyCode.E))
        {

            rb.AddTorque(transform.right * torque * Time.deltaTime * 800);
            if (!rightThrustersVFX.isPlaying)
            {
                rightThrustersVFX.Play();
            }

        }
        else if (Input.GetKey(KeyCode.Q))
        {

            rb.AddTorque(-transform.right * torque * Time.deltaTime * 800);
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
