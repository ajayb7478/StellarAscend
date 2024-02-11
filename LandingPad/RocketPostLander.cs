using UnityEngine;


public class RocketPostLander : MonoBehaviour
{
    public float speed = 5f; // Adjust this value to control the speed of movement

    private bool isColliding = false; // Flag to track collision status

    void Update()
    {
        // Check if the rocket is currently colliding with an object tagged as "Finish"
        if (isColliding)
        {
            // If it is colliding, move the rocket
            MoveRocket();
        }
    }

    void MoveRocket()
    {
        Vector3 movement = new Vector3(speed * Time.deltaTime, 0f, 0f);
        transform.Translate(movement, Space.World);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object has the "Finish" tag
        if (collision.gameObject.CompareTag("Finish"))
        {
            // Set the collision flag to true
            isColliding = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Check if the previously collided object had the "Finish" tag
        if (collision.gameObject.CompareTag("Finish"))
        {
            // Set the collision flag to false
            isColliding = false;
        }
    }
}
