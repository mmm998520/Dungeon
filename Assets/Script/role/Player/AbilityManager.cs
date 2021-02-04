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
            DataSaver.awake();
        }

        void setDictionary()
        {
            #region//當前等級
            AbilityCurrentLevel.Add("爆擊率", 0);
            AbilityCurrentLevel.Add("傷害減輕", 0);
            AbilityCurrentLevel.Add("復活上限", 0);
            AbilityCurrentLevel.Add("負載上限", 0);
            AbilityCurrentLevel.Add("血量上限增加", 0);
            AbilityCurrentLevel.Add("吸收", 0);
            AbilityCurrentLevel.Add("強力衝刺", 0);
            AbilityCurrentLevel.Add("連續衝刺", 0);
            AbilityCurrentLevel.Add("疾行", 0);
            AbilityCurrentLevel.Add("召回", 0);
            #endregion

            #region//已購買的等級
            AbilityCanUseLevel.Add("爆擊率", 0);
            AbilityCanUseLevel.Add("傷害減輕", 0);
            AbilityCanUseLevel.Add("復活上限", 0);
            AbilityCanUseLevel.Add("負載上限", 0);
            AbilityCanUseLevel.Add("血量上限增加", 0);
            AbilityCanUseLevel.Add("吸收", 0);
            AbilityCanUseLevel.Add("強力衝刺", 0);
            AbilityCanUseLevel.Add("連續衝刺", 0);
            AbilityCanUseLevel.Add("疾行", 0);
            AbilityCanUseLevel.Add("召回", 0);
            #endregion

            #region//可買的等級上限
            AbilityCanBuyLevel.Add("爆擊率", 19);
            AbilityCanBuyLevel.Add("傷害減輕", 19);
            AbilityCanBuyLevel.Add("復活上限", 2);
            AbilityCanBuyLevel.Add("負載上限", 4);
            AbilityCanBuyLevel.Add("血量上限增加", 2);
            AbilityCanBuyLevel.Add("吸收", 2);
            AbilityCanBuyLevel.Add("強力衝刺", 1);
            AbilityCanBuyLevel.Add("連續衝刺", 2);
            AbilityCanBuyLevel.Add("疾行", 2);
            AbilityCanBuyLevel.Add("召回", 2);
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