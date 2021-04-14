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
            //SystemInfo.size.Width
            //SystemInfo.PrimaryMonitorSize.Height
            Cursor.visible = false;
            if (Screen.currentResolution.width / 16f >= Screen.currentResolution.height / 9f)//太寬
            {
                Screen.SetResolution((int)(Screen.currentResolution.height * 16f / 9f), Screen.currentResolution.height, true);
            }
            else//太高
            {
                Screen.SetResolution(Screen.currentResolution.width, (int)(Screen.currentResolution.width * 9f / 16f), true);
            }
        }

        void Update()
        {
            Keyboard keyboard = Keyboard.current;
            Gamepad gamepad = Gamepad.current;
            timer += Time.deltaTime;
            if (timer > timerStoper)
            {
                SwitchScenePanel.NextScene = nextScene;
                GameObject.Find("SwitchScenePanel").GetComponent<Image>().sprite = nextSprite;
                if (nextSprite != null)
                {
                    GameObject.Find("SwitchScenePanel").GetComponent<Image>().color = Color.white;
                }
                GameObject.Find("SwitchScenePanel").GetComponent<Animator>().SetTrigger("Loading");
            }
            if (keyboard.anyKey.wasPressedThisFrame || (gamepad != null && gamepad.aButton.wasPressedThisFrame))
            {
                ButtonSelect.OnClicked();
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