using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

        public int ShowHPLine;
        public Image[] HPBarImages;

        void Update()
        {
            if (nowTextNum < TutorialText.Length)
            {
                for (int i = 0; i < GameManager.players.childCount; i++)
                {
                    GameManager.players.GetChild(i).GetComponent<PlayerManager>().enabled = false;
                }
                if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.JoystickButton0))
                {
                    TutorialText[nowTextNum].SetActive(false);
                    nowTextNum++;
                    if (nowTextNum < TutorialText.Length)
                    {
                        TutorialText[nowTextNum].SetActive(true);
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


            for (int i = 0; i < HPBarImages.Length; i++)
            {
                HPBarImages[i].color = new Color(HPBarImages[i].color.r, HPBarImages[i].color.g, HPBarImages[i].color.b, (ShowHPLine - CameraManager.center.x) / ShowHPLine);
            }
            if(CameraManager.center.x > goNextScenePosX + 0.7f)
            {
                ReGamer.ReAbility();
                SwitchScenePanel.NextScene = nextSceneName;
                GameObject.Find("SwitchScenePanel").GetComponent<Animator>().SetTrigger("Loading");
            }
        }
    }
}