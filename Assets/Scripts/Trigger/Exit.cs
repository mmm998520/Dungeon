using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.BoardGameDungeon
{
    public class Exit : MonoBehaviour
    {
        public AudioSource ExitAudio;
        void Start()
        {
            ExitAudio.Play();
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.GetComponent<PlayerManager>())
            {
                PlayerPrefs.SetFloat(collider.name + "Hurt", collider.GetComponent<PlayerManager>().Hurt);
                PlayerPrefs.SetInt(collider.name + "Level", collider.GetComponent<PlayerManager>().level);
                PlayerPrefs.SetFloat(collider.name + "Exp", collider.GetComponent<PlayerManager>().exp);
                if (GameManager.Players.childCount == 1)
                {
                    SceneManager.LoadScene("Game2");
                }
                Destroy(collider.gameObject);
            }
        }
    }
}