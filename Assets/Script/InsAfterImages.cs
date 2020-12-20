using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsAfterImages : MonoBehaviour
{
    public GameObject dash, afterImg;
    public float timer;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 0.1)
        {
            Instantiate(afterImg, new Vector2(dash.transform.position.x, dash.transform.position.y), dash.transform.rotation);
            timer = 0;
        }
    }
}
