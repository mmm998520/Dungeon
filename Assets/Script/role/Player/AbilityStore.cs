using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class AbilityStore : MonoBehaviour
    {
        Dictionary<AbilityManager.Ability, int> CanBuyList = new Dictionary<AbilityManager.Ability, int>();

        void Start()
        {
            
        }

        void Update()
        {

        }



        void Refresh()
        {
            CanBuyList.Clear();
            //CanBuyList.Add(AbilityManager.Ability.)

        }
    }
}