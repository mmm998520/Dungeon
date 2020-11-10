using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class ObjectCreater : MonoBehaviour
    {
        public GameObject created;
        public bool monster;
        public int num;

        void Start()
        {
            if (monster)
            {
                for(int i = 0; i < num; i++)
                {
                    created = Instantiate(created, transform.position, Quaternion.identity, GameManager.monsters);
                }
            }
            else
            {
                created = Instantiate(created, transform.position, Quaternion.identity, GameManager.triggers);
            }
        }

        void Update()
        {

        }
    }
}