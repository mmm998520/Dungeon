using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.BoardGameDungeon
{
    public class MazeGen : MonoBehaviour
    {
        public const int row = 12, col = 9;
        public const int Creat_row = row * 2 + 1, Creat_col = col * 2 + 1, fill = 40;
        MazeCreate mazeCreate;
        public Sprite floorSprite, wall_rowSprite, wall_colSprite;

        void Awake()
        {
            mazeCreate = MazeCreate.GetMaze(Creat_row, Creat_col);

            //在基礎迷宮上額外做通道
            int[] filling = new int[fill];
            for (int i = 0; i < fill; i++)
            {
                filling[i] = Random.Range(0, Creat_row * Creat_col);
                for (int j = 0; j < i; j++)
                {
                    if (filling[i] == filling[j])
                    {
                        i--;
                        break;
                    }
                }
                if (mazeCreate.mapList[filling[i] / Creat_col][filling[i] % Creat_col] == (int)MazeCreate.PointType.way || mazeCreate.mapList[filling[i] / Creat_col][filling[i] % Creat_col] == (int)MazeCreate.PointType.startpoint || filling[i] / Creat_col >= Creat_row - 1 || filling[i] / Creat_col <= 0 || filling[i] % Creat_col >= Creat_col - 1 || filling[i] % Creat_col <= 0 || (filling[i] / Creat_col % 2 == 0 && filling[i] % Creat_col % 2 == 0))
                {
                    i--;
                }
            }

            for (int i = 0; i < fill; i++)
            {
                mazeCreate.mapList[filling[i] / Creat_col][filling[i] % Creat_col] = (int)MazeCreate.PointType.way;
            }
            //建立房間清單
            int _i = 0, _j = 0;
            List<List<GameObject>> rooms = new List<List<GameObject>>();
            rooms.Add(new List<GameObject>());
            //建立通道清單
            List<List<int[]>> passways = new List<List<int[]>>();
            //建構方塊
            for (int i = 0; i < Creat_row; i++)
            {
                for (int j = 0; j < Creat_col; j++)
                {
                    //在能行走區域做方塊
                    if ((mazeCreate.mapList[i][j] == (int)MazeCreate.PointType.startpoint || mazeCreate.mapList[i][j] == (int)MazeCreate.PointType.way) && !(i % 2 == 0 && j % 2 == 0))
                    {
                        //起始點標記
                        if ((mazeCreate.mapList[i][j] == (int)MazeCreate.PointType.startpoint) || !(i % 2 == 0 || j % 2 == 0))
                        {
                            GameObject column = (GameObject)Resources.Load("Prefabs/maze");
                            column = MonoBehaviour.Instantiate(column);
                            column.transform.position = new Vector3(i, j, 0);
                            column.transform.localScale *= 2f;
                            column.transform.parent = transform.GetChild(0);
                            column.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = floorSprite;
                            Destroy(column.transform.GetComponent<Collider2D>());
                            //加入房間清單
                            rooms[_i].Add(column);
                            column.name = _i + "," + _j;
                            _j++;
                        }

                        //房間清單換行
                        if (_j >= (Creat_col - 1) / 2)
                        {
                            rooms.Add(new List<GameObject>());
                            _j = 0;
                            _i++;
                        }
                    }
                    //牆壁標記
                    else if (!(i % 2 == 0 && j % 2 == 0))
                    {
                        GameObject column = (GameObject)Resources.Load("Prefabs/maze");
                        column = MonoBehaviour.Instantiate(column);
                        column.transform.parent = transform.GetChild(1);
                        column.transform.position = new Vector3(i, j, 0.1f);
                        //因為column要發生變形，不希望圖片因此扭曲，所以將圖片抽出來再進行變形
                        Transform sprite = column.transform.GetChild(0);
                        sprite.parent = null;
                        sprite.localScale *= 2;
                        if (i % 2 == 0)
                        {
                            column.transform.localScale = new Vector3(0.4f, 2, 1);
                            sprite.GetComponent<SpriteRenderer>().sprite = wall_rowSprite;
                        }
                        else
                        {
                            column.transform.localScale = new Vector3(2, 0.4f, 1);
                            sprite.GetComponent<SpriteRenderer>().sprite = wall_colSprite;
                        }
                        //讓牆壁圖層在地板上面
                        sprite.GetComponent<SpriteRenderer>().sortingOrder = 1;
                        sprite.parent = column.transform;
                        column.name = i + "," + j;
                        //幫牆壁加TAG做區分
                        if (i == 0 || j == 0 || i == Creat_row - 1 || j == Creat_col - 1)
                        {
                            column.tag = "side";
                        }
                        else
                        {
                            column.tag = "wall";
                        }
                        column.layer = 8;
                    }
                }
            }
            //gameManager.GetComponent<GameManager>().rooms = rooms;
        }
    }
}