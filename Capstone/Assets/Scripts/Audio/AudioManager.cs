using UnityEngine.Audio;
using System;
using UnityEngine;

//code from Brackeys https://www.youtube.com/watch?v=6OT43pvUyfY

public class AudioManager : MonoBehaviour
{

    public static Sound[] soundsGlob;
    public  Sound[] sounds;

    //public static AudioManager instance;  //may not need this

    void Awake()
    {

        //may not need this, so commented out for now
        /*
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        */

        soundsGlob = sounds;


        foreach (Sound s in soundsGlob)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public static void Play(string name)
    {
        Sound s = Array.Find(soundsGlob, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.Play();
    }
}
