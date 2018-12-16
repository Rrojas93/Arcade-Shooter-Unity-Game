using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);

        foreach(Sound s in sounds)
        {
            s.source = this.gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;
            s.source.volume = s.volume;
        }
    }

    public void Play(string n)
    {
        foreach(Sound s in sounds)
        {
            if (s.name == n)
            {
                s.source.Play();
            }
        }
    }

    public void StopAll()
    {
        foreach(Sound s in sounds)
        {
            s.source.Stop();
        }
    }

    public void Stop(string n)
    {
        foreach(Sound s in sounds)
        {
            if(n == s.name && s.source.isPlaying)
            {
                s.source.Stop();
            }
        }
    }

    public bool isPlaying(string n)
    {
        foreach(Sound s in sounds)
        {
            if (s.name == n)
            {
                return s.source.isPlaying;
            }            
        }
        return false;
    }

    public AudioManager getInstance()
    {
        return instance;
    }
}

