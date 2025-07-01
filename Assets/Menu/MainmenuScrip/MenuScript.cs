using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Debug.Log("Game has started.");
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Player has quit the game.");
    }

    public void LoadOptions()
    {
        SceneManager.LoadScene("Options"); // Make sure a scene named "Options" exists in Build Settings
        Debug.Log("Loading Options...");
    }

    public void Maps()
    {
        SceneManager.LoadScene("Maps"); // Make sure a scene named "Credits" exists in Build Settings
        Debug.Log("Loading Maps...");
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Replace with your actual main menu scene name
        Debug.Log("Returning to Main Menu...");
    }
}
