using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class ActiveSetting : MonoBehaviour
    {
        void Start()
        {

        }

        void Update()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(Vector3.Distance(Camera.main.transform.position + new Vector3(0, 0, 10), transform.GetChild(i).position) < 13);
            }
        }
    }
}