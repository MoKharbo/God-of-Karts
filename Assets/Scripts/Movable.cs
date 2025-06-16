using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [Tooltip("How much the background moves with the mouse")]
    public float parallaxStrength = 10f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.localPosition;
    }

    void Update()
    {
        Vector2 mousePos = Input.mousePosition;
        float moveX = (mousePos.x / Screen.width - 4.5f) * parallaxStrength;
        float moveY = (mousePos.y / Screen.height - 4.5f) * parallaxStrength;

        transform.localPosition = startPosition + new Vector3(moveX, moveY, 0);
    }
}
