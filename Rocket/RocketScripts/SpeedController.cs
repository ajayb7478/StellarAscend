using TMPro;
using UnityEngine;

public class SpeedController : MonoBehaviour
{
    private Rigidbody rb;
    private float currentSpeed;
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

    public void CalculateSpeed()
    {
        currentSpeed = Vector3.Dot(rb.velocity, transform.up) * 10;
        UpdateSpeedText();
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
