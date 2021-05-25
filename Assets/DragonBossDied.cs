using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.DungeonPad
{
    public class DragonBossDied : MonoBehaviour
    {
        float timer;
        SpriteRenderer[] spriteRenderer = new SpriteRenderer[2];
        Vector3 startPos;
        void Start()
        {
            spriteRenderer[0] = GetComponent<SpriteRenderer>();
            spriteRenderer[1] = transform.GetChild(1).GetComponent<SpriteRenderer>();
            startPos = transform.position;
        }

        void Update()
        {
            timer += Time.deltaTime;
            if (timer >= 4)
            {
                if (GameManager.DEMO && SceneManager.GetActiveScene().name == "Game 2")
                {
                    SwitchScenePanel.NextScene = "DEMO Ending";
                    GameObject.Find("SwitchScenePanel").GetComponent<Animator>().SetTrigger("Loading");
                }
                else if (SceneManager.GetActiveScene().name == "Game 4")
                {
                    SwitchScenePanel.NextScene = "Ending";
                    GameObject.Find("SwitchScenePanel").GetComponent<Animator>().SetTrigger("Loading");
                }
            }
            transform.position = startPos;
            spriteRenderer[0].color = new Color(1, 1, 1, Mathf.Clamp01((2 - timer) / 2));
            spriteRenderer[1].color = new Color(spriteRenderer[1].color.r, spriteRenderer[1].color.g, spriteRenderer[1].color.b, Mathf.Clamp01((2 - timer) / 2));
        }
    }
}