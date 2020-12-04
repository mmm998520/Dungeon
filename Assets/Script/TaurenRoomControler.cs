using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class TaurenRoomControler : MonoBehaviour
    {
        public Transform monsters;
        public GameObject spider, spiderB, spiderC, Boss;
        public void insSpider(int times)
        {
            ins(spider, times);
        }

        public void insSpiderB(int times)
        {
            ins(spiderB, times);
        }

        public void insSpiderC(int times)
        {
            ins(spiderC, times);
        }

        void ins(GameObject obj, int times)
        {
            for(int i = 0; i < times; i++)
            {
                if (monsters.childCount < 5 + 4)
                {
                    Instantiate(obj, new Vector3(Random.Range(1f, 20f), Random.Range(1f, 10f), 0), Quaternion.identity, monsters).SetActive(true);
                }
                else
                {
                    break;
                }
            }
        }

        public void setBossActive(int active)
        {
            Boss.SetActive(active > 0);
        }
    }
}