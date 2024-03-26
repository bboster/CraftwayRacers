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
    }

    public Sound GetSound(string name)
    {
        Sound s = System.Array.Find(sounds, sounds => sounds.name == name);
        return s;
    }

    public void AddAudioListener(GameObject g, string name)
    {
        Sound s = System.Array.Find(sounds, sounds => sounds.name == name);

        s.source = g.AddComponent<AudioSource>();

        s.source.clip = s.clip;
        s.source.outputAudioMixerGroup = s.mixer;
        s.source.volume = s.volume;
        s.source.pitch = s.pitch;
        s.source.loop = s.loop;
    }

    public void Play(string name, int priority)
    {
        Sound s = System.Array.Find(sounds, sounds => sounds.name == name);
        s.source.priority = priority;
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


}
