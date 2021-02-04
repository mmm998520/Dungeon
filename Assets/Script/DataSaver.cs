﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace com.DungeonPad
{
    public class DataSaver : MonoBehaviour
    {

        void Awake()
        {
            if (PlayerPrefs.HasKey("data1_AbilityCurrentLevel"))
            {
                Debug.LogError("HasData1_AbilityCurrentLevel");
                loadDictionary("data1_AbilityCurrentLevel", AbilityManager.AbilityCurrentLevel);
                loadDictionary("data1_AbilityCanUseLevel", AbilityManager.AbilityCanUseLevel);
                loadDictionary("data1_AbilityCanBuyLevel", AbilityManager.AbilityCanBuyLevel);
                Debug.LogError(0);
            }
            else
            {
                Debug.LogError(1);
                saveDictionary("data1_AbilityCurrentLevel", AbilityManager.AbilityCurrentLevel);
                saveDictionary("data1_AbilityCanUseLevel", AbilityManager.AbilityCanUseLevel);
                saveDictionary("data1_AbilityCanBuyLevel", AbilityManager.AbilityCanBuyLevel);
            }
            if (PlayerPrefs.HasKey("moneyA"))
            {
                Debug.LogError("HasMoneyA");
                PlayerManager.money = PlayerPrefs.GetInt("moneyA");
            }
            else
            {
                PlayerPrefs.SetInt("moneyA", PlayerManager.money);
                PlayerPrefs.Save();
            }
            if (PlayerPrefs.HasKey("moneyB"))
            {
                Debug.LogError("HasMoneyB");
                PlayerManager.moneyB = PlayerPrefs.GetInt("moneyB");
            }
            else
            {
                PlayerPrefs.SetInt("moneyB", PlayerManager.moneyB);
                PlayerPrefs.Save();
            }
        }
        public static void Save()
        {
            saveDictionary("data1_AbilityCurrentLevel", AbilityManager.AbilityCurrentLevel);
            saveDictionary("data1_AbilityCanUseLevel", AbilityManager.AbilityCanUseLevel);
            saveDictionary("data1_AbilityCanBuyLevel", AbilityManager.AbilityCanBuyLevel);
            PlayerPrefs.SetInt("moneyA", PlayerManager.money);
            PlayerPrefs.SetInt("moneyB", PlayerManager.moneyB);
            PlayerPrefs.Save();
        }

        #region//讓Dictionary變成Json檔，幫助存、讀檔
        static void saveDictionary(string dataNum, Dictionary<string, int> dictionary)
        {
            string datastr = Serialization(dictionary);
            Debug.LogError(datastr);
            SaveData(dataNum, datastr);
        }
        static void loadDictionary(string dataNum, Dictionary<string, int> dictionary)
        {
            string loadstr = LoadData(dataNum);
            if (loadstr != "")
            {
                dictionary = Deserialization(loadstr);
            }
        }
        /// <summary>
        /// 數值文字化 json格式
        /// </summary>
        /// <param name="abilityLevels"></param>
        /// <returns></returns>
        public static string Serialization(Dictionary<string, int> dictionary)
        {
            return JsonUtility.ToJson(new Serialization<string, int>(dictionary));
        }

        /// <summary>
        /// 文字數值化 json格式
        /// </summary>
        /// <param name="datastr"></param>
        /// <returns></returns>
        public static Dictionary<string, int> Deserialization(string datastr)
        {
            return JsonUtility.FromJson<Serialization<string, int>>(datastr).ToDictionary();
        }

        public static bool SaveData(string key, string savestr)
        {
            PlayerPrefs.SetString(key, savestr);
            PlayerPrefs.Save();
            return PlayerPrefs.HasKey(key);
        }

        public static string LoadData(string key)
        {
            return PlayerPrefs.HasKey(key) ? PlayerPrefs.GetString(key) : "";
        }
        #endregion
    }

    #region//unity擴充(讓Dictionary可以json化)
    // List<T>
    [Serializable]
    public class Serialization<T>
    {
        [SerializeField]
        List<T> target;
        public List<T> ToList() { return target; }

        public Serialization(List<T> target)
        {
            this.target = target;
        }
    }

    // Dictionary<TKey, TValue>
    [Serializable]
    public class Serialization<TKey, TValue> : ISerializationCallbackReceiver
    {
        [SerializeField]
        List<TKey> keys;
        [SerializeField]
        List<TValue> values;

        Dictionary<TKey, TValue> target;
        public Dictionary<TKey, TValue> ToDictionary() { return target; }

        public Serialization(Dictionary<TKey, TValue> target)
        {
            this.target = target;
        }

        public void OnBeforeSerialize()
        {
            keys = new List<TKey>(target.Keys);
            values = new List<TValue>(target.Values);
        }

        public void OnAfterDeserialize()
        {
            var count = Math.Min(keys.Count, values.Count);
            target = new Dictionary<TKey, TValue>(count);
            for (var i = 0; i < count; ++i)
            {
                target.Add(keys[i], values[i]);
            }
        }
    }
    #endregion
}