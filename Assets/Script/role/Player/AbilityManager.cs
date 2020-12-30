using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class AbilityManager : MonoBehaviour
    {
        static System.Random _R = new System.Random();
        static T RandomEnumValue<T>()
        {
            var v = System.Enum.GetValues(typeof(T));
            return (T)v.GetValue(_R.Next(v.Length));
        }

        public enum Ability
        {
            光儲存上限增40_60_80 = 0,
            殺怪回血0_10_25 = 1,
            原地復活光球add1 = 2,
            原地復活上限add1原地復活光球add1 = 3,
            衝刺距離增加11變15 = 4,
            衝刺冷卻時間降低1 = 5,
            一般移動速度加快1 = 6,
            瞬移回夥伴身邊cd10秒 = 7
        }

        static List<Ability> myAbilitys = new List<Ability>();

        public static void addAbility(Ability ability)
        {
            myAbilitys.Add(ability);
        }

        void Start()
        {
            for(int i = 0; i < 100; i++)
            {
                Debug.LogError(RandomEnumValue<Ability>().ToString());
            }
        }

        void Update()
        {

        }
    }
}