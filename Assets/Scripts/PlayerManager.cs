using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    float moveSpeed = 3;

    void Update()
    {
        for(int i = 0; i < Input.touchCount; i++)
        {
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(Input.touches[i].position);
            touchPos.z = 0;
            float targetDis = Vector3.Distance(touchPos, new Vector3(transform.position.x, transform.position.y, 0));
            float moveDis = Time.deltaTime * moveSpeed;
            if (targetDis < 0.5f)
            {
                if(targetDis > moveDis)
                {
                    transform.position += Vector3.Normalize(touchPos - new Vector3(transform.position.x, transform.position.y, 0)) * moveDis;
                }
                else
                {
                    transform.position = new Vector3(touchPos.x, touchPos.y, transform.position.z);
                }
            }
        }
    }
}
