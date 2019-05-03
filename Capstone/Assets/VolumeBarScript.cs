using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeBarScript : MonoBehaviour
{

    public int bar;

    private Image volumeBar;
    private float volumeMax = 1;
    private float currentVolume = 0;

    // Start is called before the first frame update
    void Start()
    {
        switch (bar)
        {
            case 0:
                currentVolume = Settings.MasterVolume;
                break;
            case 1:
                currentVolume = Settings.MusicVolume;
                break;
            case 2:
                currentVolume = Settings.SFXVolume;
                break;
        }
        volumeBar = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (bar)
        {
            case 0:
                if (currentVolume != Settings.MasterVolume)
                {
                    currentVolume = Settings.MasterVolume;
                }
                break;
            case 1:
                if (currentVolume != Settings.MusicVolume)
                {
                    currentVolume = Settings.MusicVolume;
                }
                break;
            case 2:
                if (currentVolume != Settings.SFXVolume)
                {
                    currentVolume = Settings.SFXVolume;
                }
                break;
        }
        volumeBar.fillAmount = (currentVolume / volumeMax);
    }
}
