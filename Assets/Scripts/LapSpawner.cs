using UnityEngine;

public class LapSpawner : MonoBehaviour
{
    public GameObject lapTriggerPrefab;
    public int numberOfLaps = 5;
    public Vector3 startPosition = new Vector3(0, 0.5f, 10f);
    public float spacing = 30f;

    void Start()
    {
        for (int i = 0; i < numberOfLaps; i++)
        {
            GameObject trigger = Instantiate(lapTriggerPrefab, startPosition + new Vector3(0, 0, i * spacing), Quaternion.identity);
            trigger.name = "LapTrigger_" + (i + 1);
            LapTrigger lt = trigger.AddComponent<LapTrigger>();
            lt.lapIndex = i + 1;

            BoxCollider col = trigger.AddComponent<BoxCollider>();
            col.isTrigger = true;
            col.size = new Vector3(5f, 2f, 1f); // Adjust size

            trigger.layer = LayerMask.NameToLayer("Ignore Raycast"); // Optional
        }
    }
}
