using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace com.DungeonPad
{
    public class StopPanel : MonoBehaviour
    {
        private void OnEnable()
        {
            StartCoroutine(SelectButtonLater());
        }

        void Update()
        {
            Keyboard keyboard = Keyboard.current;
            if (keyboard.escapeKey.wasPressedThisFrame || (InputManager.currentGamepad != null && InputManager.currentGamepad.bButton.wasPressedThisFrame) && !SceneManager.GetActiveScene().name.Contains("Select"))
            {
                back();
            }
        }

        public void back()
        {
            Time.timeScale = 1;
            Time.fixedDeltaTime = 0.02F * Time.timeScale;
            gameObject.SetActive(false);
        }

        public void setting()
        {
            gameObject.SetActive(false);
            GameManager.settingPanel.SetActive(true);
        }

        public void home()
        {
            back();
            SwitchScenePanel.NextScene = "Home";
            GameObject.Find("SwitchScenePanel").GetComponent<Animator>().SetTrigger("Loading");
        }

        private IEnumerator SelectButtonLater()
        {
            Image image = transform.GetChild(0).GetChild(0).GetComponent<Image>();
            Sprite sp = image.sprite;
            image.sprite = transform.GetChild(0).GetChild(0).GetComponent<Button>().spriteState.selectedSprite;
            yield return null;
            EventSystem.current.SetSelectedGameObject(transform.GetChild(0).GetChild(0).gameObject);
            image.sprite = sp;
        }
    }
}