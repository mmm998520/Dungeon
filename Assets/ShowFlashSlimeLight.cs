using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowFlashSlimeLight : MonoBehaviour
{
    public Image image;
    public static float LightTimer = 0;

    void Start()
    {
        
    }

    void Update()
    {
        if (LightTimer > 0)
        {
            LightTimer -= Time.deltaTime;
            image.color = new Color(1, 1, 1, Mathf.Atan(LightTimer *4/5 - 2f));
        }
        else
        {
            image.color = Color.clear;
        }
    }
}
