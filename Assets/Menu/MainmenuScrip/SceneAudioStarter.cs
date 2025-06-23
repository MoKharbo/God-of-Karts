using UnityEngine;

public class SceneAudioStarter : MonoBehaviour
{
    public AudioSource audioSource;

    void Start()
    {
        if (audioSource != null)
        {
            audioSource.Play();
            Debug.Log("Audio played");
        }
        else
        {
            Debug.LogWarning("No AudioSource assigned to SceneAudioStarter.");
        }
    }
}
