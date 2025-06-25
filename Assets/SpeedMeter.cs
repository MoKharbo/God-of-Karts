using UnityEngine;
using TMPro;

public class SpeedMeter : MonoBehaviour
{
    [SerializeField] private Carcontroller carController;  // Reference to your car controller
    private TextMeshProUGUI textMeshPro;

    // Conversion factor from Unity units (m/s) to km/h
    private const float metersPerSecondToKmH = 3.6f;

    void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();

        if (carController == null)
        {
            Debug.LogError("SpeedMeter: Carcontroller reference not set!");
        }
    }

    void Update()
    {
        if (carController == null) return;

        // Get speed magnitude from carController's Rigidbody velocity
        float speedMetersPerSecond = carController.GetComponent<Rigidbody>().linearVelocity.magnitude;

        // Convert speed to km/h
        float speedKmH = speedMetersPerSecond * metersPerSecondToKmH;

        // Update UI text, round to int for display
        textMeshPro.text = "Speed: " + Mathf.RoundToInt(speedKmH).ToString() + " km/h";
    }
}
