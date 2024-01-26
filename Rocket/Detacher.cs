using UnityEngine;

public class Detacher : MonoBehaviour
{
    public GameObject cubeToDetach;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DetachCube();
        }
    }

    void DetachCube()
    {
        if (cubeToDetach != null)
        {
            // Remove the Cube from the Rocket's hierarchy
            cubeToDetach.transform.parent = null;

            // Add a Rigidbody to the detached Cube if it doesn't have one
            Rigidbody cubeRigidbody = cubeToDetach.GetComponent<Rigidbody>();
            if (cubeRigidbody == null)
            {
                cubeRigidbody = cubeToDetach.AddComponent<Rigidbody>();
                cubeRigidbody.useGravity = true;
            }
        }
    }
}
