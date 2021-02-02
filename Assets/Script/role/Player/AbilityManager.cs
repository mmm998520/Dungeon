using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace com.DungeonPad
{
    public class AbilityManager : MonoBehaviour
    {
        public Ability.ability[] abilitys;

        void Awake()
        {
            Ability ability = new Ability();
            ability.AbilityCurrentLevel = new Dictionary<string, int>() { { "123", 1 } };

            if (PlayerPrefs.HasKey("data"))
            {
                loadDictionary("data", ability.AbilityCurrentLevel);
            }
            else
            {
                saveDictionary("data", ability.AbilityCurrentLevel);
            }
        }

        #region//存、讀檔
        void saveDictionary(string dataNum, Dictionary<string, int> dictionary)
        {
            string datastr = Serialization(dictionary);
            Debug.LogError(datastr);
            SaveData(dataNum, datastr);
        }
        void loadDictionary(string dataNum, Dictionary<string, int> dictionary)
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
        public string Serialization(Dictionary<string, int> dictionary)
        {
            return JsonUtility.ToJson(new Serialization<string, int>(dictionary));
        }

        /// <summary>
        /// 文字數值化 json格式
        /// </summary>
        /// <param name="datastr"></param>
        /// <returns></returns>
        public Dictionary<string, int> Deserialization(string datastr)
        {
            return JsonUtility.FromJson<Serialization<string, int>>(datastr).ToDictionary();
        }

        public bool SaveData(string key, string savestr)
        {
            PlayerPrefs.SetString(key, savestr);
            PlayerPrefs.Save();
            return PlayerPrefs.HasKey(key);
        }

        public string LoadData(string key)
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

    [System.Serializable]
    public class Ability
    {
        public Dictionary<string, int> AbilityCurrentLevel = new Dictionary<string, int>();
        public Dictionary<string, int> AbilityCanUseLevel = new Dictionary<string, int>();
        public Dictionary<string, int> AbilityCanBuyLevel = new Dictionary<string, int>();
        [System.Serializable]
        public struct ability
        {
            public string name;
            public string[] depiction;
            public int[] moneyA, moneyB, cost;
        }
    }
}