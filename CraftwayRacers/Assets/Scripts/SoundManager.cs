using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public float volume;
    public Sound[] sounds;

    public static SoundManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();

            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = s.mixer;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }

    public void Play(string name)
    {
        Sound s = System.Array.Find(sounds, sounds => sounds.name == name);
        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = System.Array.Find(sounds, sounds => sounds.name == name);
        s.source.Stop();
    }

    [Tooltip("Plays sounds in sequence for their full duration.")]
    public IEnumerator PlaySoundsInSequence(string[] toPlay)
    {
        foreach(string i in toPlay)
        {
            Sound s = System.Array.Find(sounds, sounds => sounds.name == i);
            s.source.Play();

            yield return new WaitForSeconds(s.clip.length);
        }
    }

    public IEnumerator PlayEngineSound(GameObject car, string name)
    {
        Sound s = System.Array.Find(sounds, sounds => sounds.name == name);
        s.source.Play();

        for(; ; )
        {
            //Get the car speed and relate it to the pitch.
            //s.source.pitch = //car speed relation.

            yield return new WaitForEndOfFrame();
        }
    }
}
