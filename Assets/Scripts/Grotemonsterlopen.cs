using UnityEngine;
using System.Collections;

public class Grotemonsterlopen : MonoBehaviour
{
    public Transform start;
    public Transform end;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float rotationSpeed = 2f;
    [SerializeField] private float waitTime = 1f;

    private Transform target;
    private bool isWaiting = false;
    private Quaternion originalRotation;
    private Quaternion targetRotation;

    void Start()
    {
        target = end;
        originalRotation = transform.rotation;
        targetRotation = Quaternion.Euler(0f, 180f, 0f) * originalRotation;
    }

    void Update()
    {
        if (isWaiting) return;
        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, target.position) < 0.01f)
        {
            StartCoroutine(RotateAndWait());
        }
    }

    private IEnumerator RotateAndWait()
    {
        isWaiting = true;
        Quaternion fromRotation = transform.rotation;
        Quaternion toRotation = (target == end) ? targetRotation : originalRotation;
        float elapsed = 0f;
        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime * rotationSpeed;
            transform.rotation = Quaternion.Lerp(fromRotation, toRotation, elapsed);
            yield return null;
        }
        transform.rotation = toRotation;
        yield return new WaitForSeconds(waitTime);
        target = (target == end) ? start : end;
        isWaiting = false;
    }
}
