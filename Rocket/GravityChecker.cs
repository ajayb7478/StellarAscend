using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityToggle : MonoBehaviour
{
    // Variable to store the initial gravity value
    private Vector3 originalGravity;

    void Start()
    {
        // Store the initial gravity value
        originalGravity = Physics.gravity;
    }

    void Update()
    {
        // Check if the 'G' key is pressed
        if (Input.GetKeyDown(KeyCode.G))
        {
            // Toggle gravity
            ToggleGravity();
        }
    }

    void ToggleGravity()
    {
        // Check if the current gravity is the same as the original gravity
        if (Physics.gravity == originalGravity)
        {
            // If it is, set gravity to zero (effectively turning it off)
            Physics.gravity = Vector3.zero;
        }
        else
        {
            // If it's not, restore the original gravity
            Physics.gravity = originalGravity;
        }
    }
}
