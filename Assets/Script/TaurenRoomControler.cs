using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.DungeonPad
{
    public class TaurenRoomControler : MonoBehaviour
    {
        public Transform monsters;
        public GameObject spider, spiderB, spiderC, stab, Boss;

        private void Update()
        {
            print(GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime * 3589);
            if (CameraManager.center.x > 20.55f && Boss == null)
            {
                if(SceneManager.GetActiveScene().name =="Game 2")
                {
                    GameManager.layers = 2;
                    SwitchScenePanel.NextScene = "Game 1";
                    GameObject.Find("SwitchScenePanel").GetComponent<Animator>().SetTrigger("Loading");
                }
            }
            if (CameraManager.center.x > 30.64f && Boss == null)
            {
                if (SceneManager.GetActiveScene().name == "Game 4")
                {
                    ReGamer.ReGame();
                    SwitchScenePanel.NextScene = "SelectRole_Game 1";
                    GameObject.Find("SwitchScenePanel").GetComponent<Animator>().SetTrigger("Loading");
                }
            }
        }

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

        public void insStab(int times)
        {
            for (int i = 0; i < times; i++)
            {
                if (GameManager.triggers.childCount < 10)
                {
                    Instantiate(stab, new Vector3(Random.Range(1f, 20f), Random.Range(1f, 10f), 0), Quaternion.identity, GameManager.triggers);
                }
                else
                {
                    break;
                }
            }
        }

        void ins(GameObject obj, int times)
        {
            for(int i = 0; i < times; i++)
            {
                if (monsters.childCount < 0 + 4)
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

        void setStabAttack()
        {
            for(int i = 0; i < GameManager.triggers.childCount; i++)
            {
                if (GameManager.triggers.GetChild(i).GetComponent<Stab>())
                {
                    GameManager.triggers.GetChild(i).GetComponent<Animator>().SetTrigger("attack");
                }
            }
        }
    }
}