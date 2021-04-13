using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.DungeonPad
{
    public class SwitchScenePanel : MonoBehaviour
    {
        public static string NextScene;
        public float gradientVolume;
        float timer;
        static bool Gradient;

        void switchScenePanel()
        {
            SceneManager.LoadScene(NextScene);
            Gradient = true;
            GameObject[] musicObjs = GameObject.FindGameObjectsWithTag("music");
            GameObject[] TutorialMusicObjs = GameObject.FindGameObjectsWithTag("TutorialMusic");

            switch (NextScene)
            {
                case "Game 1":
                case "Game 2":
                case "Game 4":
                case "Game 0":
                case "Game 0_1":
                case "Game 0_2":
                case "Game 0_3":
                case "Game 0_4":
                case "Game 0_5":
                    for (int i = 0; i < musicObjs.Length; i++)
                    {
                        Destroy(musicObjs[i]);
                    }
                    break;
                default:
                    Gradient = false;
                    break;
            }
            switch (NextScene)
            {
                case "Game 0":
                case "Game 0_1":
                case "Game 0_2":
                case "Game 0_3":
                case "Game 0_4":
                case "Game 0_5":
                    Gradient = false;
                    break;
                default:
                    for (int i = 0; i < TutorialMusicObjs.Length; i++)
                    {
                        Destroy(TutorialMusicObjs[i]);
                    }
                    break;
            }
            timer = 0;
        }

        void setMusicSound()
        {
            MusicManager.gradientVolume = gradientVolume;
            MusicManager.SetAllVolume();
        }

        void Update()
        {
            timer += Time.deltaTime;
            if (timer < 2.1f && Gradient)
            {
                setMusicSound();
            }
        }

        void clearButtonClickedFirstInScene()
        {
            ButtonSelect.FirstInScene = "";
        }
    }
}
