using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace com.DungeonPad
{
    public class EndingRoller : MonoBehaviour
    {
        RectTransform rectTransform;
        float endingPosY;
        [SerializeField] Sprite[] endingSprites;
        // Start is called before the first frame update
        void Start()
        {
            if (PlayerPrefs.HasKey("layerFourCntinuousWinTimes"))
            {
                GameManager.layerFourCntinuousWinTimes = PlayerPrefs.GetInt("layerFourCntinuousWinTimes");
            }
            else
            {
                GameManager.layerFourCntinuousWinTimes = 0;
            }
            GetComponent<Image>().sprite = endingSprites[Mathf.Clamp(GameManager.layerFourCntinuousWinTimes++, 0, 2)];
            PlayerPrefs.SetInt("layerFourCntinuousWinTimes", GameManager.layerFourCntinuousWinTimes);
            PlayerPrefs.Save();

            rectTransform = GetComponent<RectTransform>();
            endingPosY = rectTransform.position.y + rectTransform.sizeDelta.y - 1080;
        }

        // Update is called once per frame
        void Update()
        {
            if(anyEnterHold() || anyExitHold())
            {
                rectTransform.Translate(Vector3.up * Time.deltaTime * 75 * 8);
            }
            else
            {
                rectTransform.Translate(Vector3.up * Time.deltaTime * 75);
            }
            if(rectTransform.position.y> endingPosY)
            {
                SwitchScenePanel.NextScene = "Win";
                GameObject.Find("SwitchScenePanel").GetComponent<Animator>().SetTrigger("Loading");
            }
        }


        #region// EnterKey / ExitKey
        public static bool keyboardEnterHold()
        {
            return Keyboard.current != null && Keyboard.current.enterKey.isPressed;
        }

        public static bool playerKeyboardEnterHold()
        {
            return Keyboard.current != null && (Keyboard.current.allKeys[InputManager.p1KeyboardDashNum].isPressed || Keyboard.current.allKeys[InputManager.p2KeyboardDashNum].isPressed);
        }

        public static bool twoPlayerGamepadEnterHold()
        {
            return Gamepad.current != null && Gamepad.current.aButton.isPressed;
        }

        public static bool onePlayerGamepadEnterHold()
        {
            return Gamepad.current != null && (Gamepad.current.leftShoulder.isPressed || Gamepad.current.rightShoulder.isPressed);
        }

        public static bool anyEnterHold()
        {
            return keyboardEnterHold() || playerKeyboardEnterHold() || twoPlayerGamepadEnterHold() || onePlayerGamepadEnterHold();
        }

        public static bool keyboardExitHold()
        {
            return Keyboard.current != null && Keyboard.current.escapeKey.isPressed;
        }

        public static bool playerKeyboardExitHold()
        {
            return Keyboard.current != null && (Keyboard.current.allKeys[InputManager.p1KeyboardBreakfreeKeyNum].isPressed || Keyboard.current.allKeys[InputManager.p2KeyboardBreakfreeKeyNum].isPressed);
        }

        public static bool twoPlayerGamepadExitHold()
        {
            return Gamepad.current != null && Gamepad.current.bButton.isPressed;
        }

        public static bool onePlayerGamepadExitHold()
        {
            return Gamepad.current != null && (Gamepad.current.leftTrigger.isPressed || Gamepad.current.rightTrigger.isPressed);
        }

        public static bool anyExitHold()
        {
            return keyboardExitHold() || playerKeyboardExitHold() || twoPlayerGamepadExitHold() || onePlayerGamepadExitHold();
        }
        #endregion
    }
}
