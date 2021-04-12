using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace com.DungeonPad
{
    public class ButtonSelect : MonoBehaviour
    {
        Button button;
        [SerializeField] bool canAnyEnter;
        [SerializeField] GameObject selectedSFX, clickedSFX;
        public static GameObject SelectedSFX, ClickedSFX;
        void Start()
        {
            button = GetComponent<Button>();
            if (SelectedSFX == null)
            {
                SelectedSFX = selectedSFX;
            }
            if (ClickedSFX == null)
            {
                ClickedSFX = clickedSFX;
            }
        }

        void Update()
        {
            if (canAnyEnter && InputManager.anyEnter() && EventSystem.current.currentSelectedGameObject == gameObject)
            {
                button.onClick.Invoke();
                onClicked();
            }
        }

        public void onSelected()
        {
            OnSelected();
        }

        public void onClicked()
        {
            OnClicked();
        }

        public static void OnSelected()
        {
            try
            {
                Destroy(Instantiate(SelectedSFX, Camera.main.transform.position, Quaternion.identity), 3);
            }
            catch
            {

            }
        }

        public static void OnClicked()
        {
            try
            {
                Destroy(Instantiate(ClickedSFX, Camera.main.transform.position, Quaternion.identity), 3);
            }
            catch
            {

            }
        }
    }
}
