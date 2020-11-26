using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadscene : MonoBehaviour
{
    void Start()
    {
        
    }

    
    public void selestgame()
    {
      SceneManager.LoadScene("SelectRole_Game 1");
    }

    public void selecttutorial()
    {
        SceneManager.LoadScene("SelectRole_Tutorial 1");
    }

    public void selectSetting()
    {
        SceneManager.LoadScene("Setting");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            Debug.Log("e");
            SceneManager.LoadScene("Home");
        }
    }

}
