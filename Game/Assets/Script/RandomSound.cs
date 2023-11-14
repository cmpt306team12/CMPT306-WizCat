using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSound : MonoBehaviour
{
    [Range(0.01f, 1.0f)][SerializeField] float maxVolume = 1.0f;
    [Range(0.1f, 0.5f)] [SerializeField] float volumeModifier = 0.2f;
    [Range(0.1f, 0.5f)] [SerializeField] float pitchModifier = 0.2f;
    [SerializeField] float maxDistance = 30.0f;

    public AudioSource PLayClipAt(AudioClip clip, Vector3 pos)
    {
        var tempGameObject = new GameObject("TempAudio");
        tempGameObject.transform.position = pos;
        AudioSource tempSrc = tempGameObject.AddComponent<AudioSource>();
        tempSrc.clip = clip;
        tempSrc.volume = Random.Range(maxVolume - volumeModifier, maxVolume);
        tempSrc.pitch = Random.Range(1.0f - pitchModifier, 1.0f + pitchModifier);
        tempSrc.rolloffMode = AudioRolloffMode.Linear;
        tempSrc.maxDistance = maxDistance;
        tempSrc.minDistance = 1f;
        tempSrc.spatialBlend = 1.0f;
        tempSrc.Play();
        Destroy(tempGameObject, tempSrc.clip.length);
        return tempSrc;
    }
}
