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
                    SceneManager.LoadScene("Game 2");
                }
                else
                {
                    SceneManager.LoadScene("Game 4");
                }
            }
        }
    }
}