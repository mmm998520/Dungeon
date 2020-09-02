using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.BoardGameDungeon
{
    /// <summary> 從地底下冒刺，刺的地點( child )為隨機或是在自己的位置冒刺，刺過一段時間後消失 </summary>
    public class Stab : TriggerManager
    {
        bool pop = false;
        float popTimer;
        float popTimerStoper;

        void Update()
        {
            if((popTimer += Time.deltaTime) > popTimerStoper)
            {
                foreach (Transform child in transform)
                {
                    child.GetComponent<Collider2D>().enabled = true;
                    pop = false;
                }
                popTimer = 0;
            }
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            foreach(Transform child in transform)
            {
                child.GetComponent<Collider2D>().enabled = true;
            }
            pop = true;
            popTimer = 0;
        }
    }
}