using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

namespace com.DungeonPad
{
    public class GameWarningManager : MonoBehaviour
    {
        [SerializeField] string nextScene;
        [SerializeField] Sprite nextSprite;
        [SerializeField] float timer, timerStoper;

        private void Start()
        {
            //Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void Update()
        {
            Keyboard keyboard = Keyboard.current;
            Gamepad gamepad = Gamepad.current;
            timer += Time.deltaTime;
            if (timer > timerStoper || (keyboard.anyKey.wasPressedThisFrame || (gamepad != null && gamepad.aButton.wasPressedThisFrame)))
            {
                
                SwitchScenePanel.NextScene = nextScene;
                GameObject.Find("SwitchScenePanel").GetComponent<Image>().sprite = nextSprite;
                if (nextSprite != null)
                {
                    GameObject.Find("SwitchScenePanel").GetComponent<Image>().color = Color.white;
                }
                GameObject.Find("SwitchScenePanel").GetComponent<Animator>().SetTrigger("Loading");
            }
        }
    }
}