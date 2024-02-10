using UnityEngine;

public class LandingPadMovement : MonoBehaviour
{
    public float speed = 5f; // Adjust this value to control the speed of movement

    void Update()
    {
        // Calculate the movement vector
        Vector3 movement = new Vector3(speed * Time.deltaTime, 0f, 0f);

        // Apply the movement to the landing pad's position
        transform.Translate(movement, Space.World);
    }
}
