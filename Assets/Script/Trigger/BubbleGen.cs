using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class BubbleGen : MonoBehaviour
    {
        public float timer, timerStoper;
        public GameObject Bubble;

        void Update()
        {
            if ((timer += Time.deltaTime) >= timerStoper)
            {
                timer -= timerStoper;
                Destroy(Instantiate(Bubble, transform.position, Quaternion.identity, transform), 6);
            }
        }
    }
}