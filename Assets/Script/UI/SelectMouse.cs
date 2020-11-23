using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using XInputDotNetPure; // Required in C#

namespace com.DungeonPad
{
    public class SelectMouse : MonoBehaviour
    {
        public enum mouseStat
        {
            UnSelect = 0,
            SelectedMouse = 1,
            SelectedRole = 2
        }
        public mouseStat p1Stat, p2Stat;
        public static string p1Joy = "", p2Joy = "";
        public static PlayerIndex? P1PlayerIndex = null, P2PlayerIndex = null;
        public RectTransform blue, red, mouse1, mouse2;
        Transform BluePlayer, RedPlayer;
        void Start()
        {

        }

        void Update()
        {
            if (p1Stat == mouseStat.UnSelect && p2Stat == mouseStat.UnSelect)
            {
                selectMouse(p1Stat);
                if (Input.GetKeyDown(KeyCode.JoystickButton1))
                {
                    SceneManager.LoadScene("Home");
                }
            }
            if(p1Stat == mouseStat.SelectedMouse && p2Stat == mouseStat.UnSelect)
            {
                unSelectMouse(p1Stat);
                selectRole(p1Stat);
                selectMouse(p2Stat);
            }
            if(p1Stat == mouseStat.SelectedRole && p2Stat == mouseStat.UnSelect)
            {

            }
            if(p1Stat == mouseStat.UnSelect &&p2Stat == mouseStat.SelectedMouse)
            {
                selectMouse(p1Stat);
                unSelectMouse(p2Stat);
                selectRole(p2Stat);
            }
            if (p1Stat == mouseStat.SelectedMouse && p2Stat == mouseStat.SelectedMouse)
            {
                unSelectMouse(p1Stat);
                selectRole(p1Stat);
                unSelectMouse(p2Stat);
                selectRole(p2Stat);
            }
            if (p1Stat == mouseStat.SelectedRole && p2Stat == mouseStat.SelectedMouse)
            {

            }
            if (p1Stat == mouseStat.UnSelect && p2Stat == mouseStat.SelectedRole)
            {

            }
            if (p1Stat == mouseStat.SelectedMouse && p2Stat == mouseStat.SelectedRole)
            {

            }
            if (p1Stat == mouseStat.SelectedRole && p2Stat == mouseStat.SelectedRole)
            {

            }
        }

        void selectMouse(mouseStat mouseStat)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                if(mouseStat == p1Stat)
                {
                    p1Joy = "WASD";
                }
                else
                {
                    p2Joy = "WASD";
                }
                print("WASD");
                mouseStat = mouseStat.SelectedMouse;
            }
            else if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                if (mouseStat == p1Stat)
                {
                    p1Joy = "ArrowKey";
                }
                else
                {
                    p2Joy = "ArrowKey";
                }
                print("ArrowKey");
                mouseStat = mouseStat.SelectedMouse;
            }
            else
            {
                for (int i = 1; i <= 8; i++)
                {
                    if (Input.GetKeyDown((KeyCode)330 + 20 * i))
                    {
                        if (mouseStat == p1Stat)
                        {
                            p1Joy = "" + i;
                        }
                        else
                        {
                            p2Joy = "" + i;
                        }
                        print(((KeyCode)330 + 20 * i).ToString());
                        for (int j = 0; j < 4; j++)
                        {
                            if (GamePad.GetState((PlayerIndex)j).Buttons.A == ButtonState.Pressed)
                            {
                                if (mouseStat == p1Stat)
                                {
                                    P1PlayerIndex = (PlayerIndex)j;
                                    print(i + "," + P1PlayerIndex);
                                    StartCoroutine(waitForVP1());
                                    break;
                                }
                                else
                                {
                                    P2PlayerIndex = (PlayerIndex)j;
                                    print(i + "," + P2PlayerIndex);
                                    StartCoroutine(waitForVP2());
                                    break;
                                }
                            }
                            print(j);
                        }
                        mouseStat = mouseStat.SelectedMouse;
                        break;
                    }
                }
            }
        }

        void unSelectMouse(mouseStat mouseStat)
        {
            if(mouseStat == p1Stat)
            {
                if (p1Joy == "WASD" && Input.GetKeyDown(KeyCode.K))
                {
                    mouseStat = mouseStat.UnSelect;
                    p1Joy = "";
                    print("back");
                }
                else if (p1Joy == "ArrowKey" && Input.GetKeyDown(KeyCode.Keypad2))
                {
                    mouseStat = mouseStat.UnSelect;
                    p1Joy = "";
                    print("back");
                }
                if (p1Joy != "WASD" && p1Joy != "ArrowKey" && Input.GetKeyDown((KeyCode)(330 + 20 * int.Parse(p1Joy) + 1)))
                {
                    mouseStat = mouseStat.UnSelect;
                    p1Joy = "";
                    print("back");
                }
            }
            else
            {
                if (p2Joy == "WASD" && Input.GetKeyDown(KeyCode.K))
                {
                    mouseStat = mouseStat.UnSelect;
                    p2Joy = "";
                    print("back");
                }
                else if (p2Joy == "ArrowKey" && Input.GetKeyDown(KeyCode.Keypad2))
                {
                    mouseStat = mouseStat.UnSelect;
                    p2Joy = "";
                    print("back");
                }
                if (p2Joy != "WASD" && p2Joy != "ArrowKey" && Input.GetKeyDown((KeyCode)(330 + 20 * int.Parse(p2Joy) + 1)))
                {
                    mouseStat = mouseStat.UnSelect;
                    p2Joy = "";
                    print("back");
                }
            }
        }

        void selectRole(mouseStat mouseStat)
        {
            if(mouseStat == p1Stat)
            {
                if (Mathf.Abs(mouse1.anchoredPosition.x - blue.anchoredPosition.x) < 50 && Mathf.Abs(mouse1.anchoredPosition.y - blue.anchoredPosition.y) < 50)
                {
                    mouseStat = mouseStat.SelectedRole;
                    BluePlayer.GetComponent<PlayerManager>().enabled = true;
                    BluePlayer.GetComponent<PlayerManager>().p1 = true;
                    blue.anchoredPosition = new Vector3(9999, 0, 0);
                }
                if (Mathf.Abs(mouse1.anchoredPosition.x - red.anchoredPosition.x) < 50 && Mathf.Abs(mouse1.anchoredPosition.y - red.anchoredPosition.y) < 50)
                {
                    mouseStat = mouseStat.SelectedRole;
                    RedPlayer.GetComponent<PlayerManager>().enabled = true;
                    RedPlayer.GetComponent<PlayerManager>().p1 = true;
                    red.anchoredPosition = new Vector3(9999, 0, 0);
                }
            }
            else
            {
                if (Mathf.Abs(mouse2.anchoredPosition.x - blue.anchoredPosition.x) < 50 && Mathf.Abs(mouse2.anchoredPosition.y - blue.anchoredPosition.y) < 50)
                {
                    mouseStat = mouseStat.SelectedRole;
                    BluePlayer.GetComponent<PlayerManager>().enabled = true;
                    BluePlayer.GetComponent<PlayerManager>().p1 = false;
                    blue.anchoredPosition = new Vector3(9999, 0, 0);
                }
                if (Mathf.Abs(mouse2.anchoredPosition.x - red.anchoredPosition.x) < 50 && Mathf.Abs(mouse2.anchoredPosition.y - red.anchoredPosition.y) < 50)
                {
                    mouseStat = mouseStat.SelectedRole;
                    RedPlayer.GetComponent<PlayerManager>().enabled = true;
                    RedPlayer.GetComponent<PlayerManager>().p1 = false;
                    red.anchoredPosition = new Vector3(9999, 0, 0);
                }
            }
        }

        void unSelectRole(mouseStat mouseStat)
        {
            if (mouseStat == p1Stat)
            {
                if (p1Joy == "WASD" && Input.GetKeyDown(KeyCode.K))
                {
                    mouseStat = mouseStat.SelectedMouse;
                    if(BluePlayer.GetComponent<PlayerManager>().enabled && BluePlayer.GetComponent<PlayerManager>().p1)
                    {
                        BluePlayer.GetComponent<PlayerManager>().enabled = false;
                    }
                    else
                    {
                        RedPlayer.GetComponent<PlayerManager>().enabled = false;
                    }
                    blue.anchoredPosition = new Vector2(640, 540);
                    print("back");
                }
                else if (p1Joy == "ArrowKey" && Input.GetKeyDown(KeyCode.Keypad2))
                {
                    mouseStat = mouseStat.SelectedMouse;
                    if (BluePlayer.GetComponent<PlayerManager>().enabled && BluePlayer.GetComponent<PlayerManager>().p1)
                    {
                        BluePlayer.GetComponent<PlayerManager>().enabled = false;
                    }
                    else
                    {
                        RedPlayer.GetComponent<PlayerManager>().enabled = false;
                    }
                    blue.anchoredPosition = new Vector2(640, 540);
                    print("back");
                }
                if (p1Joy != "WASD" && p1Joy != "ArrowKey" && Input.GetKeyDown((KeyCode)(330 + 20 * int.Parse(p1Joy) + 1)))
                {
                    mouseStat = mouseStat.SelectedMouse;
                    if (BluePlayer.GetComponent<PlayerManager>().enabled && BluePlayer.GetComponent<PlayerManager>().p1)
                    {
                        BluePlayer.GetComponent<PlayerManager>().enabled = false;
                    }
                    else
                    {
                        RedPlayer.GetComponent<PlayerManager>().enabled = false;
                    }
                    blue.anchoredPosition = new Vector2(640, 540);
                    print("back");
                }
            }
            else
            {
                if (p2Joy == "WASD" && Input.GetKeyDown(KeyCode.K))
                {
                    mouseStat = mouseStat.SelectedMouse;
                    if (BluePlayer.GetComponent<PlayerManager>().enabled && !BluePlayer.GetComponent<PlayerManager>().p1)
                    {
                        BluePlayer.GetComponent<PlayerManager>().enabled = false;
                    }
                    else
                    {
                        RedPlayer.GetComponent<PlayerManager>().enabled = false;
                    }
                    blue.anchoredPosition = new Vector2(640, 540);
                    print("back");
                }
                else if (p2Joy == "ArrowKey" && Input.GetKeyDown(KeyCode.Keypad2))
                {
                    mouseStat = mouseStat.SelectedMouse;
                    if (BluePlayer.GetComponent<PlayerManager>().enabled && !BluePlayer.GetComponent<PlayerManager>().p1)
                    {
                        BluePlayer.GetComponent<PlayerManager>().enabled = false;
                    }
                    else
                    {
                        RedPlayer.GetComponent<PlayerManager>().enabled = false;
                    }
                    blue.anchoredPosition = new Vector2(1280, 540);
                    print("back");
                }
                if (p2Joy != "WASD" && p2Joy != "ArrowKey" && Input.GetKeyDown((KeyCode)(330 + 20 * int.Parse(p2Joy) + 1)))
                {
                    mouseStat = mouseStat.SelectedMouse;
                    if (BluePlayer.GetComponent<PlayerManager>().enabled && !BluePlayer.GetComponent<PlayerManager>().p1)
                    {
                        BluePlayer.GetComponent<PlayerManager>().enabled = false;
                    }
                    else
                    {
                        RedPlayer.GetComponent<PlayerManager>().enabled = false;
                    }
                    blue.anchoredPosition = new Vector2(1280, 540);
                    print("back");
                }
            }
        }

        IEnumerator waitForVP1()
        {
            GamePad.SetVibration(P1PlayerIndex.Value, 1, 1);
            yield return new WaitForSeconds(0.5f);
            GamePad.SetVibration(P1PlayerIndex.Value, 0, 0);
        }

        IEnumerator waitForVP2()
        {
            GamePad.SetVibration(P2PlayerIndex.Value, 1, 1);
            yield return new WaitForSeconds(0.5f);
            GamePad.SetVibration(P2PlayerIndex.Value, 0, 0);
        }
    }
}