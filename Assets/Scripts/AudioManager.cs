using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instace;

    [SerializeField] private Sounds[] musicSounds, sfxSounds, stormlightSounds;
    [SerializeField] private AudioSource musicSource, sfxSource, stormlightSource;

    private void Awake()
    {
        if (Instace == null)
        {
            Instace = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(String name)
    {
        Sounds s = Array.Find(musicSounds, x => x.name == name);
        if(s == null)
        {
            Debug.Log("Music " + name + " not found!");
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }

    public void PlaySfx(String name)
    {
        Sounds s = Array.Find(sfxSounds, x => x.name == name);
        if(s == null)
        {
            Debug.Log("Sfx " + name + " not found!");
        }
        else
        {
            sfxSource.clip = s.clip;
            sfxSource.Play();
        }
    }

    public void PlayStormlight(String name)
    {
        Sounds s = Array.Find(stormlightSounds, x => x.name == name);
        if(s == null)
        {
            Debug.Log("Clip " + name + " not found!");
        }
        else
        {
            stormlightSource.clip = s.clip;
            stormlightSource.Play();
        }
    }

    public AudioClip GetSfxClip(String name)
    {
        Sounds s = Array.Find(sfxSounds, x => x.name == name);
        if(s == null)
        {
            Debug.Log("Sfx " + name + " not found!");
            return null;
        }
        else
        {
            return s.clip;
        }
    }

    public void StopAudioStormlight()
    {
        stormlightSource.Stop();
    }
}
