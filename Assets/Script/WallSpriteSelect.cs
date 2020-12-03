using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class WallSpriteSelect : MonoBehaviour
    {
        void Start()
        {
            int r = Random.Range(0, transform.childCount);
            for(int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(i == r);
            }
        }
    }
}