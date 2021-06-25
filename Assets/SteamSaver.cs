using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class SteamCloudPrefs
{
    // Game Data //
    public string passLayerOneTimes = "";
    public string passLayerThreeTimes = "";
    public string layerOneCntinuousDideTimes = "";
    public string layerThreeCntinuousDideTimes = "";
    public string Keyboard = "";
    public string layerFourCntinuousWinTimes = "";
    public string TaurenStat = "";
    public string DragonStat = "";
    public string MusicSound = "";
    public string FXSound = "";
    public string Lightness = "";
    public string PlayerNum = "";
}

public static class SaveLoadFile
{
    private const string FILENAME = "/SteamCloud_GLIM.sav";

    public static void Save(SteamCloudPrefs steamCloudPrefs)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + FILENAME, FileMode.Create);

        bf.Serialize(stream, steamCloudPrefs);
        stream.Close();
    }

    public static SteamCloudPrefs Load()
    {
        if (File.Exists(Application.persistentDataPath + FILENAME))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + FILENAME, FileMode.Open);

            SteamCloudPrefs data = bf.Deserialize(stream) as SteamCloudPrefs;

            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("File not found.");
            return null;
        }
    }
}

public class SteamSaver : MonoBehaviour
{
    public SteamCloudPrefs SteamStorage = new SteamCloudPrefs();
    static SteamSaver SingleOne;

    void Awake()
    {
        if(SingleOne == null)
        {
            Debug.Log(Application.persistentDataPath);
            Load();
            DontDestroyOnLoad(gameObject);
        }
        else if(SingleOne != this)
        {
            Destroy(this);
        }
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    void OnApplicationQuit()
    {
        save();
        for(int i = 0; i < Gamepad.all.Count; i++)
        {
            Gamepad.all[0].SetMotorSpeeds(0, 0);
        }
    }

    private void OnSceneUnloaded(Scene current)
    {
        for (int i = 0; i < Gamepad.all.Count; i++)
        {
            Gamepad.all[0].SetMotorSpeeds(0, 0);
        }
    }

    void save()
    {
        SteamStorage.passLayerOneTimes = PlayerPrefs.GetInt("passLayerOneTimes") + "";
        SteamStorage.passLayerThreeTimes = PlayerPrefs.GetInt("passLayerThreeTimes") + "";
        SteamStorage.layerOneCntinuousDideTimes = PlayerPrefs.GetInt("layerOneCntinuousDideTimes") + "";
        SteamStorage.layerThreeCntinuousDideTimes = PlayerPrefs.GetInt("layerThreeCntinuousDideTimes") + "";
        SteamStorage.Keyboard = PlayerPrefs.GetString("Keyboard");
        SteamStorage.layerFourCntinuousWinTimes = PlayerPrefs.GetInt("layerFourCntinuousWinTimes") + "";
        SteamStorage.TaurenStat = PlayerPrefs.GetString("TaurenStat");
        SteamStorage.DragonStat = PlayerPrefs.GetString("DragonStat");
        SteamStorage.MusicSound = PlayerPrefs.GetInt("MusicSound") + "";
        SteamStorage.FXSound = PlayerPrefs.GetInt("FXSound") + "";
        SteamStorage.Lightness = PlayerPrefs.GetInt("Lightness") + "";
        SteamStorage.PlayerNum = PlayerPrefs.GetInt("PlayerNum") + "";

        SaveLoadFile.Save(SteamStorage);
    }

    void Load()
    {
        if (SaveLoadFile.Load() != null)
        {
            SteamStorage = SaveLoadFile.Load();


            PlayerPrefs.SetInt("passLayerOneTimes", int.Parse(SteamStorage.passLayerOneTimes));
            PlayerPrefs.SetInt("passLayerThreeTimes", int.Parse(SteamStorage.passLayerThreeTimes));
            PlayerPrefs.SetInt("layerOneCntinuousDideTimes", int.Parse(SteamStorage.layerOneCntinuousDideTimes));
            PlayerPrefs.SetInt("layerThreeCntinuousDideTimes", int.Parse(SteamStorage.layerThreeCntinuousDideTimes));
            if (SteamStorage.Keyboard != "")
            {
                PlayerPrefs.SetString("Keyboard", SteamStorage.Keyboard);
            }
            PlayerPrefs.SetInt("layerFourCntinuousWinTimes", int.Parse(SteamStorage.layerFourCntinuousWinTimes));
            if (SteamStorage.TaurenStat != "")
            {
                PlayerPrefs.SetString("TaurenStat", SteamStorage.TaurenStat);
            }
            if (SteamStorage.DragonStat != "")
            {
                PlayerPrefs.SetString("DragonStat", SteamStorage.DragonStat);
            }
            PlayerPrefs.SetInt("MusicSound", int.Parse(SteamStorage.MusicSound));
            PlayerPrefs.SetInt("FXSound", int.Parse(SteamStorage.FXSound));
            PlayerPrefs.SetInt("Lightness", int.Parse(SteamStorage.Lightness));
            if (SteamStorage.PlayerNum != "0")
            {
                PlayerPrefs.SetInt("PlayerNum", int.Parse(SteamStorage.PlayerNum));
            }
        }
    }
}
