using UnityEngine.Audio;
using System;
using UnityEngine;

//code from Brackeys https://www.youtube.com/watch?v=6OT43pvUyfY

public class AudioManager : MonoBehaviour
{
    [Range(0f,1f)]
    public float masterVolume = 1;

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


    public void AdjustMasterVolume(int playerIndex)
    {
        float masterVolumePrev = masterVolume;

        if (MenuInputSelector.menuControl[playerIndex] != null)
        {
            if (MenuInputSelector.menuControl[playerIndex].inputType == InputType.KEYBOARD)
            {


                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    masterVolume = AdjustAmount(masterVolume, 0.05f);
                }

                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    masterVolume = AdjustAmount(masterVolume, -0.05f);
                }
            }
            else
            {
                if (OS.Equals("Mac"))
                {

                    if (Input.GetButtonDown(MenuInputSelector.menuControl[playerIndex].DPadRight_Mac))
                    {
                        masterVolume = AdjustAmount(masterVolume, 0.05f);
                    }

                    if (Input.GetButtonDown(MenuInputSelector.menuControl[playerIndex].DPadLeft_Mac))
                    {
                        masterVolume = AdjustAmount(masterVolume, -0.05f);
                    }
                }
                else   //for xbox(windows) and PS4
                {
                    if (!playerAxisInUse[0] && MenuInputSelector.menuControl[playerIndex] != null)
                    {
                        playerAxisInUse[0] = true;


                        if (Input.GetAxis(MenuInputSelector.menuControl[playerIndex].DPadX_Windows) > 0)
                        {
                            masterVolume = AdjustAmount(masterVolume, 0.05f);
                        }

                        if (Input.GetAxis(MenuInputSelector.menuControl[playerIndex].DPadX_Windows) < 0)
                        {
                            masterVolume = AdjustAmount(masterVolume, -0.05f);
                        }

                    }

                    if (MenuInputSelector.menuControl[0] != null
                        && (Input.GetAxis(MenuInputSelector.menuControl[playerIndex].DPadY_Windows) == 0 && Input.GetAxis(MenuInputSelector.menuControl[playerIndex].DPadX_Windows) == 0))
                    {
                        // if player is not pressing any axis, reset boolean to allow us to check user input again. 
                        // The player shouldn't be pressing more than 1 D-Pad button at the same time when searching.
                        playerAxisInUse[playerIndex] = false;
                    }
                }
            }

        }


        if (masterVolume != masterVolumePrev)
        {
            Awake(); //reupdate volume values
        }
        
    }

    private float AdjustAmount(float value, float amount)
    {
        return value = value + amount;
    }

    

   

}
