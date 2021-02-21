using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace com.DungeonPad
{
    public class ShowAbilityDetail : MonoBehaviour
    {
        Text text;
        float timer = 10;
        void Start()
        {
            text = GetComponent<Text>();
        }

        void Update()
        {
            timer += Time.deltaTime;
            if (timer > 10)
            {
                text.text = "";
            }
        }

        public void showDetail(string detail)
        {
            timer = 0;
            text.text = detail;
        }
    }
}