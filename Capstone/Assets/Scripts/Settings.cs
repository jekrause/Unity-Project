using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings {

    public static int NumOfPlayers { set; get; } = 1; // by default

    public static bool[] inputAssigned { set; get; } = { false, false, false, false, false}; // [keyboard, joystick 1, joystick 2, joystick 3, joystick 4] used to determine if input is assigned to a player already

    public static readonly string OS = Application.platform == RuntimePlatform.WindowsEditor? "Windows" : "Mac";

    // used for debugging
    public static bool ShowDebuggMsg = false;
    public static void PrintDebugMsg(string message)
    {
        if (ShowDebuggMsg) Debug.Log(message);
    }
}
