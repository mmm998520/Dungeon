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
        }
    }
}
