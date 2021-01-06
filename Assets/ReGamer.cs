using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.DungeonPad
{
    public class ReGamer : MonoBehaviour
    {
        float timer;

        void Start()
        {

        }

        void Update()
        {
            if ((timer += Time.deltaTime) > 5)
            {
                PlayerManager.money /= 2;
                AbilityManager.myAbilitys.Clear();
                SceneManager.LoadScene("Game 1");
            }
        }
    }
}

