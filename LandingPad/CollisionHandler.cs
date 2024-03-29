using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;




public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 5f;
    [SerializeField] AudioClip crashSFX;
    [SerializeField] AudioClip successSFX;
    [SerializeField] ParticleSystem crashVFX;
    [SerializeField] ParticleSystem successVFX;
    AudioSource audioSource;
    bool isTransitioning = false;
    bool collisionDisabled = false;
    Rigidbody rocketRigidbody; // Reference to the Rigidbody component of the rocket
    float rocketSpeed;
    private ThrustController thrustController;
    float currentThrottle;
    float roundedThrottle;
    float zRotation;
    int zRotationInt;
    public bool isLanded { get; private set; }
    private NavigationTutorial navigationTutorial;
    int finalStep;
    int sceneId;
    private TriggerLandingGear triggerLandingGear;
    bool landingGearStatus;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rocketRigidbody = GetComponent<Rigidbody>();
        thrustController = GetComponent<ThrustController>();
        triggerLandingGear = FindObjectOfType<TriggerLandingGear>();
        sceneId = SceneManager.GetActiveScene().buildIndex;
        if (sceneId == 1)
        {
            navigationTutorial = FindObjectOfType<NavigationTutorial>();
        }
        // Get the MeshRenderer from the specified GameObjec
        // Get the reference to the Rigidbody component
        isLanded = false;
        //GetComponent<RocketPostLander>().enabled = false;
    }

    void Update()
    {
        //RespondToDebugKeys();
        //Debug.Log(isLanded);
        landingGearStatus = triggerLandingGear.extended;
        if (sceneId == 1)
        {
            finalStep = navigationTutorial.stepNo;
        }
        else
        {
            finalStep = 7;
        }
        UpdateRocketSpeed();
        currentThrottle = thrustController.Throttle;
        roundedThrottle = Mathf.Round(currentThrottle * 10f) / 10f;
        //Debug.Log(roundedThrottle);
        if (roundedThrottle == 0 && isLanded == true && !isTransitioning && zRotationInt == 0 && finalStep == 7)
        {
            StartSuccessSequence();
            //Debug.Log("Success Sequence Conditions Met!");
        }
        UpdateZRotation();
    }

    void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadnextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionDisabled = !collisionDisabled; // this will toggle collision
        }

    }

    void UpdateRocketSpeed()
    {
        // Calculate the speed in the same relative direction as the thrust
        rocketSpeed = Vector3.Dot(rocketRigidbody.velocity, transform.up) * 10;

    }

    void UpdateZRotation()
    {
        // Get the rotation euler angles of the rocket
        Vector3 rotationAngles = transform.rotation.eulerAngles;
        // Extract the rotation about the z-axis
        zRotation = rotationAngles.z;
        zRotationInt = Mathf.RoundToInt(zRotation);
        // Do something with the zRotation value, such as printing it or using it in your logic
        //Debug.Log("Z Rotation: " + zRotationInt);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (isTransitioning || collisionDisabled) { return; }

        // Check if the collision is with the ground and the speed is less than -30
        if (other.gameObject.CompareTag("Finish") && rocketSpeed > -50f && !landingGearStatus)
        {
            isLanded = true;
        }
        else if (other.gameObject.CompareTag("Finish") && rocketSpeed > -50f && landingGearStatus)
        {
            //isLanded = false;
            StartCrashSequence();
        }
        else if (other.gameObject.CompareTag("Friendly") && landingGearStatus)
        {
            StartCrashSequence();
        }

        else
        {
            switch (other.gameObject.tag)
            {
                case "Friendly":
                    //Debug.Log("This is a friendly object");
                    break;
                case "Finish":
                    StartCrashSequence();
                    break;
                default:
                    StartCrashSequence();
                    break;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Check if the previously collided object had the "Finish" tag
        if (collision.gameObject.CompareTag("Finish"))
        {
            // Set the collision flag to false
            isLanded = false;
        }
    }

    void StartSuccessSequence()
    {
        audioSource.Stop();
        isTransitioning = true;
        audioSource.volume = 1f;
        audioSource.pitch = 1f;
        audioSource.PlayOneShot(successSFX);
        successVFX.Play();
        GetComponent<FuelController>().enabled = false;
        GetComponent<SpeedController>().enabled = false;
        GetComponent<SideThrusterController>().enabled = false;
        GetComponent<ThrustController>().enabled = false;
        Invoke("LoadnextLevel", levelLoadDelay);
    }

    void StartCrashSequence()
    {
        audioSource.Stop();
        isTransitioning = true;
        audioSource.volume = 1f;
        audioSource.pitch = 1f;
        audioSource.PlayOneShot(crashSFX);
        crashVFX.Play();
        GetComponent<FuelController>().enabled = false;
        GetComponent<SpeedController>().enabled = false;
        GetComponent<SideThrusterController>().enabled = false;
        GetComponent<ThrustController>().enabled = false;
        Transform otherModel = transform.Find("rocketModel"); // Replace "OtherModelName" with the actual name of your other model
        if (otherModel != null)
        {
            otherModel.gameObject.SetActive(false);
        }
        Invoke("ReloadLevel", levelLoadDelay);
    }

    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    void LoadnextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
}
