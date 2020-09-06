using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.BoardGameDungeon
{
    public class Stone : MonoBehaviour
    {
        public Vector3 dir;
        float speed = 3;
        float ATK = 5;
        
        public PlayerManager user;

        void Start()
        {
            int startRow, startCol;
            for (startRow = 0; startRow < MazeGen.row; startRow++)
            {
                if (Mathf.Abs(transform.position.x - (startRow * 2 + 1)) <= 1)
                {
                    break;
                }
            }
            for (startCol = 0; startCol < MazeGen.Creat_col; startCol++)
            {
                if (Mathf.Abs(transform.position.y - (startCol * 2 + 1)) <= 1)
                {
                    break;
                }
            }
            int tempRow = 0, tempCol = 0;
            int row = 0, col = 0;
            for(int i = 0; i < 3; i++)
            {
                if (i == 1)
                {
                    if(Mathf.Abs(dir.x) > 0.5f)
                    {
                        tempCol = 1;
                    }
                    else
                    {
                        tempRow = -1;
                    }
                }
                else if(i == 2)
                {
                    if (Mathf.Abs(dir.x) > 0.5f)
                    {
                        tempCol = -1;
                    }
                    else
                    {
                        tempRow = -1;
                    }
                }
                for(int j = 0; i < 20; j++)
                {
                    if (startRow + row + tempRow >= 0 && startRow + row + tempRow < MazeGen.row && startCol + col + tempCol >= 0 && startCol + col + tempCol < MazeGen.col)
                    {
                        MonsterManager.stoneway.Add(new int[] { startRow + row + tempRow, startCol + col + tempCol });
                    }
                    if (dir.x > 0.5f)
                    {
                        row++;
                    }
                    else if (dir.x < 0.5f)
                    {
                        row--;
                    }
                    else if (dir.y > 0.5f)
                    {
                        col++;
                    }
                    else if (dir.y < 0.5f)
                    {
                        col--;
                    }
                }
            }
        }

        void Update()
        {
            transform.Translate(dir * Time.deltaTime * speed);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, transform.lossyScale.x / 2, 1<<8);
            Vector3 tempDir = Quaternion.Euler(0, 0, 90) * dir.normalized;
            RaycastHit2D hit1 = Physics2D.Raycast(transform.position + tempDir, dir, transform.lossyScale.x / 10, 1<<8);
            RaycastHit2D hit2 = Physics2D.Raycast(transform.position - tempDir, dir, transform.lossyScale.x / 10, 1<<8);
            if (hit)
            {
                if (hit.collider.tag == "side")
                {
                    MonsterManager.stoneway.Clear();
                    Destroy(gameObject);
                }
                else if (hit.collider.tag == "wall")
                {
                    Destroy(hit.collider.gameObject);
                }
            }
            if (hit1)
            {
                if (hit1.collider.tag == "wall")
                {
                    Destroy(hit1.collider.gameObject);
                }
            }
            if (hit2)
            {
                if (hit2.collider.tag == "wall")
                {
                    Destroy(hit2.collider.gameObject);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.GetComponent<ValueSet>())
            {
                collider.GetComponent<ValueSet>().Hurt += ATK;
            }
            MonsterManager.addHurtMe(collider, user);
        }
    }
}
