using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    public static float gameVolume, gradientVolume = 1;
    float thisVolume;

    void Start()
    {
        if (!audioSource)
        {
            audioSource = GetComponent<AudioSource>();
        }
        thisVolume = audioSource.volume;
        setVolume();
    }

    public void setVolume()
    {
        try
        {
            audioSource.volume = thisVolume * gameVolume * gradientVolume;
        }
        catch
        {

        }
    }

    public static void SetAllVolume()
    {
        MusicManager[] MusicManagers = FindObjectsOfType<MusicManager>();
        for (int i = 0; i < MusicManagers.Length; i++)
        {
            MusicManagers[i].setVolume();
        }
    }
}
