using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        for(int i = 0; i < 20; i++)
        {
            if (Input.GetKeyDown((KeyCode)330+i))
            {
                Debug.Log(i);
            }
        }
        Debug.Log("HorizontalJoyP1 : " + Input.GetAxis("HorizontalJoyP1"));
        Debug.Log("VerticalJoyP1 : " + Input.GetAxis("VerticalJoyP1"));
        Debug.Log("HorizontalJoyP2 : " + Input.GetAxis("HorizontalJoyP2"));
        Debug.Log("VerticalJoyP2 : " + Input.GetAxis("VerticalJoyP2"));
        */
        Debug.Log("RT : " + Input.GetAxis("RT"));
        Debug.Log("LT : " + Input.GetAxis("LT"));
    }
}
