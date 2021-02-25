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
        public Transform BluePlayer, RedPlayer;
        public string NextScene;

        public enum PlayerColor
        {
            P1Blue_P2Red,
            P1Red_P2Blue
        }
        public static PlayerColor playerColor;

        void Update()
        {
            if (p1Stat == mouseStat.UnSelect && p2Stat == mouseStat.UnSelect)
            {
                p1Stat = selectMouse(true, p1Stat);
                if (Input.GetKeyDown(KeyCode.JoystickButton1))
                {
                    SceneManager.LoadScene("Home");
                }
            }
            if(p1Stat == mouseStat.SelectedMouse && p2Stat == mouseStat.UnSelect)
            {
                p1Stat = unSelectMouse(true, p1Stat);
                p1Stat = selectRole(true, p1Stat);
                p2Stat = selectMouse(false, p2Stat);
            }
            if(p1Stat == mouseStat.SelectedRole && p2Stat == mouseStat.UnSelect)
            {
                p1Stat = unSelectRole(true, p1Stat);
                p2Stat = selectMouse(false, p2Stat);
            }
            if (p1Stat == mouseStat.UnSelect &&p2Stat == mouseStat.SelectedMouse)
            {
                p1Stat = selectMouse(true, p1Stat);
                p2Stat = unSelectMouse(false, p2Stat);
                p2Stat = selectRole(false, p2Stat);
            }
            if (p1Stat == mouseStat.SelectedMouse && p2Stat == mouseStat.SelectedMouse)
            {
                p1Stat = unSelectMouse(true, p1Stat);
                p1Stat = selectRole(true, p1Stat);
                p2Stat = unSelectMouse(false, p2Stat);
                p2Stat = selectRole(false, p2Stat);
            }
            if (p1Stat == mouseStat.SelectedRole && p2Stat == mouseStat.SelectedMouse)
            {
                p1Stat = unSelectRole(true, p1Stat);
                p2Stat = unSelectMouse(false, p2Stat);
                p2Stat = selectRole(false, p2Stat);
            }
            if (p1Stat == mouseStat.UnSelect && p2Stat == mouseStat.SelectedRole)
            {
                p1Stat = selectMouse(true, p1Stat);
                p2Stat = unSelectRole(false, p2Stat);
            }
            if (p1Stat == mouseStat.SelectedMouse && p2Stat == mouseStat.SelectedRole)
            {
                p1Stat = unSelectMouse(true, p1Stat);
                p1Stat = selectRole(true, p1Stat);
                p2Stat = unSelectRole(false, p2Stat);
            }
            if (p1Stat == mouseStat.SelectedRole && p2Stat == mouseStat.SelectedRole)
            {
                //p1Stat = unSelectRole(true, p1Stat);
                //p2Stat = unSelectRole(false, p2Stat);
                if(GameManager.players.GetChild(0).position.x>20.2f&& GameManager.players.GetChild(1).position.x > 20.2f)
                {
                    if (BluePlayer.GetComponent<PlayerManager>().p1)
                    {
                        playerColor = PlayerColor.P1Blue_P2Red;
                    }
                    else
                    {
                        playerColor = PlayerColor.P1Red_P2Blue;
                    }
                    if (NextScene == "Game 3")
                    {
                        GameManager.layers = 2;
                        NextScene = "Game 1";
                    }
                    SwitchScenePanel.NextScene = NextScene;
                    GameObject.Find("SwitchScenePanel").GetComponent<Animator>().SetTrigger("Loading");
                }
            }
        }

        mouseStat selectMouse(bool p1, mouseStat mouseStat)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                if(p1 && p2Joy != "WASD")
                {
                    p1Joy = "WASD";
                    mouseStat = mouseStat.SelectedMouse;
                }
                if (!p1 && p1Joy != "WASD")
                {
                    p2Joy = "WASD";
                    mouseStat = mouseStat.SelectedMouse;
                }
                print("WASD");
            }
            else if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                if (p1 && p2Joy != "ArrowKey")
                {
                    p1Joy = "ArrowKey";
                    mouseStat = mouseStat.SelectedMouse;
                }
                if (!p1 && p1Joy != "ArrowKey")
                {
                    p2Joy = "ArrowKey";
                    mouseStat = mouseStat.SelectedMouse;
                }
                print("ArrowKey");
            }
            else
            {
                for (int i = 1; i <= 8; i++)
                {
                    if (Input.GetKeyDown((KeyCode)330 + 20 * i))
                    {
                        if (p1 && p2Joy != "" + i)
                        {
                            p1Joy = "" + i;
                            print(((KeyCode)330 + 20 * i).ToString());
                            for (int j = 0; j < 4; j++)
                            {
                                if (GamePad.GetState((PlayerIndex)j).Buttons.A == ButtonState.Pressed)
                                {
                                    if (p1)
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
                        if (!p1 && p1Joy != "" + i)
                        {
                            p2Joy = "" + i;
                            print(((KeyCode)330 + 20 * i).ToString());
                            for (int j = 0; j < 4; j++)
                            {
                                if (GamePad.GetState((PlayerIndex)j).Buttons.A == ButtonState.Pressed)
                                {
                                    if (p1)
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
            return mouseStat;
        }

        mouseStat unSelectMouse(bool p1, mouseStat mouseStat)
        {
            if(p1)
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
            return mouseStat;
        }

        mouseStat selectRole(bool p1, mouseStat mouseStat)
        {
            if(p1)
            {
                switch (p1Joy)
                {
                    case "WASD":
                        if (!Input.GetKeyDown(KeyCode.J))
                        {
                            return mouseStat;
                        }
                        break;
                    case "ArrowKey":
                        if (!Input.GetKeyDown(KeyCode.Keypad1))
                        {
                            return mouseStat;
                        }
                        break;
                    case "1":
                    case "2":
                    case "3":
                    case "4":
                    case "5":
                    case "6":
                    case "7":
                    case "8":
                        if (!Input.GetKeyDown((KeyCode)(330 + 20 * int.Parse(p1Joy))))
                        {
                            return mouseStat;
                        }
                        break;
                }
                if (mouse1.anchoredPosition.x - blue.anchoredPosition.x < 90 && mouse1.anchoredPosition.x - blue.anchoredPosition.x > 10 && mouse1.anchoredPosition.y - blue.anchoredPosition.y < 90 && mouse1.anchoredPosition.y - blue.anchoredPosition.y > 10)
                {
                    mouseStat = mouseStat.SelectedRole;
                    BluePlayer.GetComponent<PlayerManager>().playerStat = PlayerManager.PlayerStat.Move;
                    BluePlayer.GetComponent<PlayerManager>().p1 = true;
                    BluePlayer.GetComponent<PlayerJoyVibration>().enabled = true;
                    blue.anchoredPosition = new Vector3(9999, 0, 0);
                    blue.GetComponent<playerPosToUIPos>().enabled = false;
                }
                if (mouse1.anchoredPosition.x - red.anchoredPosition.x < 90 && mouse1.anchoredPosition.x - red.anchoredPosition.x > 10 && mouse1.anchoredPosition.y - red.anchoredPosition.y < 90 && mouse1.anchoredPosition.y - red.anchoredPosition.y > 10)
                {
                    mouseStat = mouseStat.SelectedRole;
                    RedPlayer.GetComponent<PlayerManager>().playerStat = PlayerManager.PlayerStat.Move;
                    RedPlayer.GetComponent<PlayerManager>().p1 = true;
                    RedPlayer.GetComponent<PlayerJoyVibration>().enabled = true;
                    red.anchoredPosition = new Vector3(9999, 0, 0);
                    red.GetComponent<playerPosToUIPos>().enabled = false;
                }
            }
            else
            {
                switch (p2Joy)
                {
                    case "WASD":
                        if (!Input.GetKeyDown(KeyCode.J))
                        {
                            return mouseStat;
                        }
                        break;
                    case "ArrowKey":
                        if (!Input.GetKeyDown(KeyCode.Keypad1))
                        {
                            return mouseStat;
                        }
                        break;
                    case "1":
                    case "2":
                    case "3":
                    case "4":
                    case "5":
                    case "6":
                    case "7":
                    case "8":
                        if (!Input.GetKeyDown((KeyCode)(330 + 20 * int.Parse(p2Joy))))
                        {
                            return mouseStat;
                        }
                        break;
                }
                if (Mathf.Abs(mouse2.anchoredPosition.x - blue.anchoredPosition.x) < 50 && Mathf.Abs(mouse2.anchoredPosition.y - blue.anchoredPosition.y) < 50)
                {
                    mouseStat = mouseStat.SelectedRole;
                    BluePlayer.GetComponent<PlayerManager>().playerStat = PlayerManager.PlayerStat.Move;
                    BluePlayer.GetComponent<PlayerManager>().p1 = false;
                    BluePlayer.GetComponent<PlayerJoyVibration>().enabled = true;
                    blue.anchoredPosition = new Vector3(9999, 0, 0);
                    blue.GetComponent<playerPosToUIPos>().enabled = false;
                }
                if (Mathf.Abs(mouse2.anchoredPosition.x - red.anchoredPosition.x) < 50 && Mathf.Abs(mouse2.anchoredPosition.y - red.anchoredPosition.y) < 50)
                {
                    mouseStat = mouseStat.SelectedRole;
                    RedPlayer.GetComponent<PlayerManager>().playerStat = PlayerManager.PlayerStat.Move;
                    RedPlayer.GetComponent<PlayerManager>().p1 = false;
                    RedPlayer.GetComponent<PlayerJoyVibration>().enabled = true;
                    red.anchoredPosition = new Vector3(9999, 0, 0);
                    red.GetComponent<playerPosToUIPos>().enabled = false;
                }
            }
            return mouseStat;
        }

        mouseStat unSelectRole(bool p1, mouseStat mouseStat)
        {
            if(p1)
            {
                if (p1Joy == "WASD" && Input.GetKeyDown(KeyCode.K))
                {
                    mouseStat = mouseStat.SelectedMouse;
                    if(BluePlayer.GetComponent<PlayerManager>().playerStat == PlayerManager.PlayerStat.Move && BluePlayer.GetComponent<PlayerManager>().p1)
                    {
                        BluePlayer.GetComponent<PlayerManager>().playerStat = PlayerManager.PlayerStat.UnSelect;
                        BluePlayer.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                        BluePlayer.GetComponent<PlayerJoyVibration>().enabled = false;
                        blue.anchoredPosition = new Vector2(-100, -300);
                    }
                    else
                    {
                        RedPlayer.GetComponent<PlayerManager>().playerStat = PlayerManager.PlayerStat.UnSelect;
                        RedPlayer.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                        RedPlayer.GetComponent<PlayerJoyVibration>().enabled = false;
                        red.anchoredPosition = new Vector2(100, -300);
                    }
                    print("back");
                }
                else if (p1Joy == "ArrowKey" && Input.GetKeyDown(KeyCode.Keypad2))
                {
                    mouseStat = mouseStat.SelectedMouse;
                    if (BluePlayer.GetComponent<PlayerManager>().playerStat == PlayerManager.PlayerStat.Move && BluePlayer.GetComponent<PlayerManager>().p1)
                    {
                        BluePlayer.GetComponent<PlayerManager>().playerStat = PlayerManager.PlayerStat.UnSelect;
                        BluePlayer.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                        BluePlayer.GetComponent<PlayerJoyVibration>().enabled = false;
                        blue.GetComponent<playerPosToUIPos>().enabled = true;
                    }
                    else
                    {
                        RedPlayer.GetComponent<PlayerManager>().playerStat = PlayerManager.PlayerStat.UnSelect;
                        RedPlayer.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                        RedPlayer.GetComponent<PlayerJoyVibration>().enabled = false;
                        red.GetComponent<playerPosToUIPos>().enabled = true;
                    }
                    print("back");
                }
                if (p1Joy != "WASD" && p1Joy != "ArrowKey" && Input.GetKeyDown((KeyCode)(330 + 20 * int.Parse(p1Joy) + 1)))
                {
                    mouseStat = mouseStat.SelectedMouse;
                    if (BluePlayer.GetComponent<PlayerManager>().playerStat == PlayerManager.PlayerStat.Move && BluePlayer.GetComponent<PlayerManager>().p1)
                    {
                        BluePlayer.GetComponent<PlayerManager>().playerStat = PlayerManager.PlayerStat.UnSelect;
                        BluePlayer.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                        BluePlayer.GetComponent<PlayerJoyVibration>().enabled = false;
                        blue.GetComponent<playerPosToUIPos>().enabled = true;
                    }
                    else
                    {
                        RedPlayer.GetComponent<PlayerManager>().playerStat = PlayerManager.PlayerStat.UnSelect;
                        RedPlayer.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                        RedPlayer.GetComponent<PlayerJoyVibration>().enabled = false;
                        red.GetComponent<playerPosToUIPos>().enabled = true;
                    }
                    print("back");
                }
            }
            else
            {
                if (p2Joy == "WASD" && Input.GetKeyDown(KeyCode.K))
                {
                    mouseStat = mouseStat.SelectedMouse;
                    if (BluePlayer.GetComponent<PlayerManager>().playerStat == PlayerManager.PlayerStat.Move && !BluePlayer.GetComponent<PlayerManager>().p1)
                    {
                        BluePlayer.GetComponent<PlayerManager>().playerStat = PlayerManager.PlayerStat.UnSelect;
                        BluePlayer.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                        BluePlayer.GetComponent<PlayerJoyVibration>().enabled = false;
                        blue.anchoredPosition = new Vector2(-100, -300);
                    }
                    else
                    {
                        RedPlayer.GetComponent<PlayerManager>().playerStat = PlayerManager.PlayerStat.UnSelect;
                        RedPlayer.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                        RedPlayer.GetComponent<PlayerJoyVibration>().enabled = false;
                        red.anchoredPosition = new Vector2(100, -300);
                    }
                    print("back");
                }
                else if (p2Joy == "ArrowKey" && Input.GetKeyDown(KeyCode.Keypad2))
                {
                    mouseStat = mouseStat.SelectedMouse;
                    if (BluePlayer.GetComponent<PlayerManager>().playerStat == PlayerManager.PlayerStat.Move && !BluePlayer.GetComponent<PlayerManager>().p1)
                    {
                        BluePlayer.GetComponent<PlayerManager>().playerStat = PlayerManager.PlayerStat.UnSelect;
                        BluePlayer.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                        BluePlayer.GetComponent<PlayerJoyVibration>().enabled = false;
                        blue.anchoredPosition = new Vector2(-100, -300);
                    }
                    else
                    {
                        RedPlayer.GetComponent<PlayerManager>().playerStat = PlayerManager.PlayerStat.UnSelect;
                        RedPlayer.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                        RedPlayer.GetComponent<PlayerJoyVibration>().enabled = false;
                        red.anchoredPosition = new Vector2(100, -300);
                    }
                    print("back");
                }
                if (p2Joy != "WASD" && p2Joy != "ArrowKey" && Input.GetKeyDown((KeyCode)(330 + 20 * int.Parse(p2Joy) + 1)))
                {
                    mouseStat = mouseStat.SelectedMouse;
                    if (BluePlayer.GetComponent<PlayerManager>().playerStat == PlayerManager.PlayerStat.Move && !BluePlayer.GetComponent<PlayerManager>().p1)
                    {
                        BluePlayer.GetComponent<PlayerManager>().playerStat = PlayerManager.PlayerStat.UnSelect;
                        BluePlayer.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                        BluePlayer.GetComponent<PlayerJoyVibration>().enabled = false;
                        blue.anchoredPosition = new Vector2(-100, -300);
                    }
                    else
                    {
                        RedPlayer.GetComponent<PlayerManager>().playerStat = PlayerManager.PlayerStat.UnSelect;
                        RedPlayer.GetComponent<PlayerManager>().playerStat = PlayerManager.PlayerStat.UnSelect;
                        RedPlayer.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                        blue.anchoredPosition = new Vector2(100, -300);
                    }
                    print("back");
                }
            }
            return mouseStat;
        }

        IEnumerator waitForLoadScene()
        {
            yield return new WaitForSeconds(1);
            if (BluePlayer.GetComponent<PlayerManager>().p1)
            {
                playerColor = PlayerColor.P1Blue_P2Red;
            }
            else
            {
                playerColor = PlayerColor.P1Red_P2Blue;
            }
            SceneManager.LoadScene(NextScene);
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