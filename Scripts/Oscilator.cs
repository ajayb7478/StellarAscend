using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Oscilator : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 startingPosition;
    [SerializeField]Vector3 movementVector;
    [SerializeField][Range(0,1)]float movementFactor; //adding a slider in the inspector
    [SerializeField]float period = 2f;
    void Start()
    {
        startingPosition = transform.position; 

    }

    // Update is called once per frame
    void Update()
    {
        if(period <= Mathf.Epsilon) // epsilon is the smallest number try to use this instead of 0 
        {
            return;
        }
        float cycles = Time.time / period; // continually growing over time
        const float tau = Mathf.PI * 2; // constant value of 6.283
        float rawSinWave = Mathf.Sin(cycles * tau); // going from -1 to 1 
        movementFactor = (rawSinWave + 1f) / 2f; // recalculated to go from 0 to 1 so it's cleaner
        Vector3 offset = movementFactor * movementVector;
        transform.position = startingPosition + offset;
    }
}
