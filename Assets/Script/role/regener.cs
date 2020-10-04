using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class regener : MonoBehaviour
    {
        public Transform master;
        void Update()
        {
            transform.position = master.position * Vector2.one;
            Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, 7, 1 << 8);
            for (int i = 0; i < collider2Ds.Length; i++)
            {
                collider2Ds[i].GetComponent<PlayerManager>().HP+=Time.deltaTime*3;
            }
        }
    }
}