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
        float popTimerStoper = 1;
        public PlayerManager user;

        void Update()
        {
            if (pop)
            {
                if ((popTimer += Time.deltaTime) > popTimerStoper)
                {
                    transform.GetChild(1).GetComponent<Collider2D>().enabled = false;
                    transform.GetChild(1).GetChild(1).GetComponent<SpriteRenderer>().enabled = false;
                    pop = false;
                    popTimer = 0;
                }
            }
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<PlayerManager>())
            {
                user = collision.GetComponent<PlayerManager>();
                transform.GetChild(1).GetComponent<Collider2D>().enabled = true;
                transform.GetChild(1).GetComponent<Spine>().user = user;
                transform.GetChild(1).GetChild(1).GetComponent<SpriteRenderer>().enabled = true;
                pop = true;
                popTimer = 0;
            }
        }
    }
}