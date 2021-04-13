using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    public static float gameVolume;
    float thisVolume;
    public bool DoorOneUse, DoorTwoUse;
    public List<GameObject> gameObjects = new List<GameObject>();

    static int clickedInOneFrame;

    void Start()
    {
        if (gameObject.name.Contains("ButtonClickSFX"))
        {
            clickedInOneFrame++;
            Debug.LogWarning(clickedInOneFrame);
            if (clickedInOneFrame >= 2)
            {
                Destroy(gameObject);
            }
            if (!audioSource)
            {
                audioSource = GetComponent<AudioSource>();
            }
            thisVolume = audioSource.volume;
            setVolume();
        }

        if (!audioSource)
        {
            audioSource = GetComponent<AudioSource>();
        }
        thisVolume = audioSource.volume;
        setVolume();
    }

    void Update()
    {
        clickedInOneFrame = 0;
        if (gameObject.name == "DoorOneSFX")
        {
            audioSource.mute = !DoorOneUse;
        }
        else if (gameObject.name == "DoorTwoSFX")
        {
            audioSource.mute = !DoorTwoUse;
        }
        else if(gameObjects.Count > 0)
        {
            setVolume();
        }
    }

    public void setVolume()
    {
        try
        {
            if (gameObjects.Count <= 0)
            {
                audioSource.volume = thisVolume * gameVolume;
            }
            else
            {
                int count = 0;
                for (int i = 0; i < gameObjects.Count; i++)
                {
                    if (gameObjects[i] != null)
                    {
                        count++;
                    }
                }
                audioSource.volume = thisVolume * gameVolume * count;
            }
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
