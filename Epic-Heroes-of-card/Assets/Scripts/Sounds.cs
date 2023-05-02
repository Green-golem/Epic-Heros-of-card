using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour
{
    public AudioClip[] sound;

    private AudioSource audioSrc => GetComponent<AudioSource>();

    public void PlaySound(AudioClip clip, float volume = 1f)
    {
        audioSrc.PlayOneShot(clip,volume);
    }
}
