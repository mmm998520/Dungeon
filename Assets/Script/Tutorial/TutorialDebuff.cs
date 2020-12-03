using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class TutorialDebuff : MonoBehaviour
    {
        public GameObject diedImformation;

        void Update()
        {
            if (diedImformation.activeSelf)
            {
                GameManager.players.GetComponent<TutorialManager2>().reset();
            }
        }

        public void debuff()
        {
            if (GameManager.players.GetChild(0).GetComponent<PlayerManager>().p1)
            {
                GameManager.players.GetChild(0).GetComponent<PlayerManager>().ConfusionTimer = 0;
                GameManager.players.GetChild(1).GetComponent<PlayerManager>().StickTimer = 0;
            }
            else
            {
                GameManager.players.GetChild(0).GetComponent<PlayerManager>().StickTimer = 0;
                GameManager.players.GetChild(1).GetComponent<PlayerManager>().ConfusionTimer = 0;
            }
        }

        public void ShowDied()
        {
            diedImformation.SetActive(true);
        }
        public void UnShowDied()
        {
            diedImformation.SetActive(false);
            GetComponent<Animator>().SetBool("Died", false);
        }

        public void reStartBubble()
        {
            GameManager.players.GetComponent<TutorialManager2>().reStartBubble();
        }
    }
}