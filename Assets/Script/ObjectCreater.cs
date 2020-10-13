using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class ObjectCreater : MonoBehaviour
    {
        public GameObject created;
        public bool monster;
        void Start()
        {
            if (monster)
            {
                created = Instantiate(created, transform.position, Quaternion.identity, GameManager.monsters);
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