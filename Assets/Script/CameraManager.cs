﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class CameraManager : MonoBehaviour
    {
        public static Vector3 center;
        public float maxX, minX, maxY, minY;
        public float disX, disY;
        public Transform Center;

        void Update()
        {
            transform.GetChild(0).localPosition = Vector3.zero;
            maxX = -float.MaxValue;
            minX = float.MaxValue;
            maxY = -float.MaxValue;
            minY = float.MaxValue;
            int i;
            for (i = 0; i < GameManager.players.childCount; i++)
            {
                Vector3 playerPos = GameManager.players.GetChild(i).position;
                if (maxX < playerPos.x)
                {
                    maxX = playerPos.x;
                }
                if (minX > playerPos.x)
                {
                    minX = playerPos.x;
                }
                if (maxY < playerPos.y)
                {
                    maxY = playerPos.y;
                }
                if (minY > playerPos.y)
                {
                    minY = playerPos.y;
                }
            }
            center = new Vector3((maxX + minX) / 2, (maxY + minY) / 2, -10);
            Center.position = center + Vector3.forward * 10;
            cameraMove();
            //cameraSize();
        }

        void cameraMove()
        {
            GetComponent<Rigidbody2D>().velocity = (center - transform.position)*3;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //暫時不用調攝影機size
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        void cameraSize()
        {
            float size;
            disX = maxX - minX;
            disY = maxY - minY;
            if (disX / 4 > disY / 3)
            {
                size = disX / 4 * 3 + 3;

            }
            else
            {
                size = disY + 3;
            }
            size = Mathf.Clamp(size, 4, 8);
            Camera.main.orthographicSize = size;
            Camera.main.transform.GetChild(0).localScale = new Vector3(size, size, 1);
        }
    }
}