using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

namespace com.DungeonPad
{
    public class DataSaver : MonoBehaviour
    {
        public AbilityData[] abilityDatas;
        public static void tryLoad()
        {
            //PlayerPrefs.DeleteAll();
            if (PlayerPrefs.HasKey("passLayerOneTimes"))
            {
                GameManager.passLayerOneTimes = PlayerPrefs.GetInt("passLayerOneTimes");
            }
            else
            {
                PlayerPrefs.SetInt("passLayerOneTimes", GameManager.passLayerOneTimes);
                PlayerPrefs.Save();
            }
            if (PlayerPrefs.HasKey("passLayerThreeTimes"))
            {
                GameManager.passLayerThreeTimes = PlayerPrefs.GetInt("passLayerThreeTimes");
            }
            else
            {
                PlayerPrefs.SetInt("passLayerThreeTimes", GameManager.passLayerThreeTimes);
                PlayerPrefs.Save();
            }
            if (PlayerPrefs.HasKey("layerOneCntinuousDideTimes"))
            {
                GameManager.layerOneCntinuousDideTimes = PlayerPrefs.GetInt("layerOneCntinuousDideTimes");
            }
            else
            {
                PlayerPrefs.SetInt("layerOneCntinuousDideTimes", GameManager.layerOneCntinuousDideTimes);
                PlayerPrefs.Save();
            }
            if (PlayerPrefs.HasKey("layerThreeCntinuousDideTimes"))
            {
                GameManager.layerThreeCntinuousDideTimes = PlayerPrefs.GetInt("layerThreeCntinuousDideTimes");
            }
            else
            {
                PlayerPrefs.SetInt("layerThreeCntinuousDideTimes", GameManager.layerThreeCntinuousDideTimes);
                PlayerPrefs.Save();
            }

            if (PlayerPrefs.HasKey("Keyboard"))
            {
                string[] keyboardNum = PlayerPrefs.GetString("Keyboard").Split(',');
                InputManager.p1KeyboardUpNum = int.Parse(keyboardNum[0]);
                InputManager.p1KeyboardDownNum = int.Parse(keyboardNum[1]);
                InputManager.p1KeyboardLeftNum = int.Parse(keyboardNum[2]);
                InputManager.p1KeyboardRightNum = int.Parse(keyboardNum[3]);
                InputManager.p1KeyboardDashNum = int.Parse(keyboardNum[4]);
                InputManager.p1KeyboardBreakfreeKeyNum = int.Parse(keyboardNum[5]);
                InputManager.p1KeyboardSkillKeyNum = int.Parse(keyboardNum[6]);
                InputManager.p1KeyboardLookskillKeyNum = int.Parse(keyboardNum[7]);
                InputManager.p2KeyboardUpNum = int.Parse(keyboardNum[8]);
                InputManager.p2KeyboardDownNum = int.Parse(keyboardNum[9]);
                InputManager.p2KeyboardLeftNum = int.Parse(keyboardNum[10]);
                InputManager.p2KeyboardRightNum = int.Parse(keyboardNum[11]);
                InputManager.p2KeyboardDashNum = int.Parse(keyboardNum[12]);
                InputManager.p2KeyboardBreakfreeKeyNum = int.Parse(keyboardNum[13]);
                InputManager.p2KeyboardSkillKeyNum = int.Parse(keyboardNum[14]);
                InputManager.p2KeyboardLookskillKeyNum = int.Parse(keyboardNum[15]);
            }
            else
            {
                PlayerPrefs.SetString("Keyboard", "36,32,14,17,23,24,26,29,62,63,60,61,84,85,83,92,");
            }
        }

        void Start()
        {
            if(SceneManager.GetActiveScene().name == "SelectRole_Game 0")
            {
                PlayerManager.money = 0;
                PlayerManager.moneyB = 0;
                PlayerManager.criticalRate = 0;
                PlayerManager.reducesDamage = 0;
                PlayerManager.MaxLife = 4;
                PlayerManager.MaxHP = 60;
                PlayerManager.killHpRecover = 0;
                PlayerManager.DashSpeed = 11;
                PlayerManager.DashCD = 0.5f;
                PlayerManager.moveSpeed = 3;
                PlayerManager.homeButton = false;
                PlayerManager.magneticField = false;
                PlayerManager.circleAttack = false;
                PlayerManager.poison = false;
                PlayerManager.trackBullet = false;
            }
            else
            {
                for (int i = 0; i < abilityDatas.Length; i++)
                {
                    if (abilityDatas[i].dataNum < AbilityManager.AbilityCurrentLevel.Count)
                    {
                        abilityDatas[i].awake();
                    }
                }
            }
        }
        public static void Save()
        {
            PlayerPrefs.SetInt("passLayerOneTimes", GameManager.passLayerOneTimes);
            PlayerPrefs.SetInt("passLayerThreeTimes", GameManager.passLayerThreeTimes);
            PlayerPrefs.SetInt("layerOneCntinuousDideTimes", GameManager.layerOneCntinuousDideTimes);
            PlayerPrefs.SetInt("layerThreeCntinuousDideTimes", GameManager.layerThreeCntinuousDideTimes);
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