using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace com.DungeonPad
{
    public class TutorialAfterDiedWord : MonoBehaviour
    {
        public Text text;
        float timer;

        // Update is called once per frame
        void Update()
        {
            timer += Time.deltaTime;
            if (timer < 1)
            {
                text.color = new Color(text.color.r, text.color.g, text.color.b, timer);
            }
            if (timer < 3)
            {
                text.color = new Color(text.color.r, text.color.g, text.color.b, timer - 1);
            }
            else
            {
                SceneManager.LoadScene("game 0");
                PlayerManager.HP = PlayerManager.MaxHP;
                PlayerManager.Life = 1;
            }
        }
    }

}
