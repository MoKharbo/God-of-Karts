using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public AudioSource audioSource;
    public AudioClip hoverClip;
    public AudioClip clickClip;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (audioSource && hoverClip)
            audioSource.PlayOneShot(hoverClip);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (audioSource && clickClip)
            audioSource.PlayOneShot(clickClip);
    }
}
