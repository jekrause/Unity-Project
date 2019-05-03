using UnityEngine.Audio;
using System;
using UnityEngine;

//code from Brackeys https://www.youtube.com/watch?v=6OT43pvUyfY

public class AudioManager : MonoBehaviour
{
    [Range(0f,1f)]
    public float masterVolume = 1;
    [Range(0f, 1f)]
    public float musicVolume = 1;
    [Range(0f, 1f)]
    public float sfxVolume = 1;

    public string StartSongName;

    public static Sound[] soundsGlob;
    public  Sound[] sounds;

    private string OS = Settings.OS;

    //for windows gamepad checks
    public static bool[] playerAxisInUse = new bool[4];

    

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
            s.source.volume = s.volume * masterVolume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

        UpdateAudioManagerMasterVolume();
    }

    private void Start()
    {
        AudioManager.Play(StartSongName);
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

    public static void Stop(string name)
    {
        Sound s = Array.Find(soundsGlob, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        try
        {
            s.source.Stop();
        }
        catch
        {
            Debug.Log("Error on stopping sound clip");
        }
        
    }

    public static void PlayRandom(string[] names)
    {
        int randNum = UnityEngine.Random.Range(0, names.Length);

        Play(names[randNum]);


    }

    public void UpdateAudioManagerMasterVolume()
    {
        float newMasterVolume = Settings.MasterVolume;
        float newMusicVolume = Settings.MusicVolume;
        float newSFXVolume = Settings.SFXVolume;

        foreach (Sound s in soundsGlob)
        {
            s.source.clip = s.clip;

            if (s.isMusic)
            {
                s.source.volume = s.volume * newMasterVolume * newMusicVolume;
            }
            else
            {
                s.source.volume = s.volume * newMasterVolume * newSFXVolume;
            }
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    /*
    public void UpdateAudioManagerMusicVolume()
    {
        float newMusicVolume = Settings.MusicVolume;

        foreach (Sound s in soundsGlob)
        {
            if (s.isMusic)
            {
                s.source.clip = s.clip;
                s.source.volume = s.volume * newMusicVolume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
            }
        }
    }

    public void UpdateAudioManagerSFXVolume()
    {
        float newSFXVolume = Settings.MusicVolume;

        foreach (Sound s in soundsGlob)
        {
            if (!s.isMusic)
            {
                s.source.clip = s.clip;
                s.source.volume = s.volume * newSFXVolume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
            }
        }
    }
    */





}
