using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScaler : MonoBehaviour
{
    float timer, timerStoper;
    float maxX;
    void Start()
    {
        maxX = transform.localScale.x;
        Debug.LogError(maxX);
        transform.localScale = new Vector3(0, transform.localScale.y, transform.localScale.z);
    }

    void Update()
    {
        if(transform.localScale.x < maxX - 0.01f)
        {
            transform.localScale = new Vector3(Mathf.Lerp(transform.localScale.x, maxX, Time.deltaTime * 10), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(maxX, transform.localScale.y, transform.localScale.z);
            Destroy(this);
        }
    }
}
