using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NavigationController : MonoBehaviour
{
    public Canvas canvas;
    public RawImage navigationArrow;
    public Transform rocket;
    public Transform landingPad;
    public Vector2 screenOffset = new Vector2(20f, 20f); // Adjust the offset as needed
    public float horizontalTextOffset = 30f; // Adjust the horizontal offset
    public float verticalTextOffset = 30f; // Adjust the vertical offset

    private TextMeshProUGUI distanceText;

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
    }

    void Update()
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
            distanceText.text = string.Format("{0:F2} M", distance);
        }
    }
}
