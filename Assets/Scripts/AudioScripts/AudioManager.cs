using UnityEngine.Audio;
using UnityEngine;
using System;

/*
 * Reference: Brackeys: Introduction to AUDIO in Unity: https://www.youtube.com/watch?v=6OT43pvUyfY
 */
public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance;
    
    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else 
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        // Convert sounds in sounds array to audio sources 
        for (int i = 0; i < sounds.Length; i++) 
        {
            sounds[i].source = gameObject.AddComponent<AudioSource>();
            sounds[i].source.clip = sounds[i].clip;
            sounds[i].source.volume = sounds[i].volume;
            sounds[i].source.pitch = sounds[i].pitch;
            sounds[i].source.loop = sounds[i].loop;
        }
    }

    /*
     * Method to handle playing audio clips in the scene 
     * Params: string name - name of the audio source to be played 
     */
    public void Play(string name) 
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        
        if (s == null) 
        {
            return;
        }

        if (!s.source.isPlaying)
        {
            s.source.Play();
        }
    }

    /*
     * Method to handle stopping currently playing audio clips in the scene 
     * Params: string name - name of the audio source to be stopped 
     */
    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            return;
        }

        if (s.source.isPlaying)
        {
            s.source.Stop();
        }
    }
}
