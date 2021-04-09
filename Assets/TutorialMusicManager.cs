using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class TutorialMusicManager : MonoBehaviour
    {
        private void Awake()
        {
            GameObject[] objs = GameObject.FindGameObjectsWithTag("TutorialMusic");
            Debug.LogError("objs.Length : " + objs.Length);
            if (objs.Length > 1)
            {
                Destroy(gameObject);
            }
            else
            {
                DontDestroyOnLoad(gameObject);
            }
        }

        void Update()
        {
            transform.position = CameraManager.center;
        }
    }
}