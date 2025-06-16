using UnityEngine;

public class SpeedBoostPickup : MonoBehaviour
{
    public float accelerationMultiplier = 1.5f;   // Verhoog de acceleratie met 1.5x
    public float maxSpeedMultiplier = 1.5f;       // Verhoog de topsnelheid met 1.5x
    public float duration = 5f;                   // Boost duurt 5 seconden

    private void OnTriggerEnter(Collider other)
    {
        Kart kart = other.GetComponent<Kart>();

        if (kart != null)
        {
            // Roep de Boost-functie van de kart aan
            kart.Boost(accelerationMultiplier, maxSpeedMultiplier, duration);

            // Pickup verdwijnt
            Destroy(gameObject);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
