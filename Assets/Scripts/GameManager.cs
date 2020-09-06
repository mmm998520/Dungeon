using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.BoardGameDungeon
{
    public class GameManager : MonoBehaviour
    {
        /// <summary> 共通父物件，提供其他component使用 </summary>
        public static Transform Floors, Walls, Players;

        void Awake()
        {
            Floors = GameObject.Find("Floors").transform;
            Walls = GameObject.Find("Walls").transform;
            Players = GameObject.Find("Players").transform;

            for(int currentRow = 0; currentRow < MazeGen.row; currentRow++)
            {
                for(int currentCol = 0; currentCol < MazeGen.col; currentCol++)
                {
                    for (int _i = -1; _i < 2; _i++)
                    {
                        for (int _j = -1; _j < 2; _j++)
                        {
                            if (_i != 0 || _j != 0)
                            {
                                int nextRow = currentRow + _i, nextCol = currentCol + _j;
                                if (nextRow < MazeGen.row && nextRow >= 0 && nextCol < MazeGen.col && nextCol >= 0)
                                {
                                    MonsterManager.cantMoves.Add(new int[] { currentRow, currentCol, nextRow, nextCol });
                                }
                            }
                        }
                    }
                }
            }

            StartCoroutine("scanMoves");
        }

        private void Update()
        {
            if (Players.childCount == 0)
            {
                SceneManager.LoadScene("Game2");
            }
        }

        WaitForSeconds halfSeconds = new WaitForSeconds(0.5f);
        IEnumerator scanMoves()
        {
            yield return halfSeconds;
            for(int i = MonsterManager.cantMoves.Count - 1; i >= 0; i--)
            {
                int[] cantMove = MonsterManager.cantMoves[i];
                //向指定pos打出兩道射線(有間距)判定打到甚麼來決定能不能通過
                Vector3 currentPos = new Vector3(cantMove[0] * 2 + 1, cantMove[1] * 2 + 1);
                Vector3 Dir = new Vector3(cantMove[2] * 2 + 1, cantMove[3] * 2 + 1) - currentPos;
                Vector3 tempDir = Quaternion.Euler(0, 0, 90) * Dir.normalized / 2;
                //layerMask，讓射線只能打牆壁
                RaycastHit2D hitWall1 = Physics2D.Raycast(currentPos + tempDir, Dir, Dir.magnitude, 1 << 8);
                RaycastHit2D hitWall2 = Physics2D.Raycast(currentPos - tempDir, Dir, Dir.magnitude, 1 << 8);
                //如果會撞到則不選擇
                if (!(hitWall1 || hitWall2))
                {
                    MonsterManager.cantMoves.Remove(cantMove);
                    Debug.DrawRay(currentPos, Dir, Color.green);
                }
            }
            print(MonsterManager.cantMoves.Count);
            yield return halfSeconds;
            for (int currentRow = 0; currentRow < MazeGen.row; currentRow++)
            {
                for (int currentCol = 0; currentCol < MazeGen.row; currentCol++)
                {
                    for (int i = -1; i < 2; i++)
                    {
                        for (int j = -1; j < 2; j++)
                        {
                            int nextRow = currentRow + i, nextCol = currentCol + j;
                            if(!MonsterManager.cantMoves.Any(p => p.SequenceEqual(new int[] { currentRow, currentCol, nextRow, nextCol })))
                            {
                                Vector3 currentPos = new Vector3(currentRow * 2 + 1, currentCol * 2 + 1);
                                Vector3 Dir = new Vector3(nextRow * 2 + 1, nextCol * 2 + 1) - currentPos;
                                RaycastHit2D hit = Physics2D.Raycast(currentPos, Dir, Dir.magnitude, 1 << 1);
                                if (hit)
                                {
                                    if (hit.collider.tag == "monster" && !MonsterManager.meetMonsterMoves.Any(p => p.SequenceEqual(new int[] { currentRow, currentCol, nextRow, nextCol })))
                                    {
                                        MonsterManager.meetMonsterMoves.Add(new int[] { currentRow, currentCol, nextRow, nextCol });
                                    }
                                    if (hit.collider.tag == "player" && !MonsterManager.meetPlayerMoves.Any(p => p.SequenceEqual(new int[] { currentRow, currentCol, nextRow, nextCol })))
                                    {
                                        MonsterManager.meetPlayerMoves.Add(new int[] { currentRow, currentCol, nextRow, nextCol });
                                    }
                                }
                                else
                                {
                                    if (MonsterManager.meetMonsterMoves.Any(p => p.SequenceEqual(new int[] { currentRow, currentCol, nextRow, nextCol })))
                                    {
                                        MonsterManager.meetMonsterMoves.RemoveAll(p => p.SequenceEqual(new int[] { currentRow, currentCol, nextRow, nextCol }));
                                    }
                                    if (MonsterManager.meetPlayerMoves.Any(p => p.SequenceEqual(new int[] { currentRow, currentCol, nextRow, nextCol })))
                                    {
                                        MonsterManager.meetPlayerMoves.RemoveAll(p => p.SequenceEqual(new int[] { currentRow, currentCol, nextRow, nextCol }));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            print("你有跑完對吧??");
            StartCoroutine("scanMoves");
        }
    }
}