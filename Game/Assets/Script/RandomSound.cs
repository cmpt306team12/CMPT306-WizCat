using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSound : MonoBehaviour
{
    [Range(0.1f, 0.5f)] float volumeModifier = 0.2f;
    [Range(0.1f, 0.5f)] float pitchModifier = 0.2f;

    public AudioSource source;
    public void PlayRandomizedSound()
    {
        source.volume = Random.Range(1.0f - volumeModifier, 1.0f);
        source.pitch = Random.Range(1.0f - pitchModifier, 1.0f + pitchModifier);
        AudioSource.PlayClipAtPoint(source.clip, gameObject.transform.position);
    }
}
