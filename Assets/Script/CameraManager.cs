using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class CameraManager : MonoBehaviour
    {
        Vector3 center;
        public float maxX, minX, maxY, minY;
        public float disX, disY;

        void Update()
        {
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
                maxX = Mathf.Clamp(maxX, 10, float.MaxValue);
                minX = Mathf.Clamp(minX, 0, MapCreater.totalRow[MapCreater.level] - 10);
                maxY = Mathf.Clamp(maxY, 7, float.MaxValue);
                minY = Mathf.Clamp(minY, 0, MapCreater.totalCol[MapCreater.level] -7);
            }
            cameraMove();
            //cameraSize();
        }

        void cameraMove()
        {
            center = new Vector3((maxX + minX) / 2, (maxY + minY) / 2, -10);
            transform.position = center;
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