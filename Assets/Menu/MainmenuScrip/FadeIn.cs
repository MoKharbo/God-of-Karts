using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    public float fadeDuration = 1f; // Time in seconds
    private Image fadeImage;
    private float timer = 0f;

    void Start()
    {
        fadeImage = GetComponent<Image>();
        fadeImage.color = new Color(0, 0, 0, 1); // Ensure it's fully black
    }

    void Update()
    {
        if (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = 1 - (timer / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, alpha);
        }
        else
        {
            // Disable the panel after fading
            gameObject.SetActive(false);
        }
    }
}
