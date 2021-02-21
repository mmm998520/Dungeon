using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class CrystalScatteringLight : MonoBehaviour
    {
        public static bool Large;
        public Transform rayPos1, rayPos2, mask;
        float distance;
        float maxDistance = 10, minDistance = 0.5f;
        void Start()
        {
            if (Large)
            {
                GetComponent<Animator>().SetTrigger("Large");
            }
        }

        void Update()
        {

        }

        void setLight()
        {
            RaycastHit2D hit1 = Physics2D.Raycast(rayPos1.position, transform.right, 1000, 1 << 9 | 1 << 12);
            RaycastHit2D hit2 = Physics2D.Raycast(rayPos2.position, transform.right, 1000, 1 << 9 | 1 << 12);
            Collider2D hited;
            if (hit1.distance < hit2.distance)
            {
                distance = hit1.distance;
                hited = hit1.collider;
            }
            else
            {
                distance = hit2.distance;
                hited = hit2.collider;
            }
            if (distance < maxDistance)
            {
                if(distance > minDistance)
                {
                    mask.localScale = new Vector3(maxDistance - distance, 1, 1);
                }
                else
                {
                    mask.localScale = new Vector3(maxDistance - minDistance, 1, 1);
                }
                setHited(hited);
            }
            else
            {
                mask.localScale = new Vector3(0, 1, 1);
            }
        }

        void setHited(Collider2D collider2D)
        {
            if (collider2D.GetComponent<MonsterManager>())
            {
                collider2D.GetComponent<MonsterManager>().HP -= 1;
                if (collider2D.GetComponent<MonsterManager>().HP <= 0)
                {
                    collider2D.GetComponent<MonsterManager>().beforeDied();
                }
            }
        }
    }
}