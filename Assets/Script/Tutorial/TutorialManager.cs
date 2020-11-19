using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.DungeonPad
{
    public class TutorialManager : MonoBehaviour
    {
        static int getTargetNum;
        static float NextTargetTime;
        public string nextSceneName;
        public GameObject[] others;

        void Start()
        {
            getTargetNum = 0;
            NextTargetTime = 0;
        }

        void Update()
        {
            if (GameManager.monsters.childCount <= 0 &&SceneManager.GetActiveScene().name == "Tutorial3")
            {
                SceneManager.LoadScene(nextSceneName);
            }
        }

        public void NextTarget(GameObject target)
        {
            if (Time.time - NextTargetTime > 0.1f)
            {
                Destroy(target);
                NextTargetTime = Time.deltaTime;
                getTargetNum++;
                if (getTargetNum == 4)
                {
                    SceneManager.LoadScene(nextSceneName);
                }
                if(getTargetNum == 2)
                {
                    ShowOther();
                }
            }
        }

        public void ShowOther()
        {
            for(int i = 0; i < others.Length; i++)
            {
                others[i].active = true;
            }
        }
    }
}