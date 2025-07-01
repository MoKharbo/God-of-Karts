using UnityEngine;

public class LapTrigger : MonoBehaviour
{
    public int lapIndex = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"Lap {lapIndex} triggered by {other.name}");
            TimeTrialManager manager = FindObjectOfType<TimeTrialManager>();
            if (manager != null)
            {
                manager.RegisterLap(lapIndex);
                gameObject.SetActive(false); // Optional: Disable after triggering
            }
        }
    }
}
