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
        void Start()
        {
            button = GetComponent<Button>();
        }

        void Update()
        {
            if (InputManager.anyEnter() && EventSystem.current.currentSelectedGameObject == gameObject)
            {
                button.onClick.Invoke();
            }
        }
    }
}
