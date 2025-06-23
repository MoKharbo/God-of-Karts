using UnityEngine;

public class CarResetOnMeshCollider : MonoBehaviour
{
    [Header("Respawn Settings")]
    public Transform respawnPoint;  // Drag your respawn Transform here

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the object has a MeshCollider
        MeshCollider meshCol = collision.collider as MeshCollider;
        if (meshCol != null)
        {
            ResetToTrack();
        }
    }

    void ResetToTrack()
    {
        if (respawnPoint != null)
        {
            // Reset position and rotation
            transform.position = respawnPoint.position;
            transform.rotation = respawnPoint.rotation;

            // Reset velocity
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        else
        {
            Debug.LogWarning("Respawn point not assigned.");
        }
    }
}
