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
            s.source.loop = s.loop;
        }
    }

    public Sound GetSound(string name)
    {
        Sound s = System.Array.Find(sounds, sounds => sounds.name == name);

        return s;
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

    public IEnumerator PlayEngineSound(GameObject car, string name)
    {
        Sound s = System.Array.Find(sounds, sounds => sounds.name == name);
        s.source.loop = true;
        s.source.priority = 0;
        s.source.Play();

        float oldValue = 0.5f;

        for (; ; )
        {

            //Get the car speed and relate it to the pitch.
            float f = Mathf.Clamp((Mathf.Abs(car.GetComponent<Rigidbody>().velocity.x) + Mathf.Abs(car.GetComponent<Rigidbody>().velocity.z) + 
                Mathf.Abs(car.GetComponent<Rigidbody>().velocity.y)) / 40f, 0.5f, 2f);

            s.source.pitch = Mathf.Lerp(f, oldValue, 1.5f * Time.deltaTime);

            oldValue = f;

            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator EngineStart(string name, GameObject car)
    {
        Sound s = System.Array.Find(sounds, sounds => sounds.name == name);
        s.source.Play();

        yield return new WaitForSeconds(s.clip.length);

        StartCoroutine(PlayEngineSound(car, "EngineSound"));
    }
}
