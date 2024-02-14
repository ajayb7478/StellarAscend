using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class NavigationController : MonoBehaviour
{
    public Canvas canvas;
    public RawImage navigationArrow;
    public Transform rocket;
    public Transform landingPad;
    public Transform sphere;
    public Vector2 screenOffset = new Vector2(20f, 20f); // Adjust the offset as needed
    public TextMeshProUGUI distanceText;


    bool sphereIndicatorActive;

    [SerializeField] float ObjectiveHeight;

    void Start()
    {
        // Create a new GameObject for TextMeshPro and set it as a child of navigationArrow
        // Add TextMeshPro component to the new GameObject
        //  distanceText = textMeshProObject.AddComponent<TextMeshProUGUI>();
        sphereIndicatorActive = true;
    }

    void Update()
    {
        // Check the relative height
        Vector3 relativePosition = rocket.InverseTransformPoint(sphere.position);


        float sphereDistance = Vector3.Distance(rocket.position, sphere.position);

        if (sphereDistance >= 10f && sphereIndicatorActive == true)
        {
            ObjectiveIndicator(rocket, sphere);// Run SphereIndicator method
        }
        else
        {
            sphereIndicatorActive = false; // Stop SphereIndicator method
            ObjectiveIndicator(rocket, landingPad); // Run LandingIndicator method continuously
        }
    }

    void ObjectiveIndicator(Transform rocketPosition, Transform objectivePosition)
    {
        if (rocketPosition == null || objectivePosition == null || navigationArrow == null || canvas == null || distanceText == null)
            return;


        //Vector3 relativePosition = rocket.InverseTransformPoint(sphere.position);
        float distance = Vector3.Distance(rocketPosition.position, objectivePosition.position);
        Vector3 screenPos = Camera.main.WorldToScreenPoint(objectivePosition.position);

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

            distanceText.text = "" + Mathf.RoundToInt(distance) + "m";
        }
    }
}
