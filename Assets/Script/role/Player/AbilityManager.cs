using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class AbilityManager : MonoBehaviour
    {
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
            allAbility.Add("血量上限增加 LV0→LV1", 1);
            allAbility.Add("血量上限增加 LV1→LV2", 1);
            allAbility.Add("殺怪回血 LV0→LV1", 1);
            allAbility.Add("殺怪回血 LV1→LV2", 1);
            allAbility.Add("復活光球+1", 255);
            allAbility.Add("復活上限+1(送1顆復活光球)", 2);
            allAbility.Add("衝刺距離增加", 1);
            allAbility.Add("降低衝刺冷卻 LV0→LV1", 1);
            allAbility.Add("降低衝刺冷卻 LV1→LV2", 1);
            allAbility.Add("加快移動 LV0→LV1", 1);
            allAbility.Add("加快移動 LV1→LV2", 1);
            allAbility.Add("按X傳送到隊友身邊(冷卻10秒)", 1);
        }
    }
}