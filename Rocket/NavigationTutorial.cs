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

    bool sphereIndicatorActive;
    float delayTimer;
    bool hasDelayed = false;
    private HeightCalculator height;
    private SpeedController speed;
    private CollisionHandler land;
    bool hasReachedHeight;
    bool isNavActive;
    [SerializeField] int stepNo = 0;
    float speedCheckTimer = 0f;



    void Start()
    {
        // Create a new GameObject for TextMeshPro and set it as a child of navigationArrow
        //NavStart();
        navigationArrow.enabled = false;
        tutorialText.text = "Welcome To the Tutorial";
        height = FindObjectOfType<HeightCalculator>();
        speed = FindObjectOfType<SpeedController>();
        land = FindObjectOfType<CollisionHandler>();
    }

    void NavStart()
    {
        navigationArrow.enabled = true;
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
    }

    void Update()
    {

        float sphereDistance = CalculateSphereDistance();
        float landingPadDistance = CalculateLandingPadDistance();
        float actualSpeed = speed.CalculateSpeed();
        float actualHeight = height.GetRelativeHeight();
        bool landed = land.isLanded;
        delayTimer += Time.deltaTime;
        //delayTimer += Time.deltaTime;
        if (stepNo == 0) // Check if 5 seconds have elapsed
        {
            if (delayTimer >= 5f)
            {
                stepNo = 1;
            }
        }
        if (stepNo == 1)
        {
            tutorialText.text = "Move the Throttle Up By Pressing 'W' key to  lift off";

            if (actualHeight > 10)
            {
                stepNo = 2;
            }
        }
        if (stepNo == 2)
        {
            tutorialText.text = "Try To Hover a Bit by experimenting with the throttle";
            if (actualSpeed > -10 && actualSpeed < 10)
            {
                // Increment the speed check timer
                speedCheckTimer += Time.deltaTime;
                if (speedCheckTimer >= 4f) // Check if the speed condition has been held for at least 10 seconds
                {
                    tutorialText.text = "Good you were able to Hover";
                    StartCoroutine(DelayedIncrementStepNo(2f));
                    stepNo = 3;
                    speedCheckTimer = 0f;
                }
            }
            else
            {
                // Reset the speed check timer if the speed is not within the desired range
                speedCheckTimer = 0f;
            }
        }
        if (stepNo == 3)
        {
            tutorialText.text = "Now Use D Key to MoveRight";
            if (Input.GetKey(KeyCode.D))
            {
                speedCheckTimer += Time.deltaTime;
                if (speedCheckTimer >= 1f)
                {
                    tutorialText.text = "Good Job!";
                    stepNo = 4;
                }
            }
            else
            {
                // Reset the speed check timer if the speed is not within the desired range
                speedCheckTimer = 0f;
            }
        }
        if (stepNo == 4)
        {
            tutorialText.text = "Now Use A Key to Move Left";
            if (Input.GetKey(KeyCode.A))
            {
                speedCheckTimer += Time.deltaTime;
                if (speedCheckTimer >= 2f)
                {
                    tutorialText.text = "Good Job!";
                    stepNo = 5;
                }
            }
            else
            {
                // Reset the speed check timer if the speed is not within the desired range
                speedCheckTimer = 0f;
            }
        }
        if (stepNo == 5)
        {
            if (!navigationArrow.enabled)
            {
                NavStart();
            }
            /* StartCoroutine(DelayedIncrementStepNo(2f));
            tutorialText.text = "Now Reach the navigation Point";
            StartCoroutine(DelayedIncrementStepNo(2f)); */
            SphereIndicator();
            tutorialText.text = "Now Reach the navigation Point at an altitude";
            if (sphereDistance <= 10f)
            {
                stepNo = 6;
            }
        }
        if (stepNo == 6)
        {
            if (!navigationArrow.enabled)
            {
                NavStart();
            }
            tutorialText.text = "Now follow the Nav Arrow to Land by Lowering the Throttle";
            sphereIndicatorActive = false; // Stop SphereIndicator method
            LandingIndicator(); // Run LandingIndicator method continuously
            //Debug.Log(landingPadDistance);
            if (landingPadDistance < 10f)
            {
                tutorialText.text = "Touch Down Smoothly and Cut Throttle to 0 Press 'X' after touchdown";
                LandingIndicator();
            }
        }
    }


    private float CalculateSphereDistance()
    {
        // Check the relative height
        Vector3 relativePosition = rocket.InverseTransformPoint(sphere.position);


        float sphereDistance = Vector3.Distance(rocket.position, sphere.position);

        //Debug.Log(sphereDistance);

        return sphereDistance;
    }

    private float CalculateLandingPadDistance()
    {
        // Check the relative height
        Vector3 relativePosition = rocket.InverseTransformPoint(landingPad.position);

        float landingPadDistance = Vector3.Distance(rocket.position, landingPad.position);

        //Debug.Log(landingPadDistance);
        return landingPadDistance;
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
            distanceText.text = string.Format("{0:F2}M", distance);
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
            distanceText.text = string.Format("{0:F2}M", distance);
        }
    }

    IEnumerator DelayedIncrementStepNo(float cglock)
    {
        // Add a delay of 2 seconds
        yield return new WaitForSeconds(cglock);
        tutorialText.text = "";
        // Increment stepNo after the delay
    }

}
