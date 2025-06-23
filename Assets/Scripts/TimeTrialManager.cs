using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class TimeTrialManager : MonoBehaviour
{
    [Header("Player & Timing")]
    public GameObject playerCar;
    public float timeLimit = 180f;
    public float timeBonusPerLap = 20f;

    [Header("UI")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI resultText;

    [Header("Scene Settings")]
    public string winSceneName = "NextScene";

    private int currentLap = 0;
    private float timer = 0f;
    private bool gameEnded = false;

    void Start()
    {
        timer = 0f;
        currentLap = 0;
        resultText.text = "";
        timerText.transform.localScale = Vector3.one;
    }

    void Update()
    {
        if (gameEnded) return;

        timer += Time.deltaTime;

        float timeLeft = Mathf.Max(0, timeLimit - timer);
        timerText.text = $"Time: {timeLeft:F1}s";

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

            // Add extra time for completing a lap
            timeLimit += timeBonusPerLap;

            Debug.Log($"Lap {currentLap} completed. Time extended by {timeBonusPerLap} seconds.");

            // Trigger bounce effect on timer
            StartCoroutine(BounceText(timerText));

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

        // Scale up
        while (timer < duration / 2f)
        {
            float t = timer / (duration / 2f);
            text.transform.localScale = Vector3.Lerp(originalScale, targetScale, t);
            timer += Time.deltaTime;
            yield return null;
        }

        timer = 0f;

        // Scale back down
        while (timer < duration / 2f)
        {
            float t = timer / (duration / 2f);
            text.transform.localScale = Vector3.Lerp(targetScale, originalScale, t);
            timer += Time.deltaTime;
            yield return null;
        }

        text.transform.localScale = originalScale;
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
