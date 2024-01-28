using TMPro;
using UnityEngine;

public class MainController : MonoBehaviour
{
    private ThrustController thrustController;
    private FuelController fuelController;
    private SpeedController speedController;
    private SideThrusterController sideThrusterController;


    void Start()
    {
        // Assuming each controller is attached to the same GameObject as MainController
        thrustController = GetComponent<ThrustController>();
        fuelController = GetComponent<FuelController>();
        speedController = GetComponent<SpeedController>();
        sideThrusterController = GetComponent<SideThrusterController>();

    }

    void Update()
    {
        // Call methods from each controller
        thrustController.ProcessThrust(fuelController.CurrentFuel);
        thrustController.UpdateThrottleUI();
        fuelController.ConsumeFuel(thrustController.Throttle);
        fuelController.UpdateFuelText();
        speedController.CalculateSpeed();
        sideThrusterController.SideThrusterFunctions();
    }
}