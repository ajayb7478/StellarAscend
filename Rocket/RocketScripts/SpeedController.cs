using TMPro;
using UnityEngine;

public class SpeedController : MonoBehaviour
{
    private Rigidbody rb;
    private float currentSpeed;
    private float currentSideSpeed;
    public TextMeshProUGUI speedText;

    //public float CurrentSpeed { get { return currentSpeed; } }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentSpeed = 0f;
        if (speedText != null)
        {
            UpdateSpeedText();
        }
    }

    void Update()
    {
        CalculateSpeed();
        CalculateSideSpeed();
        UpdateSpeedText();
    }

    public float CalculateSpeed()
    {
        currentSpeed = Vector3.Dot(rb.velocity, transform.up) * 10;
        return currentSpeed;
    }
    public float CalculateSideSpeed()
    {
        currentSideSpeed = Vector3.Dot(rb.velocity, transform.right) * 10;
        return Mathf.FloorToInt(currentSideSpeed); ;
    }

    void UpdateSpeedText()
    {
        // Check if speedText is assigned before trying to use it
        if (speedText != null)
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
    }
}
