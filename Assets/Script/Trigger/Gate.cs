using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class Gate : Trigger
    {
        public Transform theOther;
        void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.GetComponent<PlayerManager>())
            {
                collider.transform.position = new Vector3(theOther.position.x, theOther.position.y, collider.transform.position.z);
                triggerGen.rePos(transform);
                triggerGen.rePos(theOther);
            }
        }
    }
}