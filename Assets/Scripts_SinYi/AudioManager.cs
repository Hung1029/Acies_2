using UnityEditor.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;

    void Awake()
    {
        foreach (Sound s in sounds) 
        {
            s.source=gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.playOnAwake = s.onAwake;

        }
    }
    private void Start()
    {
        Play("Scenebg");
    }

    public void Play(string name) 
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) {
            Debug.LogWarning("Sound:" + name + "not found!");
            return; }
        s.source.Play();
    }
    public void Pause(string name) 
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound:" + name + "not found!");
            return;
        }
        s.source.Pause();
    }

    public bool isPlaying(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound:" + name + "not found!");
        }
        return s.source.isPlaying;
    }
    public void linerout(string name) { 
    
    }
}
