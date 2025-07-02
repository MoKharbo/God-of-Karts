using UnityEngine;
using System.Collections;
public class MainSound : MonoBehaviour
{
    public AudioClip sound1;
    public AudioClip sound2;

    [Range(0f, 1f)] public float volume1 = 0.5f;
    [Range(0f, 1f)] public float volume2 = 0.5f;

    private AudioSource audioSource1;
    private AudioSource audioSource2;

    void Start()
    {
        // Create and configure first audio source
        audioSource1 = gameObject.AddComponent<AudioSource>();
        audioSource1.clip = sound1;
        audioSource1.volume = volume1;
        audioSource1.playOnAwake = false;

        // Create and configure second audio source
        audioSource2 = gameObject.AddComponent<AudioSource>();
        audioSource2.clip = sound2;
        audioSource2.volume = volume2;
        audioSource2.playOnAwake = false;

        // Play both if they are assigned
        if (sound1 != null) audioSource1.Play();
        if (sound2 != null) audioSource2.Play();
    }
}
