using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.Dungeon
{
    public class CameraManager : MonoBehaviour
    {
        public Transform players;
        Vector3 center;

        void Update()
        {
            center = Vector3.zero;
            for (int i = 0;i< players.childCount; i++)
            {
                center += players.GetChild(i).position;
            }
            center /= players.childCount;
            center += Vector3.back * 2;
            transform.position = center;
        }
    }
}