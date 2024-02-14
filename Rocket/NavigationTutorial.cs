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
    bool isCompleted = false;
    float sphereDistance;
    bool successSfxPlayed;
    bool stepStartSfxPlayed;
    bool step6SFXPlayed;
    public AudioSource successNotification;
    public AudioSource stepStartSfx;



    void Start()
    {
        // Create a new GameObject for TextMeshPro and set it as a child of navigationArrow
        //NavStart();
        navigationArrow.enabled = false;
        tutorialText.text = "Welcome To the Tutorial";
        height = FindObjectOfType<HeightCalculator>();
        speed = FindObjectOfType<SpeedController>();
        land = FindObjectOfType<CollisionHandler>();
        successSfxPlayed = false;
        stepStartSfxPlayed = false;
        step6SFXPlayed = false;
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
        //Debug.Log(delayTimer);
        //delayTimer += Time.deltaTime;
        if (stepNo == 0) // Check if 5 seconds have elapsed
        {
            tutorialText.text = "Welcome To the Tutorial";
            StartCoroutine(WaitForNextStep(0, 4));
        }
        if (stepNo == 1)
        {
            tutorialText.text = "Move the Throttle Up By Pressing 'W' key to lift off";
            StepStartSFX();
            if (actualHeight > 10)
            {
                StepSuccessSFX();
                tutorialText.text = "Congratulations!!";
                StartCoroutine(WaitForNextStep(1, 4));
            }
        }
        if (stepNo == 2)
        {
            StepStartSFX();
            tutorialText.text = "Try To Hover a Bit by maintaining the speed between -10 and 10 using 'W' and 'S' for a few seconds";
            if (actualSpeed > -10 && actualSpeed < 10 || speedCheckTimer >= 5f)
            {
                // Increment the speed check timer
                speedCheckTimer += Time.deltaTime;
                if (speedCheckTimer >= 5f) // Check if the speed condition has been held for at least 10 seconds
                {
                    StepSuccessSFX();
                    tutorialText.text = "Congratulations!!";
                    StartCoroutine(WaitForNextStep(2, 4));
                    //speedCheckTimer = 0f;
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
            tutorialText.text = "Now Use 'D' Key to Move Right";
            StepStartSFX();
            if (Input.GetKey(KeyCode.D) || speedCheckTimer >= 1f)
            {
                speedCheckTimer += Time.deltaTime;
                Debug.Log(speedCheckTimer);
                if (speedCheckTimer >= 1f)
                {
                    //speedCheckTimer = 3f;
                    StepSuccessSFX();
                    tutorialText.text = "Congratulations!!";
                    StartCoroutine(WaitForNextStep(3, 3)); //stepNo Delay
                    //speedCheckTimer = 0f;
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
            tutorialText.text = "Now Use 'A' Key to Move Left";
            StepStartSFX();
            if (Input.GetKey(KeyCode.A) || speedCheckTimer >= 1f)
            {
                speedCheckTimer += Time.deltaTime;
                Debug.Log(speedCheckTimer);
                if (speedCheckTimer >= 1f)
                {
                    //speedCheckTimer = 3f;
                    StepSuccessSFX();
                    tutorialText.text = "Congratulations!!";
                    StartCoroutine(WaitForNextStep(4, 3)); //stepNo Delay
                    //speedCheckTimer = 0f;
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
            if (isCompleted == false)
            {
                StepStartSFX();
                SphereIndicator();
                tutorialText.text = "Now Reach the navigation Point at an altitude";
            }
            if (sphereDistance <= 10f || isCompleted == true)
            {
                isCompleted = true;
                StepSuccessSFX();
                tutorialText.text = "Congratulations!!";
                distanceText.text = "";
                navigationArrow.enabled = false;
                StartCoroutine(WaitForNextStep(5, 4));
            }
        }
        if (stepNo == 6)
        {
            if (!navigationArrow.enabled)
            {
                NavStart();
            }
            tutorialText.text = "Now follow the Nav Arrow to Land by Lowering the Throttle";
            StepStartSFX();
            sphereIndicatorActive = false; // Stop SphereIndicator method
            LandingIndicator(); // Run LandingIndicator method continuously
            if (landingPadDistance < 10f)
            {
                if (!step6SFXPlayed)
                {
                    stepStartSfx.Play();
                    step6SFXPlayed = true; // Set the flag to true to indicate that the SFX has been played
                }
                tutorialText.text = "Touch Down Smoothly and Cut Throttle to 0 Press 'X' after touchdown";
                LandingIndicator();
            }
        }
    }


    private void StepSuccessSFX()
    {
        if (!successSfxPlayed)
        {
            // Play the notification audio once
            successNotification.Play();
            successSfxPlayed = true; // Set the flag to true to indicate that the audio has been played
        }
    }

    private void StepStartSFX()
    {
        if (!stepStartSfxPlayed)
        {
            // Play the notification audio once
            stepStartSfx.Play();
            stepStartSfxPlayed = true; // Set the flag to true to indicate that the audio has been played
        }
    }

    IEnumerator WaitForNextStep(int stepNumber, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (stepNo == stepNumber)
        {
            stepNo += 1;
            speedCheckTimer = 0f;
            successSfxPlayed = false;
            stepStartSfxPlayed = false;
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



}
