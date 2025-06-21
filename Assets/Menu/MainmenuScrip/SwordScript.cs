using UnityEngine;

public class SwordScript : MonoBehaviour
{
    public float spinSpeed = 720f; // Degrees per second
    public float spinDuration = 0.3f;

    private bool isSpinning = false;
    private float spinTime = 0f;
    private Renderer swordRenderer;

    void Start()
    {
        // Cache the Renderer component
        swordRenderer = GetComponent<Renderer>();
        if (swordRenderer != null)
        {
            swordRenderer.enabled = false; // Hide by default
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isSpinning)
        {
            isSpinning = true;
            spinTime = 0f;

            if (swordRenderer != null)
            {
                swordRenderer.enabled = true; // Show sword
            }
        }

        if (isSpinning)
        {
            float delta = Time.deltaTime;
            transform.Rotate(Vector3.up, spinSpeed * delta);
            spinTime += delta;

            if (spinTime >= spinDuration)
            {
                isSpinning = false;

                if (swordRenderer != null)
                {
                    swordRenderer.enabled = false; // Hide sword
                }
            }
        }
    }
}
