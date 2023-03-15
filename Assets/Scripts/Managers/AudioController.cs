using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    private AudioSource playerSrc;
    private AudioSource musicSrc;
    
    private void Awake() {
        musicSrc = GetComponent<AudioSource>();
    }

    public void PlayMusic(AudioClip clip)
    {
        if (musicSrc == null) {
            Debug.Log("Music Source is " + musicSrc.ToString());
            return;
        }

        musicSrc.Stop();
        musicSrc.clip = clip;
        musicSrc.volume = Managers.GetCanvasManager().GetMusicSliderValue();
        musicSrc.Play();
    }

    public void PlayClip(AudioClip clip)
    {
        if (playerSrc == null)
            playerSrc = Managers.GetPlayerController().GetComponent<AudioSource>();

        playerSrc.PlayOneShot(clip, Managers.GetCanvasManager().GetSFXSliderValue());
    }
}
