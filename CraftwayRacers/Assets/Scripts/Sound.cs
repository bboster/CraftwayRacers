using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;

    public AudioMixerGroup mixer;

    [Range(0f, 1f), Tooltip("Value must be between 0 and 1.")]
    public float volume;

    [Range(0f, 3f), Tooltip("Value must be between 0 and 3.")]
    public float pitch;

    [HideInInspector]
    public AudioSource source;

    public bool loop;
}
