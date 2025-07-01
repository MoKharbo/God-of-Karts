using UnityEngine;

public class BoomerangLauncher : MonoBehaviour
{
    [Header("Boomerang Settings")]
    public GameObject boomerangPrefab;
    public Transform boomerangSpawnPoint;
    public float throwForce = 100f;
    public KeyCode throwKey = KeyCode.F;
    public float cooldownTime = 4f;

    private bool canThrow = true;
    private Rigidbody kartRb;

    void Start()
    {
        kartRb = GetComponent<Rigidbody>();
        if (boomerangSpawnPoint == null)
        {
            Debug.LogWarning("Boomerang spawn point not set. Please assign one in the inspector.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(throwKey) && canThrow && boomerangPrefab != null && boomerangSpawnPoint != null && score.score >= 300)
        {
            score.score -= 300;
            ThrowBoomerang();
        }
    }

    void ThrowBoomerang()
    {
        GameObject boomerang = Instantiate(boomerangPrefab, boomerangSpawnPoint.position, boomerangSpawnPoint.rotation);
        Rigidbody boomerangRb = boomerang.GetComponent<Rigidbody>();

        // Add throw force and inherit kart velocity
        Vector3 totalVelocity = boomerangSpawnPoint.forward * throwForce + kartRb.linearVelocity;
        boomerangRb.linearVelocity = totalVelocity;

        // Assign return target
        Boomerang boomerangScript = boomerang.GetComponent<Boomerang>();
        if (boomerangScript != null)
        {
            boomerangScript.returnTarget = boomerangSpawnPoint;
        }

        canThrow = false;
        Invoke(nameof(ResetThrow), cooldownTime);
    }

    void ResetThrow()
    {
        canThrow = true;
    }
}
