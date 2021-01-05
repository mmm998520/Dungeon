using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class Seller : MonoBehaviour
    {
        List<PlayerManager> inRange = new List<PlayerManager>();
        new Rigidbody2D rigidbody;
        void Start()
        {
            rigidbody = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            rigidbody.velocity = Vector3.zero;
        }
    }
}