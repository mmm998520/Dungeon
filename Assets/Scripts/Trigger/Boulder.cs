using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.BoardGameDungeon
{
    /// <summary> 從觸發器十字方向的地圖邊緣生成滾石滾向觸發器 </summary>
    public class Boulder : TriggerManager
    {
        public GameObject stone;
        void Start()
        {

        }

        void Update()
        {

        }

        void OnTriggerEnter2D(Collider2D collision)
        {
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
            int r = Random.Range(0, 4);
            GameObject stone;
            if (r >= 3)
            {
                stone = Instantiate(this.stone, new Vector3(row * 2 + 1, MazeGen.col * 2 + 1), Quaternion.identity);
            }
            else if (r >= 2)
            {
                stone = Instantiate(this.stone, new Vector3(row * 2 + 1, 0), Quaternion.identity);
            }
            else if (r >= 1)
            {
                stone = Instantiate(this.stone, new Vector3(row * 2 + 1, MazeGen.col * 2 + 1), Quaternion.identity);
            }
            else
            {
                stone = Instantiate(this.stone, new Vector3(row * 2 + 1, MazeGen.col * 2 + 1), Quaternion.identity);
            }
            stone.GetComponent<>
        }
    }
}