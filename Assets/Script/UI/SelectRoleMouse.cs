/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace com.DungeonPad
{
    public class SelectRoleMouse : MonoBehaviour
    {
        public bool p1;
        float mouseSpeed = 3000;
        RectTransform rectTransform;
        public bool canUse;
        public SelectMouse selectMouse;
        // Start is called before the first frame update
        void Start()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        // Update is called once per frame
        void Update()
        {
            canUse = ((p1 && selectMouse.p1Stat == SelectMouse.mouseStat.SelectedMouse) || (!p1 && selectMouse.p2Stat == SelectMouse.mouseStat.SelectedMouse));
            GetComponent<Image>().enabled = canUse;
            if (canUse)
            {
                move();
            }
        }

        void move()
        {
            Vector2 mouseMove = Vector3.zero;
            if (p1)
            {
                switch (SelectMouse.p1Joy)
                {
                    case "WASD":
                        mouseMove.x = Input.GetAxis("HorizontalWASD") * Time.deltaTime * mouseSpeed;
                        mouseMove.y = Input.GetAxis("VerticalWASD") * Time.deltaTime * mouseSpeed;
                        break;
                    case "ArrowKey":
                        mouseMove.x = Input.GetAxis("HorizontalArrowKey") * Time.deltaTime * mouseSpeed;
                        mouseMove.y = Input.GetAxis("VerticalArrowKey") * Time.deltaTime * mouseSpeed;
                        break;
                    case "1":
                    case "2":
                    case "3":
                    case "4":
                    case "5":
                    case "6":
                    case "7":
                    case "8":
                        mouseMove.x = Input.GetAxis("HorizontalJoy" + SelectMouse.p1Joy) * Time.deltaTime * mouseSpeed;
                        mouseMove.y = -Input.GetAxis("VerticalJoy" + SelectMouse.p1Joy) * Time.deltaTime * mouseSpeed;
                        break;
                }
            }
            else
            {
                switch (SelectMouse.p2Joy)
                {
                    case "WASD":
                        mouseMove.x = Input.GetAxis("HorizontalWASD") * Time.deltaTime * mouseSpeed;
                        mouseMove.y = Input.GetAxis("VerticalWASD") * Time.deltaTime * mouseSpeed;
                        break;
                    case "ArrowKey":
                        mouseMove.x = Input.GetAxis("HorizontalArrowKey") * Time.deltaTime * mouseSpeed;
                        mouseMove.y = Input.GetAxis("VerticalArrowKey") * Time.deltaTime * mouseSpeed;
                        break;
                    case "1":
                    case "2":
                    case "3":
                    case "4":
                    case "5":
                    case "6":
                    case "7":
                    case "8":
                        mouseMove.x = Input.GetAxis("HorizontalJoy" + SelectMouse.p2Joy) * Time.deltaTime * mouseSpeed;
                        mouseMove.y = -Input.GetAxis("VerticalJoy" + SelectMouse.p2Joy) * Time.deltaTime * mouseSpeed;
                        break;
                }
            }
            rectTransform.anchoredPosition += mouseMove;
            if (rectTransform.anchoredPosition.x > Screen.width)
            {
                rectTransform.anchoredPosition = new Vector2(Screen.width, rectTransform.anchoredPosition.y);
            }
            if (rectTransform.anchoredPosition.x < 100)
            {
                rectTransform.anchoredPosition = new Vector2(100, rectTransform.anchoredPosition.y);
            }
            if (rectTransform.anchoredPosition.y > Screen.height)
            {
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, Screen.height);
            }
            if (rectTransform.anchoredPosition.y < 100)
            {
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, 100);
            }
        }
    }
}*/