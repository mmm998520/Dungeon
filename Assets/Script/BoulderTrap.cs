using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class BoulderTrap : MonoBehaviour
    {
        public GameObject Boulder;
        void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.layer == 8)
            {
                GameObject boulder;
                if (Random.Range(0, 2) < 1)
                {
                    boulder = Instantiate(Boulder, new Vector3(1, Random.Range(1, 8)), Quaternion.identity);
                }
                else
                {
                    boulder = Instantiate(Boulder, new Vector3(Random.Range(1, 11), 1), Quaternion.identity);
                }
                RaycastHit2D hitup = Physics2D.Raycast(boulder.transform.position * Vector2.one, Vector2.up, 2, 1<<12);
                RaycastHit2D hitright = Physics2D.Raycast(boulder.transform.position * Vector2.one, Vector2.right, 2, 1<<12);
                RaycastHit2D hitdown = Physics2D.Raycast(boulder.transform.position * Vector2.one, Vector2.down, 2, 1<<12);
                RaycastHit2D hitleft = Physics2D.Raycast(boulder.transform.position * Vector2.one, Vector2.left, 2, 1<<12);
                if (hitup)
                {
                    Debug.DrawRay(boulder.transform.position * Vector2.one, Vector2.up);
                    boulder.GetComponent<Boulder>().dir = Vector3.down;
                }
                else if (hitright)
                {
                    Debug.DrawRay(boulder.transform.position * Vector2.one, Vector2.right);
                    boulder.GetComponent<Boulder>().dir = Vector3.left;
                }
                else if (hitdown)
                {
                    Debug.DrawRay(boulder.transform.position * Vector2.one, Vector2.down);
                    boulder.GetComponent<Boulder>().dir = Vector3.up;
                }
                else if (hitleft)
                {
                    Debug.DrawRay(boulder.transform.position * Vector2.one, Vector2.left);
                    boulder.GetComponent<Boulder>().dir = Vector3.right;
                }

                Destroy(gameObject);
            }
        }
    }
}