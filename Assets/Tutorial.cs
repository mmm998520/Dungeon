using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace com.DungeonPad
{
    public class Tutorial : MonoBehaviour
    {
        int nowTextNum = 0;
        [SerializeField] GameObject[] TutorialText;
        [SerializeField] int goNextScenePosX;
        [SerializeField] string nextSceneName;
        [SerializeField] Transform[] insSpiderWebPoses;
        [SerializeField] GameObject spiderWeb;
        float insSpiderWebTimer = 3;
        public int ShowHPLine;
        public Image[] HPBarImages;

        void Update()
        {
            Keyboard keyboard = Keyboard.current;
            if (nowTextNum < TutorialText.Length)
            {
                for (int i = 0; i < GameManager.players.childCount; i++)
                {
                    Transform player = GameManager.players.GetChild(i);
                    player.GetComponent<PlayerManager>().enabled = false;
                    player.GetComponent<PlayerManager>().v = Vector3.zero;
                    player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                    player.GetChild(0).GetComponent<Animator>().enabled = false;
                }
                if (InputManager.anyEnter())
                {
                    if(Time.timeScale > 0.1f)
                    {
                        ButtonSelect.OnClicked();
                        TutorialText[nowTextNum].SetActive(false);
                        nowTextNum++;
                        if (nowTextNum < TutorialText.Length)
                        {
                            TutorialText[nowTextNum].SetActive(true);
                        }
                    }
                }
            }
            else if (nowTextNum == TutorialText.Length)
            {
                for(int i = 0; i < GameManager.players.childCount; i++)
                {
                    GameManager.players.GetChild(i).GetComponent<PlayerManager>().enabled = true;
                }
                nowTextNum++;
            }
            else
            {
                if (GameManager.CurrentSceneName == "Game 0" || GameManager.CurrentSceneName == "Game 0_1")
                {
                    for (int i = 0; i < GameManager.players.childCount; i++)
                    {
                        GameManager.players.GetChild(i).GetComponent<PlayerManager>().DashTimer = 0.3f;
                    }
                }
            }

            insSpiderWebTimer += Time.deltaTime;
            if (insSpiderWebTimer >= 2.7f)
            {
                insSpiderWebTimer = 0;
                for (int i = 0; i < insSpiderWebPoses.Length; i++)
                {
                    GameObject web = Instantiate(spiderWeb, insSpiderWebPoses[i].position, Quaternion.Euler(0, 180, 0));
                    web.GetComponent<MonsterAttack>().ATK = 25;
                    web.GetComponent<MonsterShooter>().timerStoper = 999;
                }
            }

            for (int i = 0; i < HPBarImages.Length; i++)
            {
                HPBarImages[i].color = new Color(HPBarImages[i].color.r, HPBarImages[i].color.g, HPBarImages[i].color.b, (ShowHPLine - CameraManager.center.x) / ShowHPLine);
            }
            if(CameraManager.center.x > goNextScenePosX + 0.7f)
            {
                SwitchScenePanel.NextScene = nextSceneName;
                GameObject.Find("SwitchScenePanel").GetComponent<Animator>().SetTrigger("Loading");
            }
        }
    }
}