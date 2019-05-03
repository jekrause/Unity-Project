using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings {

    public static int NumOfPlayers { set; get; } = 1; // by default

    public static bool[] inputAssigned { set; get; } = { false, false, false, false, false}; // [keyboard, joystick 1, joystick 2, joystick 3, joystick 4] used to determine if input is assigned to a player already

    public static readonly string OS = Application.platform == RuntimePlatform.WindowsEditor? "Windows" : "Mac";

    public static float MasterVolume = 1.0f;  //controls volume of all audio in the game
    public static float MusicVolume = 1.0f;  //controls volume of all music in the game
    public static float SFXVolume = 1.0f;  //controls volume of all sound effects in the game

    // used for debugging
    public static bool ShowDebuggMsg = false;
    public static void PrintDebugMsg(string message)
    {
        if (ShowDebuggMsg) Debug.Log(message);
    }


    public static bool IncreaseMasterVolume()
    {
        float prevVolume = MasterVolume;
        MasterVolume = MasterVolume + 0.05f;

        if (MasterVolume > 1f)
        {
            MasterVolume = 1f;
        }

        if (prevVolume == MasterVolume)
        {
            return false;
        }

        UpdateMasterVolume();
        return true;
    }

    public static bool DecreaseMasterVolume()
    {
        float prevVolume = MasterVolume;
        MasterVolume = MasterVolume - 0.05f;

        if (MasterVolume < 0f)
        {
            MasterVolume = 0f;
        }

        if (prevVolume == MasterVolume)
        {
            return false;
        }

        UpdateMasterVolume();
        return true;
    }

    public static void UpdateMasterVolume()
    {
        GameObject audiomanager = GameObject.Find("AudioManager");
        audiomanager.GetComponent<AudioManager>().UpdateAudioManagerMasterVolume();
    }

    public static void PrintAudioVolumes()
    {
        Debug.Log("MasterVolume = "+MasterVolume);
        Debug.Log("MusicVolume = " + MusicVolume);
        Debug.Log("SFXVolume = " + SFXVolume);
    }









    public static bool IncreaseMusicVolume()
    {
        float prevVolume = MusicVolume;
        MusicVolume = MusicVolume + 0.05f;

        if (MusicVolume > 1f)
        {
            MusicVolume = 1f;
        }

        if (prevVolume == MusicVolume)
        {
            return false;
        }

        UpdateMasterVolume();
        return true;
    }

    public static bool DecreaseMusicVolume()
    {
        float prevVolume = MusicVolume;
        MusicVolume = MusicVolume - 0.05f;

        if (MusicVolume < 0f)
        {
            MusicVolume = 0f;
        }

        if (prevVolume == MusicVolume)
        {
            return false;
        }

        UpdateMasterVolume();
        return true;
    }

    /*
    public static void UpdateMusicVolume()
    {
        GameObject audiomanager = GameObject.Find("AudioManager");
        audiomanager.GetComponent<AudioManager>().UpdateAudioManagerMusicVolume();
    }
    */








    public static bool IncreaseSFXVolume()
    {
        float prevVolume = SFXVolume;
        SFXVolume = SFXVolume + 0.05f;

        if (SFXVolume > 1f)
        {
            SFXVolume = 1f;
        }

        if (prevVolume == SFXVolume)
        {
            return false;
        }

        UpdateMasterVolume();
        return true;
    }

    public static bool DecreaseSFXVolume()
    {
        float prevVolume = SFXVolume;
        SFXVolume = SFXVolume - 0.05f;

        if (SFXVolume < 0f)
        {
            SFXVolume = 0f;
        }

        if (prevVolume == SFXVolume)
        {
            return false;
        }

        UpdateMasterVolume();
        return true;
    }

    /*
    public static void UpdateSFXVolume()
    {
        GameObject audiomanager = GameObject.Find("AudioManager");
        audiomanager.GetComponent<AudioManager>().UpdateAudioManagerSFXVolume();
    }
    */

}
