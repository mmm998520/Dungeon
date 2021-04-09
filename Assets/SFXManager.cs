using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    AudioSource audioSource;
    public static float volume;
    float thisVolume;
    public bool DoorOneUse, DoorTwoUse;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        thisVolume = audioSource.volume;
    }

    void Update()
    {
        //audioSource.volume = thisVolume * volume;
        audioSource.volume = thisVolume;
        if (gameObject.name == "DoorOneSFX")
        {
            if (!DoorOneUse)
            {
                audioSource.volume = 0;
            }
        }
        else if (gameObject.name == "DoorTwoSFX")
        {
            if (!DoorTwoUse)
            {
                audioSource.volume = 0;
            }
        }
    }
}
