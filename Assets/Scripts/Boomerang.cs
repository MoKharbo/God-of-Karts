using UnityEngine;

public class Boomerang : MonoBehaviour
{
    public Transform returnTarget;
    public float returnDelay = 0.5f;
    public float returnSpeed = 100f;
    public float spinSpeed = 1000f; // horizontal spin

    private Rigidbody rb;
    private bool isReturning = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Set rotation to horizontal (flat like a boomerang)
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);

        // Delay before return starts
        Invoke(nameof(StartReturn), returnDelay);
    }

    void FixedUpdate()
    {
        // Spin around local right axis (x-axis) — horizontal spin
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, 0f, spinSpeed * Time.fixedDeltaTime));
    }

    void StartReturn()
    {
        isReturning = true;
        rb.linearVelocity = Vector3.zero;
        rb.useGravity = false;
        rb.angularVelocity = Vector3.zero;
    }

    void Update()
    {
        if (isReturning && returnTarget != null)
        {
            Vector3 direction = (returnTarget.position - transform.position).normalized;
            float step = returnSpeed * Time.deltaTime;
            transform.position += direction * step;

            if (Vector3.Distance(transform.position, returnTarget.position) < 0.5f)
            {
                Destroy(gameObject);
            }
        }
    }
}
