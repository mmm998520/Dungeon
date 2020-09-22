using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class Boulder : MonoBehaviour
    {
        public float ATK;

        public Vector3 dir = Vector3.zero;
        void Update()
        {
            transform.Translate(dir * Time.deltaTime);
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.layer == 8)
            {
                collider.GetComponent<PlayerManager>().HP -= ATK;
            }
        }
    }
}