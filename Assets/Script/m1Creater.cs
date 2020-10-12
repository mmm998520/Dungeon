using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class m1Creater : MonoBehaviour
    {
        public GameObject m1;
        void Start()
        {
            m1 = Instantiate(m1, transform.position, Quaternion.identity, GameManager.monsters);
        }

        void Update()
        {

        }
    }
}