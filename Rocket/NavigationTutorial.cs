using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class NavigationTutorial : MonoBehaviour
{
    public Canvas canvas;
    public RawImage navigationArrow;
    public Transform rocket;
    public Transform landingPad;
    public Transform sphere;
    public Vector2 screenOffset = new Vector2(20f, 20f); // Adjust the offset as needed
    public float horizontalTextOffset = 30f; // Adjust the horizontal offset
    public float verticalTextOffset = 30f; // Adjust the vertical offset done
    public float delayBetweenIndicators = 2f;
    private TextMeshProUGUI distanceText;

    public TextMeshProUGUI tutorialText;
    private bool isColliding = true;

    bool sphereIndicatorActive;
    float delayTimer;
    bool hasDelayed = false;



    void Start()
    {
        // Create a new GameObject for TextMeshPro and set it as a child of navigationArrow
        GameObject textMeshProObject = new GameObject("DistanceText");
        textMeshProObject.transform.SetParent(navigationArrow.transform, false);
        // Add TextMeshPro component to the new GameObject
        distanceText = textMeshProObject.AddComponent<TextMeshProUGUI>();
        // You can customize TextMeshPro settings here
        // For example:
        distanceText.fontSize = 16;
        distanceText.color = Color.white;
        distanceText.alignment = TextAlignmentOptions.Center;
        sphereIndicatorActive = true;
        tutorialText.text = "Welcome To the Tutorial";

    }

    void Update()
    {
        float sphereDistance = CalculateDistance();

        if (!hasDelayed)
        {
            delayTimer += Time.deltaTime;
            Debug.Log(isColliding);
            if (delayTimer >= 5f) // Check if 5 seconds have elapsed
            {
                tutorialText.text = "Move the Throttle Up slowly to lift off";
                hasDelayed = true; // Set flag to indicate delay has occurred
                if (!isColliding)
                {
                    tutorialText.text = "Try To Hover a Bit by experimenting with the throttle";
                }
            }

            if (sphereDistance >= 10f && sphereIndicatorActive == true)
            {
                SphereIndicator(); // Run SphereIndicator method
            }
            else
            {
                sphereIndicatorActive = false; // Stop SphereIndicator method
                LandingIndicator(); // Run LandingIndicator method continuously
            }
        }
    }

    private float CalculateDistance()
    {
        // Check the relative height
        Vector3 relativePosition = rocket.InverseTransformPoint(sphere.position);


        float sphereDistance = Vector3.Distance(rocket.position, sphere.position);

        //Debug.Log(sphereDistance);

        return sphereDistance;
    }

    void SphereIndicator()
    {
        if (rocket == null || sphere == null || navigationArrow == null || canvas == null || distanceText == null)
            return;


        Vector3 relativePosition = rocket.InverseTransformPoint(sphere.position);


        float distance = Vector3.Distance(rocket.position, sphere.position);


        Vector3 screenPos = Camera.main.WorldToScreenPoint(sphere.position);

        // If the landing pad is behind the camera, flip the arrow
        if (screenPos.z < 0)
        {
            screenPos *= -1f;
        }

        // Apply offset from the edge of the screen
        screenPos.x = Mathf.Clamp(screenPos.x + screenOffset.x, screenOffset.x, Screen.width - screenOffset.x);
        screenPos.y = Mathf.Clamp(screenPos.y + screenOffset.y, screenOffset.y, Screen.height - screenOffset.y);

        // Set the arrow position to the adjusted screen position
        navigationArrow.rectTransform.position = new Vector3(screenPos.x, screenPos.y, screenPos.z);

        // Set the position of the TextMeshPro text below and to the right of the RawImage
        if (distanceText != null)
        {
            distanceText.rectTransform.position = new Vector3(screenPos.x + horizontalTextOffset, screenPos.y - verticalTextOffset, screenPos.z);
            distanceText.text = string.Format("{0:F2}M : Go To This Nav Point", distance);
        }
    }

    void LandingIndicator()
    {
        if (rocket == null || landingPad == null || navigationArrow == null || canvas == null || distanceText == null)
            return;

        // Get the relative position of the landing pad with respect to the rocket
        Vector3 relativePosition = rocket.InverseTransformPoint(landingPad.position);

        // Calculate the distance from the rocket to the landing pad
        float distance = Vector3.Distance(rocket.position, landingPad.position);

        // Get the position of the landing pad in screen space
        Vector3 screenPos = Camera.main.WorldToScreenPoint(landingPad.position);

        // If the landing pad is behind the camera, flip the arrow
        if (screenPos.z < 0)
        {
            screenPos *= -1f;
        }

        // Apply offset from the edge of the screen
        screenPos.x = Mathf.Clamp(screenPos.x + screenOffset.x, screenOffset.x, Screen.width - screenOffset.x);
        screenPos.y = Mathf.Clamp(screenPos.y + screenOffset.y, screenOffset.y, Screen.height - screenOffset.y);

        // Set the arrow position to the adjusted screen position
        navigationArrow.rectTransform.position = new Vector3(screenPos.x, screenPos.y, screenPos.z);

        // Set the position of the TextMeshPro text below and to the right of the RawImage
        if (distanceText != null)
        {
            distanceText.rectTransform.position = new Vector3(screenPos.x + horizontalTextOffset, screenPos.y - verticalTextOffset, screenPos.z);
            distanceText.text = string.Format("{0:F2}M Land on the Landing Pad", distance);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Check if the previously collided object had the "Finish" tag
        if (collision.gameObject.CompareTag("Friendly"))
        {
            // Set the collision flag to false
            isColliding = false;
        }
    }
}
