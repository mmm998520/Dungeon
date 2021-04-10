using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    public static float gameVolume;
    float thisVolume;
    public bool DoorOneUse, DoorTwoUse;

    void Start()
    {
        if(!audioSource)
        {
            audioSource = GetComponent<AudioSource>();
        }
        thisVolume = audioSource.volume;
        setVolume();
    }

    void Update()
    {
        if (gameObject.name == "DoorOneSFX")
        {
            audioSource.mute = !DoorOneUse;
        }
        else if (gameObject.name == "DoorTwoSFX")
        {
            audioSource.mute = !DoorTwoUse;
        }
    }

    public void setVolume()
    {
        try
        {
            audioSource.volume = thisVolume * gameVolume;
        }
        catch
        {

        }
    }

    public static void SetAllVolume()
    {
        SFXManager[] SFXManagers = FindObjectsOfType<SFXManager>();
        for(int i = 0; i < SFXManagers.Length; i++)
        {
            SFXManagers[i].setVolume();
        }
    }
}
