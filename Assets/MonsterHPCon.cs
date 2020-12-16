using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class MonsterHPCon : MonoBehaviour
    {
        public MonsterManager monsterManager;
        void Start()
        {

        }

        void Update()
        {
            transform.localScale = new Vector3(monsterManager.HP / monsterManager.MaxHP, 1, 1);
        }
    }
}