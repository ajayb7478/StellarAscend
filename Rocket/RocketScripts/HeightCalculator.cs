using UnityEngine;

public class HeightCalculator : MonoBehaviour
{
    // Initial height (you may want to set this in the inspector or through code)
    public float initialHeight;

    // Variable to store the relative height
    private float relativeHeight;
    public int integerHeight;

    void Start()
    {
        initialHeight = transform.position.y - 3.24f;
    }

    void Update()
    {
        // Call the height calculation method
        HeightCalculation();
       // Debug.Log(GetRelativeHeight());
    }

    void HeightCalculation()
    {
        // Calculate relative height
        relativeHeight = transform.position.y + initialHeight;
        integerHeight = Mathf.RoundToInt(relativeHeight);
        // Do something with the relative height value, like printing it to the console
        //Debug.Log("Relative Height: " + relativeHeight);
    }

    // Public method to get the relativeHeight value
}
