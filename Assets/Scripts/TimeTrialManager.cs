using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class TimeTrialManager : MonoBehaviour
{
    [Header("Player & Timing")]
    public GameObject playerCar;
    public float timeLimit = 180f;
    public float timeBonusPerLap = 20f;

    [Header("UI")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI resultText;
    public Image screenFlashImage; // Assign in inspector

    [Header("Scene Settings")]
    public string winSceneName = "NextScene";

    private int currentLap = 0;
    private float timer = 0f;
    private bool gameEnded = false;

    private Color normalColor = Color.white;
    private Color warningColor = Color.red;
    private float warningThreshold = 10f;

    void Start()
    {
        timer = 0f;
        currentLap = 0;
        resultText.text = "";
        timerText.transform.localScale = Vector3.one;
        if (screenFlashImage != null)
            screenFlashImage.color = Color.clear;
    }

    void Update()
    {
        if (gameEnded) return;

        timer += Time.deltaTime;

        float timeLeft = Mathf.Max(0, timeLimit - timer);
        timerText.text = $"Time: {timeLeft:F1}s";

        // Change text color when time is low
        timerText.color = (timeLeft <= warningThreshold) ? warningColor : normalColor;

        if (timeLeft <= 0f && currentLap < 5)
        {
            Lose();
        }
    }

    public void RegisterLap(int lapIndex)
    {
        if (gameEnded) return;

        if (lapIndex == currentLap + 1)
        {
            currentLap++;
            timeLimit += timeBonusPerLap;

            Debug.Log($"Lap {currentLap} completed. Time extended by {timeBonusPerLap} seconds.");

            StartCoroutine(BounceText(timerText));
            StartCoroutine(FlashScreen());

            if (currentLap >= 5)
            {
                Win();
            }
        }
    }

    private IEnumerator BounceText(TextMeshProUGUI text)
    {
        float duration = 0.3f;
        float bounceScale = 1.3f;

        Vector3 originalScale = Vector3.one;
        Vector3 targetScale = Vector3.one * bounceScale;

        float timer = 0f;

        while (timer < duration / 2f)
        {
            float t = timer / (duration / 2f);
            text.transform.localScale = Vector3.Lerp(originalScale, targetScale, t);
            timer += Time.deltaTime;
            yield return null;
        }

        timer = 0f;

        while (timer < duration / 2f)
        {
            float t = timer / (duration / 2f);
            text.transform.localScale = Vector3.Lerp(targetScale, originalScale, t);
            timer += Time.deltaTime;
            yield return null;
        }

        text.transform.localScale = originalScale;
    }

    private IEnumerator FlashScreen()
    {
        if (screenFlashImage == null) yield break;

        float duration = 0.5f;
        Color flashColor = new Color(0.2f, 0.6f, 1f, 0.5f); // Soft blue with transparency

        float timer = 0f;
        while (timer < duration / 2f)
        {
            screenFlashImage.color = Color.Lerp(Color.clear, flashColor, timer / (duration / 2f));
            timer += Time.deltaTime;
            yield return null;
        }

        timer = 0f;
        while (timer < duration / 2f)
        {
            screenFlashImage.color = Color.Lerp(flashColor, Color.clear, timer / (duration / 2f));
            timer += Time.deltaTime;
            yield return null;
        }

        screenFlashImage.color = Color.clear;
    }

    private void Win()
    {
        gameEnded = true;
        resultText.text = "You Win!";
        Invoke("LoadWinScene", 2f);
    }

    private void Lose()
    {
        gameEnded = true;
        resultText.text = "You Lose!";
    }

    private void LoadWinScene()
    {
        SceneManager.LoadScene(winSceneName);
    }
}
