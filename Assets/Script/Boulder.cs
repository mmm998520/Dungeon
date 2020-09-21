using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class Boulder : MonoBehaviour
    {
        public Vector3 dir = Vector3.zero;
        void Update()
        {
            transform.Translate(dir * Time.deltaTime);
        }
    }
}