using UnityEngine;
using System.Collections;
using TMPro;
public class ThrustController : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] float mainThrust = 1;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioSource engineStart;
    [SerializeField] AudioSource engineStop;
    [SerializeField] ParticleSystem mainThrusterVFX;
    float throttle = 0f;
    AudioSource audioSource;
    public TextMeshProUGUI throttleText;
    public float Throttle { get { return throttle; } }
    private FuelController fuelController;
    private EngineToggle engineToggle;
    private HeightCalculator heightCalculator;
    private bool isEngineOn = false;
    private bool isDelayed = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        engineToggle = FindObjectOfType<EngineToggle>();
        heightCalculator = FindObjectOfType<HeightCalculator>();
        throttleText.text = "Throttle: 0%";
        if (throttleText != null)
        {
            UpdateThrottleUI();
        }
        fuelController = GetComponent<FuelController>();
    }

    void Update()
    {
        Debug.Log(heightCalculator.integerHeight);
        if (engineToggle.textState)
        {
            if (!isEngineOn)
            {
                isEngineOn = true;
                engineStart.Play();
                StartCoroutine(DelayedThrottle(1.2f)); // DelayedThrottle coroutine will set throttle to 0.4f after 2 seconds
            }
            ProcessThrust(fuelController.CurrentFuel);
            UpdateThrottleUI();
        }
        else
        {
            if (isEngineOn)
            {
                engineStop.Play();      // DelayedThrottle coroutine will set throttle to 0.4f after 2 seconds
            }
            isEngineOn = false;
            StartCoroutine(DelayedThrottleOff(2.8f)); // Reset the flag when engine is turned off
        }

    }

    private IEnumerator DelayedThrottleOff(float delay)
    {
        if (isDelayed)
        {
            isDelayed = false;
            yield return new WaitForSeconds(delay);
            StopThrusting();
            UpdateThrottleUI();
        }
    }

    private IEnumerator DelayedThrottle(float delay)
    {
        if (!isDelayed)
        {
            isDelayed = true;
            yield return new WaitForSeconds(delay);
            throttle = 0.4f;
        }
    }

    void UpdateThrottleUI()
    {
        // Assuming ThrustController has a property named Throttle
        throttleText.text = "Throttle: " + Mathf.Round(throttle * 100f) + "%";
    }

    void ProcessThrust(float fuel)
    {
        if (Input.GetKey(KeyCode.W))
        {
            if (fuel > 0)
            {
                IncreaseThrottle();
            }
        }
        else if (Input.GetKey(KeyCode.S))
        {
            if (fuel > 0)
            {
                DecreaseThrottle();
            }
        }
        else if (Input.GetKey(KeyCode.X))
        {
            StopThrusting();
        }
        else if (fuel == 0)
        {
            StopThrusting();
        }
        ApplyThrust();
    }

    void IncreaseThrottle()
    {
        throttle = Mathf.Min(throttle + Time.deltaTime / 3, 1f);
    }

    void DecreaseThrottle()
    {
        throttle = Mathf.Max(throttle - Time.deltaTime / 3, 0.3f);
    }

    void ApplyThrust()
    {
        float thrustMultiplier = mainThrust * throttle;

        rb.AddRelativeForce(Vector3.up * thrustMultiplier * Time.deltaTime * 490);
        // Calculate the desired volume based on thrust
        float desiredVolume = Mathf.Lerp(0.4f, 1f, throttle); // Adjust the volume range as needed
        // Calculate the desired pitch based on thrust
        float desiredPitch = Mathf.Lerp(0.5f, 1f, throttle); // Adjust the pitch range as needed
        // Set the volume and pitch of the audio source
        audioSource.volume = desiredVolume;
        audioSource.pitch = desiredPitch;

        if (throttle != 0f && !audioSource.isPlaying)
        {
            audioSource.volume = desiredVolume;
            audioSource.pitch = desiredPitch;
            audioSource.PlayOneShot(mainEngine);
        }

        if (throttle != 0f && !mainThrusterVFX.isPlaying)
        {
            mainThrusterVFX.Play();
        }

        float desiredScale = Mathf.Lerp(0.3f, 1f, throttle); // Adjust the range as needed

        // Access the transform of the Particle System and set its local scale
        mainThrusterVFX.transform.localScale = new Vector3(desiredScale, desiredScale, desiredScale);

        ApplyScaleRecursively(mainThrusterVFX.transform, desiredScale);

        if (throttle == 0f)
        {
            StopThrusting();
        }
    }

    void ApplyScaleRecursively(Transform parentTransform, float scale)
    {
        foreach (Transform child in parentTransform)
        {
            child.localScale = new Vector3(scale, scale, scale);
            ApplyScaleRecursively(child, scale);
        }
    }

    void StopThrusting()
    {
        audioSource.Stop();
        mainThrusterVFX.Stop();
        throttle = 0f;
    }
}
