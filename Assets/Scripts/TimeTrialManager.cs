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
    public Image screenFlashImage;

    [Header("Scene Settings")]
    public string winSceneName = "NextScene";
    public string loseSceneName = "MainMenu";

    private int currentLap = 0;
    public float timer = 0f;
    private bool gameEnded = false;
    private bool lap5Completed = false;

    private Color normalColor = Color.white;
    private Color warningColor = Color.red;
    private float warningThreshold = 10f;

    void Start()
    {
        timer = 0f;
        currentLap = 0;
        lap5Completed = false;
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

        timerText.color = (timeLeft <= warningThreshold) ? warningColor : normalColor;

        if (timeLeft <= 0f && currentLap < 10)
        {
            Lose();
        }
    }

    public void RegisterLap(int lapIndex)
    {
        if (gameEnded) return;

        // FULL BLOCK: no laps beyond 5 allowed until lap 5 completed
        if (lapIndex > 5 && !lap5Completed)
        {
            Debug.LogWarning($"Lap {lapIndex} ignored: lap 5 not completed yet.");
            return;
        }

        if (lapIndex == currentLap + 1)
        {
            currentLap++;
            timeLimit += timeBonusPerLap;

            Debug.Log($"Lap {currentLap} completed. Time extended by {timeBonusPerLap} seconds.");

            StartCoroutine(BounceText(timerText));
            StartCoroutine(FlashScreen());

            if (currentLap == 5)
            {
                lap5Completed = true;
                Debug.Log("Lap 5 completed! Laps 6 to 10 are now active.");
            }

            if (currentLap >= 10)
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
        Color flashColor = new Color(0.2f, 0.6f, 1f, 0.5f);

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
        StartCoroutine(FadeToBlackAndReturnToMenu());
    }

    private void LoadWinScene()
    {
        SceneManager.LoadScene(winSceneName);
    }

    private IEnumerator FadeToBlackAndReturnToMenu(float fadeDuration = 2f, float waitAfterFade = 1f)
    {
        if (screenFlashImage == null)
        {
            yield return new WaitForSeconds(fadeDuration + waitAfterFade);
            SceneManager.LoadScene(loseSceneName);
            yield break;
        }

        float timer = 0f;
        Color startColor = screenFlashImage.color;
        Color targetColor = Color.black;
        targetColor.a = 1f;

        while (timer < fadeDuration)
        {
            screenFlashImage.color = Color.Lerp(startColor, targetColor, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        screenFlashImage.color = targetColor;

        yield return new WaitForSeconds(waitAfterFade);
        SceneManager.LoadScene(loseSceneName);
    }
}
