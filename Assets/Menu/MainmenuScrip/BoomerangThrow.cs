using UnityEngine;

public class BoomerangThrow : MonoBehaviour
{
    public float throwForce = 10f;
    public float returnDelay = 1f;
    public float returnSpeed = 5f;

    private Vector3 startPosition;
    private Rigidbody rb;
    private bool returning = false;

    void Start()
    {
        startPosition = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !returning)
        {
            ThrowForward();
            Invoke(nameof(StartReturn), returnDelay);
        }

        if (returning)
        {
            ReturnToStart();
        }
    }

    void ThrowForward()
    {
        rb.isKinematic = false;
        rb.linearVelocity = transform.forward * throwForce;
    }

    void StartReturn()
    {
        returning = true;
        rb.isKinematic = true; // Stop physics movement so we can move it manually
    }

    void ReturnToStart()
    {
        transform.position = Vector3.MoveTowards(transform.position, startPosition, returnSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, startPosition) < 0.1f)
        {
            returning = false;
            rb.linearVelocity = Vector3.zero;
        }
    }
}
