using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class test : MonoBehaviour
{
    public static string p1Joy, p2Joy;
    public bool selectP1 = true;

    void Update()
    {
        if (selectP1)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                p1Joy = "WASD";
                print("WASD");
                selectP1 = false;
            }
            else if(Input.GetKeyDown(KeyCode.Keypad1))
            {
                p1Joy = "ArrowKey";
                print("ArrowKey");
                selectP1 = false;
            }
            else
            {
                for (int i = 1; i <= 8; i++)
                {
                    if (Input.GetKeyDown((KeyCode)330 + 20 * i))
                    {
                        p1Joy = "" + i;
                        print(((KeyCode)330 + 20 * i).ToString());
                        selectP1 = false;
                        break;
                    }
                }
            }
        }
        else if (p1Joy == "WASD" && Input.GetKeyDown(KeyCode.K))
        {
            selectP1 = true;
            p1Joy = "";
            print("back");
        }
        else if (p1Joy == "ArrowKey" && Input.GetKeyDown(KeyCode.Keypad2))
        {
            selectP1 = true;
            p1Joy = "";
            print("back");
        }
        else if (p1Joy == "WASD" || p1Joy == "ArrowKey")
        {

        }
        else if (Input.GetKeyDown((KeyCode)(330 + 20 * int.Parse(p1Joy) + 1)))
        {
            selectP1 = true;
            p1Joy = "";
            print("back");
        }

        if (!selectP1)
        {
            if (Input.GetKeyDown(KeyCode.J) && p1Joy != "WASD")
            {
                p2Joy = "WASD";
                print("WASD");
                SceneManager.LoadScene("Game 1");
            }
            else if (Input.GetKeyDown(KeyCode.Keypad1) && p1Joy != "ArrowKey")
            {
                p2Joy = "ArrowKey";
                print("ArrowKey");
                SceneManager.LoadScene("Game 1");
            }
            else
            {
                for (int i = 1; i <= 8; i++)
                {
                    if (Input.GetKeyDown((KeyCode)330 + 20 * i) && p1Joy != "" + i)
                    {
                        p2Joy = "" + i;
                        print(((KeyCode)330 + 20 * i).ToString());
                        SceneManager.LoadScene("Game 1");
                        break;
                    }
                }
            }
        }
    }
}
