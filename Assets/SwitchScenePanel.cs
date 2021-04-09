using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.DungeonPad
{
    public class SwitchScenePanel : MonoBehaviour
    {
        public static string NextScene;
        void switchScenePanel()
        {
            SceneManager.LoadScene(NextScene);
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
                    GameObject[] objs = GameObject.FindGameObjectsWithTag("music");
                    for (int i = 0; i < objs.Length; i++)
                    {
                        Destroy(objs[i]);
                    }
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
                    break;
                default:
                    GameObject[] objs = GameObject.FindGameObjectsWithTag("TutorialMusic");
                    for (int i = 0; i < objs.Length; i++)
                    {
                        Destroy(objs[i]);
                    }
                    break;
            }
        }
    }
}
