using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class DestoryMe : MonoBehaviour
    {
        public void destroyMe()
        {
            Destroy(gameObject);
        }
    }
}