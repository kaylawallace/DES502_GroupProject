using UnityEngine.Audio;
using UnityEngine;

/*
 * Reference: Brackeys: Introduction to AUDIO in Unity: https://www.youtube.com/watch?v=6OT43pvUyfY
 */

/*
 * Class to store AudioClips to be played in the scene 
 * volume, pitch, and loop can be controlled in the Unity editor
 */
[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip; 

    [Range(0f, 1f)]
    public float volume;

    [Range(.1f, 3f)]
    public float pitch;

    public bool loop;

    [HideInInspector]
    public AudioSource source;
    
}


