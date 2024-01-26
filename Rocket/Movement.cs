using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float mainThrust = 1;
    [SerializeField] float torque = 200;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] ParticleSystem rightThrustersVFX;
    [SerializeField] ParticleSystem leftThrustersVFX;
    [SerializeField] ParticleSystem mainThrusterVFX;
    [SerializeField] float maxFuel = 200f;
    [SerializeField] float fuelConsumptionRate = 10f;
    float currentFuel;
    float currentSpeed;
    float throttle = 0f; // throttle value between -1 and 1
    AudioSource audioSource;

    public TextMeshProUGUI fuelText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI throttleText; // New TextMeshProUGUI for displaying throttle range

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
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
        if (throttleText != null)
        {
            UpdateThrottleText();
        }
    }

    void Update()
    {
        ProcessThrust();
        ConsumeFuel();
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
        if (throttleText != null)
        {
            UpdateThrottleText();
        }
    }

    void UpdateFuelText()
    {
        fuelText.text = "Fuel: " + Mathf.Round(currentFuel).ToString();
    }

    void ConsumeFuel()
    {
        if (throttle > 0f)
        {
            float thrustMultiplier = mainThrust * throttle;
            currentFuel = Mathf.Max(0f, currentFuel - thrustMultiplier * fuelConsumptionRate * Time.deltaTime);
        }
    }

    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.W) && currentFuel > 0f)
        {
            IncreaseThrottle();
        }
        else if (Input.GetKey(KeyCode.S))
        {
            DecreaseThrottle();
        }

        ApplyThrust();
    }

    void IncreaseThrottle()
    {
        throttle = Mathf.Min(throttle + Time.deltaTime / 10, 1f);
    }

    void DecreaseThrottle()
    {
        throttle = Mathf.Max(throttle - Time.deltaTime / 10, 0f);
    }

    void ApplyThrust()
    {
        float thrustMultiplier = mainThrust * throttle;

        if (currentFuel > 0f)
        {
            // Consume fuel based on thrust
            float fuelConsumed = thrustMultiplier * fuelConsumptionRate * Time.deltaTime;
            currentFuel = Mathf.Max(0f, currentFuel - fuelConsumed);

            // Apply thrust
            rb.AddRelativeForce(Vector3.up * thrustMultiplier);

            Debug.Log("Force Applied: " + Vector3.up * thrustMultiplier);

            if (throttle != 0f && !audioSource.isPlaying)
            {
                audioSource.PlayOneShot(mainEngine);
            }

            if (throttle != 0f && !mainThrusterVFX.isPlaying)
            {
                mainThrusterVFX.Play();
            }

            if (throttle == 0f)
            {
                StopThrusting();
            }
        }
        else
        {
            // No fuel, stop thrusting
            StopThrusting();
        }
    }

    void StopThrusting()
    {
        audioSource.Stop();
        mainThrusterVFX.Stop();
        throttle = 0f; // Resetting the throttle to zero
    }

    void CalculateSpeed()
    {
        currentSpeed = Vector3.Dot(rb.velocity, transform.up) * 10;
    }

    void UpdateSpeedText()
    {
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

    void UpdateThrottleText()
    {
        throttleText.text = "Throttle: " + Mathf.Round(throttle * 100f) + "%";
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
