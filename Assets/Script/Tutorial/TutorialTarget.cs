using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class TutorialTarget : MonoBehaviour
    {
        enum TouchStat
        {
            non,
            red,
            blue,
            red_blue
        }
        TouchStat touchStat;
        void Start()
        {

        }

        void Update()
        {

        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if(collider.name == "Red")
            {
                if(touchStat == TouchStat.non)
                {
                    touchStat = TouchStat.red;
                }
                if(touchStat == TouchStat.blue)
                {
                    touchStat = TouchStat.red_blue;
                }
            }
            if(collider.name == "Blue")
            {
                if(touchStat == TouchStat.non)
                {
                    touchStat = TouchStat.blue;
                }
                if(touchStat == TouchStat.red)
                {
                    touchStat = TouchStat.red_blue;
                }
            }
            if(touchStat == TouchStat.red_blue)
            {
                gameObject.SetActive(false);
            }
        }
    }
}