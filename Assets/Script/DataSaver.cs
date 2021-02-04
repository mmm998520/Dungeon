using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace com.DungeonPad
{
    public class DataSaver : MonoBehaviour
    {
        public AbilityData[] abilityDatas;
        public static void tryLoad()
        {
            //PlayerPrefs.DeleteAll();
            if (PlayerPrefs.HasKey("data1_AbilityCurrentLevel"))
            {
                AbilityManager.AbilityCurrentLevel = loadDictionary("data1_AbilityCurrentLevel");
                AbilityManager.AbilityCanUseLevel = loadDictionary("data1_AbilityCanUseLevel");
                AbilityManager.AbilityCanBuyLevel = loadDictionary("data1_AbilityCanBuyLevel");
            }
            else
            {
                saveDictionary("data1_AbilityCurrentLevel", AbilityManager.AbilityCurrentLevel);
                saveDictionary("data1_AbilityCanUseLevel", AbilityManager.AbilityCanUseLevel);
                saveDictionary("data1_AbilityCanBuyLevel", AbilityManager.AbilityCanBuyLevel);
            }
            if (PlayerPrefs.HasKey("moneyA"))
            {
                PlayerManager.money = PlayerPrefs.GetInt("moneyA");
            }
            else
            {
                PlayerPrefs.SetInt("moneyA", PlayerManager.money);
                PlayerPrefs.Save();
            }
            if (PlayerPrefs.HasKey("moneyB"))
            {
                PlayerManager.moneyB = PlayerPrefs.GetInt("moneyB");
            }
            else
            {
                PlayerPrefs.SetInt("moneyB", PlayerManager.moneyB);
                PlayerPrefs.Save();
            }
            if (PlayerPrefs.HasKey("Costed"))
            {
                AbilityManager.Costed = PlayerPrefs.GetInt("Costed");
            }
            else
            {
                PlayerPrefs.SetInt("Costed", AbilityManager.Costed);
                PlayerPrefs.Save();
            }
            if (PlayerPrefs.HasKey("TotalCost"))
            {
                AbilityManager.TotalCost = PlayerPrefs.GetInt("TotalCost");
            }
            else
            {
                PlayerPrefs.SetInt("TotalCost", AbilityManager.TotalCost);
                PlayerPrefs.Save();
            }
        }

        void Start()
        {
            for (int i = 0; i < abilityDatas.Length; i++)
            {
                if (abilityDatas[i].dataNum < AbilityManager.AbilityCurrentLevel.Count)
                {
                    abilityDatas[i].awake();
                }
            }
        }
        public static void Save()
        {
            saveDictionary("data1_AbilityCurrentLevel", AbilityManager.AbilityCurrentLevel);
            saveDictionary("data1_AbilityCanUseLevel", AbilityManager.AbilityCanUseLevel);
            saveDictionary("data1_AbilityCanBuyLevel", AbilityManager.AbilityCanBuyLevel);
            PlayerPrefs.SetInt("moneyA", PlayerManager.money);
            PlayerPrefs.SetInt("moneyB", PlayerManager.moneyB);
            PlayerPrefs.SetInt("Costed", AbilityManager.Costed);
            PlayerPrefs.SetInt("TotalCost", AbilityManager.TotalCost);
            PlayerPrefs.Save();
        }

        #region//讓Dictionary變成Json檔，幫助存、讀檔
        static void saveDictionary(string dataNum, Dictionary<string, int> dictionary)
        {
            string datastr = Serialization(dictionary);
            SaveData(dataNum, datastr);
        }
        static Dictionary<string, int> loadDictionary(string dataNum)
        {
            string loadstr = LoadData(dataNum);
            if (loadstr != "")
            {
                return Deserialization(loadstr);
            }
            else
            {
                return null;
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