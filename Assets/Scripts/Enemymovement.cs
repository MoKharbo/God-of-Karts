using Unity.VisualScripting;
using UnityEngine;

public class Enemymovement : MonoBehaviour
{
    public Scoredrift score;
    public Carcontroller PlayerCar;
    public GameObject Player;
    public GameObject Enemy;
    public SpeedMeter speedMeter;
    public float followDistance = 2f;   // Distance behind the player
    public float followHeight = 2f;     // Base height above the player
    public float followSpeed = 5f;      // How fast to follow

    public float floatAmplitude = 0.5f; // How far it floats up/down & side to side
    public float floatFrequency = 1f;   // How fast the floating motion is

    private Vector3 floatOffset;        // Internal offset for floating
    public bool Slow = false;
    private bool HasBeenDestroyed = false;
    private void Start()
    {
            if (Player == null)
                Player = GameObject.FindGameObjectWithTag("Player"); // Assign correct tag

            if (Enemy == null)  
                Enemy = gameObject;

            if (PlayerCar == null && Player != null)
                PlayerCar = Player.GetComponent<Carcontroller>();

            if (score == null)
                score = FindObjectOfType<Scoredrift>();

            if (speedMeter == null)
                speedMeter = FindObjectOfType<SpeedMeter>();
    }
    void LateUpdate()
    {
        float time = Time.time;

        // Step 1: Calculate floating offsets using sine and cosine
        floatOffset.x = Mathf.Sin(time * floatFrequency) * floatAmplitude;               // Left-right
        floatOffset.y = Mathf.Cos(time * floatFrequency * 1.2f) * floatAmplitude;        // Up-down
        floatOffset.z = 0;                                                                // No forward/back float

        // Step 2: Base follow position (behind + above the player)
        Vector3 basePosition = Player.transform.position
                               - Player.transform.forward * followDistance
                               + Vector3.up * followHeight;

        // Step 3: Apply floating offset relative to the player's rotation
        Vector3 worldFloatOffset = Player.transform.right * floatOffset.x + Vector3.up * floatOffset.y;
        Vector3 targetPosition = basePosition + worldFloatOffset;

        // Step 4: Smoothly move the enemy to the target position
        Enemy.transform.position = Vector3.Lerp(Enemy.transform.position, targetPosition, followSpeed * Time.deltaTime);

        // Step 5: Face opposite the player's Y rotation (180° turn on Y only)
        Vector3 playerEuler = Player.transform.eulerAngles;
        Quaternion rotatedY = Quaternion.Euler(0, playerEuler.y + 180f, 0);
        Enemy.transform.rotation = Quaternion.Lerp(Enemy.transform.rotation, rotatedY, followSpeed * Time.deltaTime);
    }

    private void Update()
    {
        if (Mathf.Abs(followDistance - 2.4f) < 0.01f)
        {
            Slow = true;
        }
        else
        {
            Slow = false;
        }

        if (Slow)
        {
            PlayerCar.maxSpeed = 40f;
        }

        if (!HasBeenDestroyed && PlayerCar.isDrifting && score.score > 800 && Enemy)
        {
            GameObject.Destroy(Enemy);
            score.score -= 800;
            HasBeenDestroyed = true;
        }
    }
}
