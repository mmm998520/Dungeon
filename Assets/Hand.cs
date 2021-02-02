using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class Hand : MonoBehaviour
    {
        Transform stickedPlayer = null;
        public Catcher catcher;

        void Start()
        {

        }

        void Update()
        {
            if(catcher.catcherStat == Catcher.CatcherStat.attcak)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 0.1f, 1 << 8 | 1 << 12);
                if (hit)
                {
                    if (hit.collider.GetComponent<PlayerManager>())
                    {
                        stickedPlayer = hit.collider.transform;
                    }
                    catcher.catcherStat = Catcher.CatcherStat.back;
                }
            }
            else if(catcher.catcherStat == Catcher.CatcherStat.back)
            {
                if (transform.parent.localScale.x <= 1)
                {
                    stickedPlayer = null;
                    catcher.catcherStat = Catcher.CatcherStat.idle;
                }
            }
            if (stickedPlayer != null)
            {
                stickedPlayer.position = transform.position;
            }
        }
    }
}