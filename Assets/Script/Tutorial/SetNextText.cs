﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace com.DungeonPad
{
    public class SetNextText : MonoBehaviour
    {
        void Start()
        {

        }

        void Update()
        {

        }

        public void died()
        {
            SwitchScenePanel.NextScene = SceneManager.GetActiveScene().name;
            GameObject.Find("SwitchScenePanel").GetComponent<Animator>().SetTrigger("Loading");
            PlayerManager.HP = PlayerManager.MaxHP;
        }

        public void setString(string words)
        {
            GetComponent<Text>().text = words;
        }
    }
}