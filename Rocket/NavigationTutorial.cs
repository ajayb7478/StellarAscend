using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System;

public class NavigationTutorial : MonoBehaviour
{
    public Canvas canvas;
    public RawImage navigationArrow;
    public Transform rocket;
    public Transform landingPad;
    public Transform sphere;
    public Vector2 screenOffset = new Vector2(20f, 20f); // Adjust the offset as needed
    public TextMeshProUGUI distanceText;
    public TextMeshProUGUI tutorialText;

    bool sphereIndicatorActive;
    float delayTimer;
    bool hasDelayed = false;
    private HeightCalculator height;
    private SpeedController speed;
    private CollisionHandler land;
    bool hasReachedHeight;
    bool isNavActive;
    public int stepNo = 0;
    float speedCheckTimer = 0f;
    bool isCompleted = false;
    float sphereDistance;
    bool successSfxPlayed;
    bool stepStartSfxPlayed;
    bool step6SFXPlayed;
    public AudioSource successNotification;
    public AudioSource stepStartSfx;
    private EngineToggle engineToggle;
    bool landingGearStatus;
    private TriggerLandingGear triggerLandingGear;


    void Start()
    {
        // Create a new GameObject for TextMeshPro and set it as a child of navigationArrow
        //NavStart();
        navigationArrow.enabled = false;
        tutorialText.text = "Welcome To the Tutorial";
        height = FindObjectOfType<HeightCalculator>();
        speed = FindObjectOfType<SpeedController>();
        land = FindObjectOfType<CollisionHandler>();
        engineToggle = FindObjectOfType<EngineToggle>();
        triggerLandingGear = FindObjectOfType<TriggerLandingGear>();
        successSfxPlayed = false;
        stepStartSfxPlayed = false;
        step6SFXPlayed = false;
    }

    void NavStart()
    {
        navigationArrow.enabled = true;
        sphereIndicatorActive = true;
    }

    void Update()
    {
        float sphereDistance = CalculateSphereDistance();
        float landingPadDistance = CalculateLandingPadDistance();
        float actualSpeed = speed.CalculateSpeed();
        float actualHeight = height.integerHeight;
        bool landed = land.isLanded;
        delayTimer += Time.deltaTime;
        landingGearStatus = triggerLandingGear.extended;
        //Debug.Log(delayTimer);
        //delayTimer += Time.deltaTime;
        WelcomeStep(0);
        MoveThrottleUpStep(actualHeight, 1);
        LiftUpLandingGearStep(landingGearStatus, 2);  // the last parameter is the step number 
        HoverStep(actualSpeed, 3);
        MoveRightStep(4);
        MoveLeftStep(5);
        GoToSphereStep(sphereDistance, 6);
        GoToLandingPadStep(landingPadDistance, 7); //Always has to be the last step
    }

    void LiftUpLandingGearStep(bool landingGearStatus, int currentStepNo)
    {
        if (stepNo == currentStepNo) // Check if 5 seconds have elapsed
        {

            tutorialText.text = "Press L to lift up the landing gear";
            if (landingGearStatus)
            {
                StartCoroutine(WaitForNextStep(currentStepNo, 2));
                StepSuccessSFX();
                tutorialText.text = "Great Job!";
            }
        }
    }

    void GoToLandingPadStep(float landingPadDistance, int currentStepNo)
    {
        if (stepNo == currentStepNo)
        {
            if (!navigationArrow.enabled)
            {
                NavStart();
            }
            tutorialText.text = "Now follow the Nav Arrow to Land by Lowering the Throttle";
            StepStartSFX();
            sphereIndicatorActive = false; // Stop SphereIndicator method
            ObjectiveIndicator(rocket, landingPad); // Run LandingIndicator method continuously
            if (landingPadDistance < 20f)
            {
                if (!step6SFXPlayed)
                {
                    stepStartSfx.Play();
                    step6SFXPlayed = true; // Set the flag to true to indicate that the SFX has been played
                }
                tutorialText.text = "Lower Landing Gear and Touch Down Smoothly";
                ObjectiveIndicator(rocket, landingPad);
            }
        }
    }

    void GoToSphereStep(float sphereDistance, int currentStepNo)
    {
        if (stepNo == currentStepNo)
        {
            if (!navigationArrow.enabled)
            {
                NavStart();
            }
            if (isCompleted == false)
            {
                StepStartSFX();
                ObjectiveIndicator(rocket, sphere);
                tutorialText.text = "Now Reach the navigation Point at an altitude";
            }
            if (sphereDistance <= 10f || isCompleted == true)
            {
                isCompleted = true;
                StepSuccessSFX();
                tutorialText.text = "Congratulations!!";
                distanceText.text = "";
                navigationArrow.enabled = false;
                StartCoroutine(WaitForNextStep(currentStepNo, 4));
            }
        }
    }

    void MoveLeftStep(int currentStepNo)
    {
        if (stepNo == currentStepNo)
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
                    StartCoroutine(WaitForNextStep(currentStepNo, 3)); //stepNo Delay
                    //speedCheckTimer = 0f;
                }
            }
            else
            {
                // Reset the speed check timer if the speed is not within the desired range
                speedCheckTimer = 0f;
            }
        }
    }

    void MoveRightStep(int currentStepNo)
    {
        if (stepNo == currentStepNo)
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
                    StartCoroutine(WaitForNextStep(currentStepNo, 3)); //stepNo Delay
                    //speedCheckTimer = 0f;
                }
            }
            else
            {
                // Reset the speed check timer if the speed is not within the desired range
                speedCheckTimer = 0f;
            }
        }
    }

    void HoverStep(float actualSpeed, int currentStepNo)
    {
        if (stepNo == currentStepNo)
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
                    StartCoroutine(WaitForNextStep(currentStepNo, 4));
                    //speedCheckTimer = 0f;
                }
            }
            else
            {
                // Reset the speed check timer if the speed is not within the desired range
                speedCheckTimer = 0f;
            }
        }
    }

    void MoveThrottleUpStep(float actualHeight, int currentStepNo)
    {
        if (stepNo == currentStepNo)
        {
            tutorialText.text = "Move the Throttle Up By Pressing 'W' key to lift off";
            StepStartSFX();
            if (actualHeight > 10)
            {
                StepSuccessSFX();
                tutorialText.text = "Congratulations!!";
                StartCoroutine(WaitForNextStep(currentStepNo, 4));
            }
        }
    }

    void WelcomeStep(int currentStepNo)
    {
        if (stepNo == currentStepNo) // Check if 5 seconds have elapsed
        {
            tutorialText.text = "Welcome To the Tutorial .. Turn on the Engine";
            if (engineToggle.textState)
            {
                StartCoroutine(WaitForNextStep(currentStepNo, 4));
                tutorialText.text = "Nice !! The Engine is now Running";
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
