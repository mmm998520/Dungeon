using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace com.DungeonPad
{
    public class SelectRoleMouse : MonoBehaviour
    {
        public bool p1;
        float mouseSpeed = 3;
        RectTransform rectTransform;
        public bool canUse;
        public bool selected = false;

        // Start is called before the first frame update
        void Start()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        // Update is called once per frame
        void Update()
        {
            canUse = !selected && ((p1 && SelectRole.p1Joy != "") || (!p1 && SelectRole.p2Joy != ""));
            GetComponent<Image>().enabled = canUse;
            move();
        }

        void move()
        {
            Vector2 mouseMove = Vector3.zero;
            if (p1)
            {
                switch (SelectRole.p1Joy)
                {
                    case "WASD":
                        mouseMove.x = Input.GetAxis("HorizontalWASD") * mouseSpeed;
                        mouseMove.y = -Input.GetAxis("VerticalWASD") * mouseSpeed;
                        break;
                    case "ArrowKey":
                        mouseMove.x = Input.GetAxis("HorizontalArrowKey") * mouseSpeed;
                        mouseMove.y = -Input.GetAxis("VerticalArrowKey") * mouseSpeed;
                        break;
                    case "1":
                    case "2":
                    case "3":
                    case "4":
                    case "5":
                    case "6":
                    case "7":
                    case "8":
                        mouseMove.x = Input.GetAxis("HorizontalJoy" + SelectRole.p1Joy) * mouseSpeed;
                        mouseMove.y = -Input.GetAxis("VerticalJoy" + SelectRole.p1Joy) * mouseSpeed;
                        break;
                }
            }
            else
            {
                switch (SelectRole.p2Joy)
                {
                    case "WASD":
                        mouseMove.x = Input.GetAxis("HorizontalWASD") * mouseSpeed;
                        mouseMove.y = -Input.GetAxis("VerticalWASD") * mouseSpeed;
                        break;
                    case "ArrowKey":
                        mouseMove.x = Input.GetAxis("HorizontalArrowKey") * mouseSpeed;
                        mouseMove.y = -Input.GetAxis("VerticalArrowKey") * mouseSpeed;
                        break;
                    case "1":
                    case "2":
                    case "3":
                    case "4":
                    case "5":
                    case "6":
                    case "7":
                    case "8":
                        mouseMove.x = Input.GetAxis("HorizontalJoy" + SelectRole.p2Joy) * mouseSpeed;
                        mouseMove.y = -Input.GetAxis("VerticalJoy" + SelectRole.p2Joy) * mouseSpeed;
                        break;
                }
            }
            rectTransform.anchoredPosition += mouseMove;
        }
    }
}