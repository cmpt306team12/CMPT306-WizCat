using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMusic : MonoBehaviour
{
    public AudioClip victory;

    public AudioClip gameOver;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    private void Update()
    {
        // Check static bool if game is paused
        if (PauseMenu.isPaused)
        {
            // Check whether the music is playing from audio source
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
            audioSource.Pause();
        }
        else
        {
            // Resume the music
            audioSource.UnPause();
        }
    }

    public void changeBGM()
    {
        StartCoroutine(FadeOutAndChange());
    } 

    public void CallPlayerDeath()
    {
        StartCoroutine(PlayerDeath());
    }

    IEnumerator PlayerDeath()
    {
        float fadeDuration = 1f;
        float startVolume = audioSource.volume;

        // Gradually decrease the volume to zero
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }
        audioSource.volume = 0;
        audioSource.clip = gameOver;
        audioSource.volume = startVolume;
        audioSource.Play();
        StartCoroutine(StopClipIn(gameOver.length));
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

    IEnumerator StopClipIn(float value)
    {
        yield return new WaitForSeconds(value);
        audioSource.Stop();
    }

}
