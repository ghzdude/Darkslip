using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    private AudioSource src;

    // Start is called before the first frame update
    void Start()
    {
        src = gameObject.GetComponent<AudioSource>();   
    }

    public void PlayClip(AudioClip clip, float volume)
    {
        src.PlayOneShot(clip, volume);
    }

    public void PlayClip(AudioClip clip, float volume, AudioSource src)
    {
        src.PlayOneShot(clip, volume);
    }
}
