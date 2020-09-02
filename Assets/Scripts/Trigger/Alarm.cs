using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.BoardGameDungeon
{
    public class Alarm : MonoBehaviour
    {
        GameObject monster;
        void Start()
        {

        }

        void Update()
        {

        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.GetComponent<PlayerManager>())
            {
                //位置像素化
                int row, col;
                for (row = 0; row < MazeGen.row; row++)
                {
                    if (Mathf.Abs(transform.position.x - (row * 2 + 1)) <= 1)
                    {
                        break;
                    }
                }
                for (col = 0; col < MazeGen.Creat_col; col++)
                {
                    if (Mathf.Abs(transform.position.y - (col * 2 + 1)) <= 1)
                    {
                        break;
                    }
                }
                if (Random.Range(0, 2) < 2)
                {
                    Instantiate(monster, new Vector3((row + 1) * 2 + 1, col * 2 + 1, transform.position.z), Quaternion.identity);
                    Instantiate(monster, new Vector3((row - 1) * 2 + 1, col * 2 + 1, transform.position.z), Quaternion.identity);
                }
                else
                {
                    Instantiate(monster, new Vector3(row * 2 + 1, (col + 1) * 2 + 1, transform.position.z), Quaternion.identity);
                    Instantiate(monster, new Vector3(row * 2 + 1, (col - 1) * 2 + 1, transform.position.z), Quaternion.identity);
                }
            }
        }
    }
}