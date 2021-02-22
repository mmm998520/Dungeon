using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.DungeonPad
{
    public class ExitRoller : MonoBehaviour
    {
        void Update()
        {
            transform.localPosition = Vector3.zero;
            if (transform.rotation.eulerAngles.z > 180 && transform.rotation.eulerAngles.z < 200)
            {
                if (GameManager.layers == 1)
                {
                    GameManager.passLayerOneTimes += 1;
                    SwitchScenePanel.NextScene = "Game 2";
                    GameObject.Find("SwitchScenePanel").GetComponent<Animator>().SetTrigger("Loading");
                }
                else
                {
                    SwitchScenePanel.NextScene = "Game 4";
                    GameObject.Find("SwitchScenePanel").GetComponent<Animator>().SetTrigger("Loading");
                }
            }
        }
    }
}