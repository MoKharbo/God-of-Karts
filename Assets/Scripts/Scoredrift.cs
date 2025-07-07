using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Scoredrift : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private Carcontroller Player;
    public int score;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Player.isDrifting && Player.gasInput >= 1)
        {
            score += 1;
        }
        
        if (scoreText != null)
        {
            scoreText.text = "Drift Score: " + score;
        }
    }
}
