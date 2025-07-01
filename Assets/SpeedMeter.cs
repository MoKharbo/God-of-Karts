using UnityEngine;

public class SpeedMeter : MonoBehaviour
{
    [SerializeField] private Carcontroller carController;   
    [SerializeField] private RectTransform needle;          

    [SerializeField] private float startX = -100f;          
    [SerializeField] private float endX = 100f;             
    [SerializeField] private float maxSpeed = 200f;         

    private const float metersPerSecondToKmH = 3.6f;

    void Update()
    {
        if (carController == null || needle == null) return;

        float speedMps = carController.GetComponent<Rigidbody>().linearVelocity.magnitude;
        float speedKmH = speedMps * metersPerSecondToKmH;

        float normalizedSpeed = Mathf.Clamp01(speedKmH / maxSpeed);

        Vector3 pos = needle.localPosition;
        pos.x = Mathf.Lerp(startX, endX, normalizedSpeed);
        needle.localPosition = pos;
    }
}
