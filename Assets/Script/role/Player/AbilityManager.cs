using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace com.DungeonPad
{
    public class AbilityManager : MonoBehaviour
    {
        public static int Costed = 0, TotalCost = 2;
        public Ability.ability[] abilitys;
        public static Ability.ability[] Abilitys;
        public static List<int> myAbilitys = new List<int>();
        public static Dictionary<string, int> AbilityCurrentLevel = new Dictionary<string, int>();
        public static Dictionary<string, int> AbilityCanUseLevel = new Dictionary<string, int>();
        public static Dictionary<string, int> AbilityCanBuyLevel = new Dictionary<string, int>();

        void Awake()
        {
            Abilitys = abilitys;
            abilitys = null;

            setDictionary();
            for (int i = 0; i < Abilitys.Length; i++)
            {
                if (AbilityCurrentLevel.ContainsKey(Abilitys[i].name))
                {
                    myAbilitys.Add(i);
                }
            }
            DataSaver.tryLoad();
        }

        void setDictionary()
        {
            #region//當前等級
            AbilityCurrentLevel.Add("暴擊", 0);
            AbilityCurrentLevel.Add("不屈", 0);
            AbilityCurrentLevel.Add("光炸", 0);
            AbilityCurrentLevel.Add("強光", 0);
            AbilityCurrentLevel.Add("守護", 0);
            AbilityCurrentLevel.Add("突進", 0);
            AbilityCurrentLevel.Add("低耗", 0);
            AbilityCurrentLevel.Add("疾行", 0);
            AbilityCurrentLevel.Add("傳送", 0);
            AbilityCurrentLevel.Add("磁場", 0);
            AbilityCurrentLevel.Add("濺射", 0);
            AbilityCurrentLevel.Add("根性", 0);
            AbilityCurrentLevel.Add("光鏢", 0);
            AbilityCurrentLevel.Add("免疫", 0);
            #endregion

            #region//已購買的等級
            AbilityCanUseLevel.Add("暴擊", 2);
            AbilityCanUseLevel.Add("不屈", 1);
            AbilityCanUseLevel.Add("光炸", 2);
            AbilityCanUseLevel.Add("強光", 1);
            AbilityCanUseLevel.Add("守護", 2);
            AbilityCanUseLevel.Add("突進", 1);
            AbilityCanUseLevel.Add("低耗", 2);
            AbilityCanUseLevel.Add("疾行", 2);
            AbilityCanUseLevel.Add("傳送", 2);
            AbilityCanUseLevel.Add("磁場", 1);
            AbilityCanUseLevel.Add("濺射", 1);
            AbilityCanUseLevel.Add("根性", 1);
            AbilityCanUseLevel.Add("光鏢", 1);
            AbilityCanUseLevel.Add("免疫", 1);
            #endregion

            #region//可買的等級上限
            AbilityCanBuyLevel.Add("暴擊", 2);
            AbilityCanBuyLevel.Add("不屈", 1);
            AbilityCanBuyLevel.Add("光炸", 2);
            AbilityCanBuyLevel.Add("強光", 1);
            AbilityCanBuyLevel.Add("守護", 2);
            AbilityCanBuyLevel.Add("突進", 1);
            AbilityCanBuyLevel.Add("低耗", 2);
            AbilityCanBuyLevel.Add("疾行", 2);
            AbilityCanBuyLevel.Add("傳送", 2);
            AbilityCanBuyLevel.Add("磁場", 1);
            AbilityCanBuyLevel.Add("濺射", 1);
            AbilityCanBuyLevel.Add("根性", 1);
            AbilityCanBuyLevel.Add("光鏢", 1);
            AbilityCanBuyLevel.Add("免疫", 1);
            #endregion
        }
    }

    [System.Serializable]
    public class Ability
    {
        
        [System.Serializable]
        public struct ability
        {
            public string name;
            public string[] detail;
            public string[] UnLockDetail;
            public string[] Num;//小面板上顯示的數值
            public int[] moneyA, moneyB, cost;
            public bool forever;
        }
    }
}