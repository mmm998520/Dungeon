using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.BoardGameDungeon
{
    /// <summary> 從地底下冒刺，刺的地點( child )為隨機或是在自己的位置冒刺，刺過一段時間後消失 </summary>
    public class Stab : TriggerManager
    {
        public PlayerManager user;
        public List<Transform> spines = new List<Transform>();

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<PlayerManager>())
            {
                user = collision.GetComponent<PlayerManager>();
                foreach (Transform spine in spines)
                {
                    spine.GetComponent<Collider2D>().enabled = true;
                    spine.GetComponent<Spine>().user = user;
                    spine.GetComponent<Spine>().popTimer = 0;
                    spine.GetChild(1).GetComponent<SpriteRenderer>().enabled = true;
                }
            }
        }
    }
}