using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class ObjectCreater : MonoBehaviour
    {
        public GameObject created;
        public bool monster;
        public bool ramdomUperCreated;
        public int num;
        static float ramdomNum = 0.5f;

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
                if (ramdomUperCreated)
                {
                    if(Random.Range(0f,1f) < ramdomNum)
                    {
                        ramdomNum = 0.5f;
                    }
                    else
                    {
                        ramdomNum += 0.1f;
                    }
                }
                else
                {
                    created = Instantiate(created, transform.position, Quaternion.identity, GameManager.triggers);
                }
            }
        }

        void Update()
        {

        }
    }
}