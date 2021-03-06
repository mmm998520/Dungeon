﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoter : MonoBehaviour
{
    public float speed, timer, timerStoper, destoryTime;
    public WaitForSeconds destoryTimer;

    void Start()
    {
        destoryTimer = new WaitForSeconds(destoryTime);
    }
    void Update()
    {
        transform.Translate(Vector3.right * Time.deltaTime * speed);
        if ((timer += Time.deltaTime) > timerStoper)
        {
            StartCoroutine("destroy");
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == 9 || collider.gameObject.layer == 10 || collider.gameObject.layer == 11 || collider.gameObject.layer == 12)
        {
            StartCoroutine("destroy");
        }
    }

    IEnumerator destroy()
    {
        speed = 0;
        yield return destoryTimer;
        Destroy(gameObject);
    }
}
