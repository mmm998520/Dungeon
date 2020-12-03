using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class SpiderBehavior : MonoBehaviour
    {
        public enum Behavior
        {
            ramdomMove = 0,
            attack = 1
        }
        public Behavior behavior = Behavior.ramdomMove;

        void Update()
        {
            if(behavior == Behavior.ramdomMove)
            {
                for(int i = 0; i < transform.childCount; i++)
                {
                    if (transform.GetChild(i).GetComponent<Spider>())
                    {
                        transform.GetChild(i).GetComponent<Spider>().spiderBehavior = Spider.SpiderBehavior.ramdomMove;
                    }
                    else if (transform.GetChild(i).GetComponent<SpiderB>())
                    {
                        transform.GetChild(i).GetComponent<SpiderB>().spiderBehavior = SpiderB.SpiderBehavior.ramdomMove;
                    }
                    else if (transform.GetChild(i).GetComponent<SpiderC>())
                    {
                        transform.GetChild(i).GetComponent<SpiderC>().spiderBehavior = SpiderC.SpiderBehavior.ramdomMove;
                    }
                }
            }
            else
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    if (transform.GetChild(i).GetComponent<Spider>())
                    {
                        transform.GetChild(i).GetComponent<Spider>().spiderBehavior = Spider.SpiderBehavior.attack;
                    }
                    else if (transform.GetChild(i).GetComponent<SpiderB>())
                    {
                        transform.GetChild(i).GetComponent<SpiderB>().spiderBehavior = SpiderB.SpiderBehavior.attack;
                    }
                    else if (transform.GetChild(i).GetComponent<SpiderC>())
                    {
                        transform.GetChild(i).GetComponent<SpiderC>().spiderBehavior = SpiderC.SpiderBehavior.attack;
                    }
                }
            }
        }
    }
}