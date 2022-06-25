using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = .5f;
    [Range(0f, 1f)]
    public float pitch = .5f;

    public bool loop;

    [HideInInspector]
    public AudioSource source;
}
