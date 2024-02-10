using TMPro;
using UnityEngine;

public class FuelController : MonoBehaviour
{
    float currentFuel;
    [SerializeField] float maxFuel = 200f;
    [SerializeField] float fuelConsumptionRate = 10f;
    public TextMeshProUGUI fuelText;

    public float CurrentFuel { get { return currentFuel; } }
    private ThrustController thrustController;

    void Start()
    {
        currentFuel = maxFuel;
        fuelText.text = "Fuel: 0";
        // If fuelText is still null, try finding it in the Canvas
        if (fuelText != null)
        {
            UpdateFuelText();
        }
        thrustController = GetComponent<ThrustController>();
    }

    void Update()
    {
        UpdateFuelText();
        ConsumeFuel(thrustController.Throttle);
    }

    void UpdateFuelText()
    {
        if (fuelText != null)
        {
            fuelText.text = "Fuel: " + Mathf.Round(currentFuel).ToString();
        }
    }

    void ConsumeFuel(float throttle)
    {
        if (throttle > 0f)
        {
            float fuelConsumed = throttle * fuelConsumptionRate * Time.deltaTime;
            currentFuel = Mathf.Max(0f, currentFuel - fuelConsumed);
        }

    }
}