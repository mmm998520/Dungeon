using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomePageMusic : MonoBehaviour
{
    private void Awake()
    {
        transform.position = Camera.main.transform.position;
        GameObject[] objs = GameObject.FindGameObjectsWithTag("music");
        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
