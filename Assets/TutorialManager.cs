using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class TutorialManager : MonoBehaviour
    {
        public GameObject[] tutorialScenes;
        public Transform[] targetPoses;
        public int tutorialScenesNum, targetPosesNum;

        void Start()
        {

        }

        void Update()
        {

        }

        public void NextTarget(Transform target)
        {
            if(++targetPosesNum < targetPoses.Length)
            {
                target.position = targetPoses[targetPosesNum].position;
            }
            else
            {
                Destroy(target.gameObject);
            }
            if (targetPosesNum == 4 || targetPosesNum == 8)
            {
                tutorialScenes[tutorialScenesNum].SetActive(false);
                tutorialScenes[++tutorialScenesNum].SetActive(true);
            }
        }
    }
}