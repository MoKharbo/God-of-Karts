using UnityEngine;

public class Car : MonoBehaviour
{
    public GameObject wheelPrefab;
    GameObject[] wheelPrefabs = new GameObject[4];

    Vector3[] wheels = new Vector3[4];
    public Vector2 wheelDistance = new Vector2(2, 2);
    float[] oldDist = new float[4];

    float maxSuspensionLength = 3f;
    float suspensionMultiplier = 120f;
    float dampSensitivity = 500f;
    float maxDamp = 40f;

    Rigidbody rb;

    private void Awake()
    {
        for (int i = 0; i < 4; i++)
        {
            oldDist[i] = maxSuspensionLength;
            wheelPrefabs[i] = Instantiate(wheelPrefab, wheels[i], Quaternion.identity);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        wheels[0] = transform.right * wheelDistance.x + transform.forward * wheelDistance.y;
        wheels[0] = transform.right * -wheelDistance.x + transform.forward * wheelDistance.y;
        wheels[0] = transform.right * wheelDistance.x + transform.forward * -wheelDistance.y;
        wheels[0] = transform.right * -wheelDistance.x + transform.forward * -wheelDistance.y;

        for (int i = 0; i < 4; i++ )
        {
            RaycastHit hit;
            Physics.Raycast(transform.position+ wheels[i], -transform.up, out hit, maxSuspensionLength);
            if (hit.collider != null)
            {
                rb.AddForceAtPosition((Mathf.Clamp(maxSuspensionLength - hit.distance, 0, 3) * suspensionMultiplier * transform.up + transform.up * 
                    Mathf.Clamp((oldDist[i] - hit.distance) * dampSensitivity, 0, maxDamp)) * Time.deltaTime, transform.position + wheels[i]);
            }
            else
            {

            }
        }

    }
}
