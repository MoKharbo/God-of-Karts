using UnityEngine;

public class destroyenemy : MonoBehaviour
{
    public float shakeAmount = 0.1f;
    public float shakeSpeed = 10f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float shakeOffset = Mathf.Sin(Time.time * shakeSpeed) * shakeAmount;

        // Shake left-right relative to enemy's orientation
        transform.position = startPosition + transform.right * shakeOffset;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Axe"))
        {
            Destroy(gameObject);
        }
    }
}