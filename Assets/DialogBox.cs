using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace com.DungeonPad
{
    public class DialogBox : MonoBehaviour
    {
        [SerializeField] MonsterManager monsterManager;
        [SerializeField] PlayerManager[] playerManagers;
        [Space]
        [SerializeField] Sprite[] TaurenTalk_Normal;
        [SerializeField] Sprite TaurenTalk_PlayerWin;
        [SerializeField] Sprite TaurenTalk_PlayerDied;
        [Space]
        [SerializeField] Sprite[] DragonTalk_Normal;
        [SerializeField] Sprite DragonTalk_PlayerWin;
        [SerializeField] Sprite DragonTalk_PlayerDied;

        private void Start()
        {
            monsterManager.canMove = false;
            if (GameManager.CurrentSceneName == "Game 2")
            {
                if (PlayerPrefs.HasKey("TaurenStat"))
                {
                    if (PlayerPrefs.GetString("TaurenStat") == "TaurenTalk_PlayerWin" && Random.Range(0, 10) < 5)
                    {
                        GetComponent<Image>().sprite = TaurenTalk_PlayerWin;
                    }
                    else if (PlayerPrefs.GetString("TaurenStat") == "TaurenTalk_PlayerDied" && Random.Range(0, 10) < 9)
                    {
                        GetComponent<Image>().sprite = TaurenTalk_PlayerDied;
                    }
                    else
                    {
                        GetComponent<Image>().sprite = TaurenTalk_Normal[Mathf.Clamp(GameManager.AbilityNum - 2, 0, 2)];
                    }
                }
                else
                {
                    GetComponent<Image>().sprite = TaurenTalk_Normal[Mathf.Clamp(GameManager.AbilityNum - 2, 0, 2)];
                }
            }

            if (GameManager.CurrentSceneName == "Game 4")
            {
                if (PlayerPrefs.HasKey("DragonStat"))
                {
                    if (PlayerPrefs.GetString("DragonStat") == "DragonTalk_PlayerWin" && Random.Range(0, 10) < 5)
                    {
                        GetComponent<Image>().sprite = DragonTalk_PlayerWin;
                    }
                    else if (PlayerPrefs.GetString("DragonStat") == "DragonTalk_PlayerDied" && Random.Range(0, 10) < 5)
                    {
                        GetComponent<Image>().sprite = DragonTalk_PlayerDied;
                    }
                    else
                    {
                        GetComponent<Image>().sprite = DragonTalk_Normal[Mathf.Clamp(GameManager.AbilityNum - 4, 0, 4)];
                    }
                }
                else
                {
                    GetComponent<Image>().sprite = DragonTalk_Normal[Mathf.Clamp(GameManager.AbilityNum - 4, 0, 4)];
                }
            }
        }

        void Update()
        {
            if (InputManager.anyEnter() || InputManager.anyExit())
            {
                ButtonSelect.OnClicked();
                monsterManager.canMove = true;
                monsterManager.GetComponent<Animator>().enabled = true;
                playerManagers[0].enabled = true;
                playerManagers[1].enabled = true;
                gameObject.SetActive(false);
                GameObject.Find("Main Camera").GetComponent<CameraManager>().enabled = true;
            }
        }
    }
}