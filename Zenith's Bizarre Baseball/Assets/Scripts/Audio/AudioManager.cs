using UnityEngine;
using System;
using UnityEngine.Audio;
using System.Runtime.InteropServices.WindowsRuntime;

public class AudioManager : MonoBehaviour
{
    [HideInInspector] public Sound[] sounds;
    public static AudioManager instance;

    private void Awake() {
        instance = this;
        sounds = GetComponentsInChildren<Sound>();
        foreach(Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        } 
    }

    public void Play(string name) {
        Sound s = Array.Find(sounds, sound => sound._name == name);
        if(s == null) {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.OnPlay();
        s.source.Play();
    }

    public void PlayOneShot(string name)
    {
        Sound s = Array.Find(sounds, sound => sound._name == name);
        if(s == null) {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.OnPlay();
        s.source.PlayOneShot(s.clip);
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound._name == name);
        if(s == null) {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.OnStop();
        s.source.Stop();
    }

    public void StopAll()
    {
        foreach(Sound s in sounds) {
            if(s.loop)
            {
                s.OnStop();
                s.source.Stop();
            }
        }
    }

    public AudioSource GetAudioSource(string name)
    {
        Sound s = Array.Find(sounds, sound => sound._name == name);
        if(s == null) {
            Debug.LogWarning("Sound: " + name + " not found!");
            return null;
        }
        return s.source;
    }

    public Sound GetSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound._name == name);
        if(s == null) {
            Debug.LogWarning("Sound: " + name + " not found!");
            return null;
        }
        return s;
    }
}
