using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource playSong;
    public AudioSource introSong;



    public void ChangeMusic()
    {
        playSong.Play();
        introSong.Stop();

    }
}
