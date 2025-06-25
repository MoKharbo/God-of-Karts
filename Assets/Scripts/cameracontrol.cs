using UnityEngine;

public class cameracontrol : MonoBehaviour
{
    [SerializeField] private Transform target;
    private Rigidbody rb;
    [SerializeField] Vector3 Offset;
    [SerializeField] float speed = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = target.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 playerForward = (rb.linearVelocity + target.forward).normalized;
        transform.position = Vector3.Lerp(transform.position, target.position + target.transform.TransformVector(Offset) + playerForward * (-5f), speed * Time.deltaTime);
        transform.LookAt(target);
    }
}
