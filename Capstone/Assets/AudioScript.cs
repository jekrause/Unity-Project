using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{

    public AudioClip MoveSoundClip;
    public AudioClip SelectSoundClip;
    public AudioClip GoBackSoundClip;
    public AudioSource SoundSource;

    // Start is called before the first frame update
    void Start()
    {
        //SoundSource.clip = SoundClip;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playSelectSound();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) || 
                    Input.GetKeyDown(KeyCode.DownArrow) ||
                    Input.GetKeyDown(KeyCode.LeftArrow) ||
                    Input.GetKeyDown(KeyCode.RightArrow))
        {
            playMoveSound();
        }

    }

    void playMoveSound()
    {
        SoundSource.clip = MoveSoundClip;
        SoundSource.Play();
    }

    void playSelectSound()
    {
        SoundSource.clip = SelectSoundClip;
        SoundSource.Play();
    }

    void playBackSound()
    {
        SoundSource.clip = GoBackSoundClip;
        SoundSource.Play();
    }

}
