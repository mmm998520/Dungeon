using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.DungeonPad
{
    public class TutorialManager : MonoBehaviour
    {
        public Transform[] targetPoses;
        public int targetPosesNum;
        public string nextSceneName;
        float NextTargetTimer;

        void Start()
        {

        }

        void Update()
        {
            NextTargetTimer += Time.deltaTime;
            if (GameManager.monsters.childCount <= 0 &&SceneManager.GetActiveScene().name == "Tutorial3")
            {
                SceneManager.LoadScene(nextSceneName);
            }
        }

        public void NextTarget(Transform target)
        {
            if (NextTargetTimer > 0.3f)
            {
                NextTargetTimer = 0;
                if (++targetPosesNum < targetPoses.Length)
                {
                    target.position = targetPoses[targetPosesNum].position;
                }
                else
                {
                    Destroy(target.gameObject);
                }
                if (targetPosesNum == 4)
                {
                    SceneManager.LoadScene(nextSceneName);
                }
            }
        }
    }
}