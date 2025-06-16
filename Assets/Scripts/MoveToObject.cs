using UnityEngine;

public class MoveToObject : MonoBehaviour
{
    public GameObject Player;
    public GameObject Enemy;
     public float followDistance = 2f;
       public float followHeight = 2f;  
    public float followSpeed = 5f;
      public float delayFactor = 0.3f;
     public float floatAmplitude = 0.5f;
    public float floatFrequency = 1f;
     public float rotationResponse = 10f;
    private Vector3 velocity;
    private Vector3 floatOffset;
     

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
float time = Time.time;

        // Step 1: Create floating offset
        floatOffset.x = Mathf.Sin(time * floatFrequency) * floatAmplitude;           // Side to side
        floatOffset.y = Mathf.Cos(time * floatFrequency * 1.3f) * floatAmplitude;    // Up and down
        floatOffset.z = 0; // No forward/backward float

        // Step 2: Base follow position (behind and above the player)
        Vector3 basePosition = Player.transform.position 
                             - Player.transform.forward * followDistance 
                             + Vector3.up * followHeight;

        // Step 3: Apply floating offset relative to the player's orientation
        Vector3 worldFloatOffset = Player.transform.right * floatOffset.x + Vector3.up * floatOffset.y;
        Vector3 targetPosition = basePosition + worldFloatOffset;

        // Step 4: Smooth delayed movement using SmoothDamp
        Enemy.transform.position = Vector3.SmoothDamp(Enemy.transform.position, targetPosition, ref velocity, delayFactor);

        // Step 5: Calculate rotation to face the same general direction as the car but with tilt based on float offset
        float yaw = Player.transform.eulerAngles.y + 180f;                 // Face backwards
        float roll = -floatOffset.x * rotationResponse;                   // Tilt when floating left/right
        float pitch = floatOffset.y * rotationResponse * 0.5f;            // Pitch slightly up/down

        Quaternion targetRotation = Quaternion.Euler(pitch, yaw, roll);
        Enemy.transform.rotation = Quaternion.Lerp(Enemy.transform.rotation, targetRotation, followSpeed * Time.deltaTime);
    }
}
