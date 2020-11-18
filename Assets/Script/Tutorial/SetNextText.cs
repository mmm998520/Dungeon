using System.Collections;
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
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            PlayerManager.HP = PlayerManager.MaxHP;
        }

        public void setString(string words)
        {
            GetComponent<Text>().text = words;
        }
    }
}