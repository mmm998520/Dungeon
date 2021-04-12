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
        void Start()
        {
            button = GetComponent<Button>();
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
            Debug.LogWarning("onSelected");
            Instantiate(selectedSFX, Camera.main.transform.position, Quaternion.identity);
        }

        public void onClicked()
        {
            Debug.LogWarning("onClicked");
            Instantiate(clickedSFX, Camera.main.transform.position, Quaternion.identity);
        }
    }
}
