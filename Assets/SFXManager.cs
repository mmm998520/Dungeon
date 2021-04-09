using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    AudioSource audioSource;
    public static float volume;
    float thisVolume;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        thisVolume = audioSource.volume;
    }

    void Update()
    {
        //audioSource.volume = thisVolume * volume;
    }
}
