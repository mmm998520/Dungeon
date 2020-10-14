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
            if (transform.rotation.eulerAngles.z > 345&& transform.rotation.eulerAngles.z<355)
            {
                SceneManager.LoadScene("Game 2");
            }
        }
    }
}