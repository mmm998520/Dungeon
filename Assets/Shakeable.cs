using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace com.DungeonPad
{
    public class Shakeable : MonoBehaviour
    {
        public static bool shake = false;
        float timer, timerStoper, rate;

        public void Update()
        {
            if (PlayerManager.HP < PlayerManager.MaxHP * 0.4f)
            {
                timer += Time.deltaTime;
                rate = PlayerManager.MaxHP / PlayerManager.HP / 20;
                timerStoper = PlayerManager.HP / PlayerManager.MaxHP / 2;
                if (timer > timerStoper)
                {
                    timer = 0;
                    Shake();
                }
            }
            else
            {
                transform.localPosition = Vector3.zero;
            }
        }

        void Shake()
        {
            rate = Mathf.Clamp(rate / 5, 0, 0.2f);
            transform.localPosition = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1)) * rate;
        }
    }
}