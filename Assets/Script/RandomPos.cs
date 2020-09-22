using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class RandomPos : MonoBehaviour
    {
        void Start()
        {
            transform.position = new Vector3(Random.Range(1, 11), Random.Range(1, 8));
        }
    }
}