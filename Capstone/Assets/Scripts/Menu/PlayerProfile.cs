using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class PlayerProfile : MonoBehaviour
{
    public string profileName;
    public string playerClass;


    public PlayerProfile()
    {
        profileName = "blankName";
        playerClass = "blankClass";
    }

}
