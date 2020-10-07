using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class Bubble : MonoBehaviour
    {
        public Vector2 v, a;
        void Start()
        {
            v = Quaternion.Euler(0, 0, Random.Range(0, 360)) * Vector2.right * 0.5f;
            a = Quaternion.Euler(0, 0, Random.Range(0, 360)) * Vector2.right * 0.5f;
        }

        void Update()
        {
            a = Quaternion.Euler(0, 0, Random.Range(-10, 10)) * a;
            v += a * Time.deltaTime;
            transform.Translate(v * Time.deltaTime);
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.layer >= 8 && collider.transform != transform.parent)
            {
                if (collider.GetComponent<PlayerManager>())
                {
                    Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, 30, 1 << 9);
                    for(int i = 0; i < collider2Ds.Length; i++)
                    {
                        if (collider2Ds[i].GetComponent<Slime>())
                        {
                            collider2Ds[i].GetComponent<Slime>().BurstBubbler = collider.transform;
                        }
                    }
                }
                Destroy(gameObject);
            }
        }
    }
}