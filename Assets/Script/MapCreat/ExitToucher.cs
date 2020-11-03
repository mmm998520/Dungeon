using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class ExitToucher : MonoBehaviour
    {
        public static bool P1touch, P2touch;
        bool haveTouch = false;
        public string playerName;

        void Start()
        {

        }

        void Update()
        {
            haveTouch = false;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.35f);
            for(int i = 0; i < colliders.Length; i++)
            {
                if(colliders[i].name == playerName)
                {
                    if(playerName == "p1")
                    {
                        P1touch = true;
                    }
                    if (playerName == "p2")
                    {
                        P2touch = true;
                    }
                    haveTouch = true;
                }
            }
            if(!haveTouch)
            {
                if (playerName == "p1")
                {
                    P1touch = false;
                }
                if (playerName == "p2")
                {
                    P2touch = false;
                }
            }

        }
    }
}
