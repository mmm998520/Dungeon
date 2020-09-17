using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.Dungeon
{
    public class CameraManager : MonoBehaviour
    {
        public Transform players;
        Vector3 center;
        float maxX, minX, maxY, minY;
        public float disX, disY;

        void Update()
        {
            cameraMove();
            cameraSize();
        }

        void cameraMove()
        {
            center = Vector3.zero;
            int i;
            for (i = 0; i < players.childCount; i++)
            {
                center += players.GetChild(i).position;
            }
            center /= players.childCount;
            center += Vector3.back * 2;
            transform.position = center;
        }

        void cameraSize()
        {
            maxX = -float.MaxValue;
            minX = float.MaxValue;
            maxY = -float.MaxValue;
            minY = float.MaxValue;
            int i;
            for (i = 0; i < players.childCount; i++)
            {
                Vector3 playerPos = players.GetChild(i).position;
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