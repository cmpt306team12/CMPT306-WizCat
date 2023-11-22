using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMusic : MonoBehaviour
{
    public AudioClip victory;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void changeBGM()
    {
        StartCoroutine(FadeOutAndChange());
    }

    IEnumerator FadeOutAndChange()
    {
        // Assuming a fade duration of 2 seconds, you can adjust this as needed
        float fadeDuration = 3f;
        float startVolume = audioSource.volume;

        // Gradually decrease the volume to zero
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        // Ensure the volume is set to zero to avoid any potential rounding errors
        audioSource.volume = 0;

        // Swap out the AudioClip
        audioSource.clip = victory;

        // Reset the volume to its original level
        audioSource.volume = startVolume;

        // Play the new AudioClip
        audioSource.Play();
    }
}
