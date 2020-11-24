using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class TutorialDebuff : MonoBehaviour
    {
        // Start is called before the first frame update
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
    }
}