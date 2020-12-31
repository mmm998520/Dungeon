using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class AbilityManager : MonoBehaviour
    {
        //public static List<string> allAbility = new List<string>();
        public static Dictionary<string, byte> allAbility = new Dictionary<string, byte>();
        public static List<string> myAbilitys = new List<string>();
        void Awake()
        {
            setAllAbility();
        }

        void Update()
        {

        }

        void setAllAbility()
        {
            allAbility.Clear();
            allAbility.Add("光儲存上限增(40→60)", 1);
            allAbility.Add("光儲存上限增(60→80)", 1);
            allAbility.Add("殺怪回血10", 1);
            allAbility.Add("殺怪回血25", 1);
            allAbility.Add("原地復活光球+1", 255);
            allAbility.Add("原地復活上限+1(同時送1顆)", 2);
            allAbility.Add("衝刺距離增加 4變5.5", 1);
            allAbility.Add("衝刺冷卻時間降低0.1", 2);
            allAbility.Add("一般移動速度加快", 2);
            allAbility.Add("瞬移回夥伴身邊(冷卻10秒)", 1);
        }
    }
}