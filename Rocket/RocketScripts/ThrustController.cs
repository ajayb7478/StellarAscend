using UnityEngine;
using TMPro;
public class ThrustController : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] float mainThrust = 1;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] ParticleSystem mainThrusterVFX;
    float throttle = 0f;
    AudioSource audioSource;
    public TextMeshProUGUI throttleText;

    public float Throttle { get { return throttle; } }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        throttleText.text = "Throttle: 0%";
        if (throttleText != null)
        {
            UpdateThrottleUI();
        }
    }

    public void UpdateThrottleUI()
    {
        // Assuming ThrustController has a property named Throttle
        throttleText.text = "Throttle: " + Mathf.Round(throttle * 100f) + "%";
    }

    public void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.W))
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

        rb.AddRelativeForce(Vector3.up * thrustMultiplier);

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

    void StopThrusting()
    {
        audioSource.Stop();
        mainThrusterVFX.Stop();
        throttle = 0f;
    }
}
