using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class Navigate : MonoBehaviour
    {
        public int startRoomRow, startRoomCol;
        int[,] canGoRoom;
        public Dictionary<int, int> canGo = new Dictionary<int, int>();
        [SerializeField]
        private AddCanGoByHandList.AddCanGoByHandListUnit[] addCanGoByHandList;

        int startRow, startCol;
        public int[] endRow, endCol;
        const float squareRootOfTwo = 1.414f;
        float[,] g, h, f;
        int[,] lastPos;
        HashSet<int> open, close;

        protected List<int[]> roads = new List<int[]>();
        public int[] nextPos;
        public float rotateSpeed = 200;

        #region//設定可通行路徑，每次開新房都要刷新
        public void arriveNewRoom(int roomRow, int roomCol)
        {
            setCanGoRoom();
            bool[,] roomPasswayDatas = GameManager.mazeCreater.created;
            for (int t = 0; t < 50; t++)
            {
                for(int i =0;i< GameManager.mazeCreater.roomCountRowNum; i++)
                {
                    for (int j = 0; j < GameManager.mazeCreater.roomCountRowNum; j++)
                    {
                        addCanGoRoom(i, j);
                        if (!GameManager.mazeCreater.created[i, j])
                        {
                            canGoRoom[i, j] = -1;
                        }
                    }
                }
            }
            setCanGo();
        }
        #region//定義可通行房間
        void setCanGoRoom()
        {
            canGoRoom = new int[GameManager.mazeCreater.roomCountRowNum, GameManager.mazeCreater.roomCountColNum];
            int i, j;
            for(i = 0; i <= canGoRoom.GetUpperBound(0); i++)
            {
                for(j = 0; j <= canGoRoom.GetUpperBound(1); j++)
                {
                    canGoRoom[i, j] = -1;
                    if (i == startRoomRow && j == startRoomCol)
                    {
                        canGoRoom[i, j] = GameManager.mazeCreater.roomPasswayDatas[i, j];
                    }
                }
            }
        }
        bool addCanGoRoom(int roomRow,int roomCol)
        {
            if (Mathf.Abs(roomRow - startRoomRow) < 1 && Mathf.Abs(roomCol - startRoomCol) < 1)
            {
                int[,] roomPasswayDatas = GameManager.mazeCreater.roomPasswayDatas;
                if (canGoRoom[roomRow, roomCol] == -1)
                {
                    if (roomRow - 1 >= 0)
                    {
                        //看上方有沒有通道
                        if (roomPasswayDatas[roomRow, roomCol] == 0 || roomPasswayDatas[roomRow, roomCol] == 1 || roomPasswayDatas[roomRow, roomCol] == 3 || roomPasswayDatas[roomRow, roomCol] == 4 || roomPasswayDatas[roomRow, roomCol] == 6)
                        {
                            //看上方房間有沒有下通道
                            if (canGoRoom[roomRow - 1, roomCol] == 0 || canGoRoom[roomRow - 1, roomCol] == 1 || canGoRoom[roomRow - 1, roomCol] == 2 || canGoRoom[roomRow - 1, roomCol] == 3 || canGoRoom[roomRow - 1, roomCol] == 6)
                            {
                                canGoRoom[roomRow, roomCol] = roomPasswayDatas[roomRow, roomCol];
                                return true;
                            }
                        }
                    }
                    if (roomCol + 1 <= roomPasswayDatas.GetUpperBound(1))
                    {
                        //看右方有沒有通道
                        if (roomPasswayDatas[roomRow, roomCol] == 0 || roomPasswayDatas[roomRow, roomCol] == 1 || roomPasswayDatas[roomRow, roomCol] == 2 || roomPasswayDatas[roomRow, roomCol] == 4 || roomPasswayDatas[roomRow, roomCol] == 5)
                        {
                            //看右方房間有沒有左通道
                            if (canGoRoom[roomRow, roomCol + 1] == 0 || canGoRoom[roomRow, roomCol + 1] == 2 || canGoRoom[roomRow, roomCol + 1] == 3 || canGoRoom[roomRow, roomCol + 1] == 4 || canGoRoom[roomRow, roomCol + 1] == 5)
                            {
                                canGoRoom[roomRow, roomCol] = roomPasswayDatas[roomRow, roomCol];
                                return true;
                            }
                        }
                    }
                    if (roomRow + 1 <= roomPasswayDatas.GetUpperBound(1))
                    {
                        //看下方有沒有通道
                        if (roomPasswayDatas[roomRow, roomCol] == 0 || roomPasswayDatas[roomRow, roomCol] == 1 || roomPasswayDatas[roomRow, roomCol] == 2 || roomPasswayDatas[roomRow, roomCol] == 3 || roomPasswayDatas[roomRow, roomCol] == 6)
                        {
                            //看下方房間有沒有上通道
                            if (canGoRoom[roomRow + 1, roomCol] == 0 || canGoRoom[roomRow + 1, roomCol] == 1 || canGoRoom[roomRow + 1, roomCol] == 3 || canGoRoom[roomRow + 1, roomCol] == 4 || canGoRoom[roomRow + 1, roomCol] == 6)
                            {
                                canGoRoom[roomRow, roomCol] = roomPasswayDatas[roomRow, roomCol];
                                return true;
                            }
                        }
                    }
                    if (roomCol - 1 >= 0)
                    {
                        //看左方有沒有通道
                        if (roomPasswayDatas[roomRow, roomCol] == 0 || roomPasswayDatas[roomRow, roomCol] == 2 || roomPasswayDatas[roomRow, roomCol] == 3 || roomPasswayDatas[roomRow, roomCol] == 4 || roomPasswayDatas[roomRow, roomCol] == 5)
                        {
                            //看左方房間有沒有右通道
                            if (canGoRoom[roomRow, roomCol - 1] == 0 || canGoRoom[roomRow, roomCol - 1] == 1 || canGoRoom[roomRow, roomCol - 1] == 2 || canGoRoom[roomRow, roomCol - 1] == 4 || canGoRoom[roomRow, roomCol - 1] == 5)
                            {
                                canGoRoom[roomRow, roomCol] = roomPasswayDatas[roomRow, roomCol];
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
        #endregion
        void setCanGo()
        {
            canGo = new Dictionary<int, int>();
            int row, col;
            int i, j, k, l;
            for(i = 0; i < GameManager.mazeCreater.roomCountRowNum; i++)
            {
                for (j = 0; j < GameManager.mazeCreater.roomCountColNum; j++)
                {
                    if (canGoRoom[i, j] != -1)
                    {
                        for (k = 0; k < GameManager.mazeCreater.objectCountRowNum; k++)
                        {
                            row = i * GameManager.mazeCreater.objectCountRowNum + k;
                            for (l = 0; l < GameManager.mazeCreater.objectCountColNum; l++)
                            {
                                col = j * GameManager.mazeCreater.objectCountColNum + l;
                                if (GameManager.mazeCreater.mazeDatas[row, col] != "wall" && !GameManager.mazeCreater.mazeDatas[row, col].Contains("Door") && !GameManager.mazeCreater.mazeDatas[row, col].Contains("上") && !GameManager.mazeCreater.mazeDatas[row, col].Contains("下") && !GameManager.mazeCreater.mazeDatas[row, col].Contains("左") && !GameManager.mazeCreater.mazeDatas[row, col].Contains("右"))
                                {
                                    canGo.Add(canGo.Count, row * MazeCreater.totalCol + col);
                                    print(GameManager.mazeCreater.mazeDatas[row, col]);
                                }
                                else
                                {
                                    print(GameManager.mazeCreater.mazeDatas[row, col]);
                                }
                            }
                        }
                    }
                }
            }
            List<int[]> a = new List<int[]>();
            foreach(int v in canGo.Values)
            {
                a.Add(new int[] { v / MazeCreater.totalCol, v % MazeCreater.totalCol });
            }
        }
        #endregion

        #region//手動添加可通行路徑
        protected void addCanGoByHand()
        {
            for(int i = 0; i < addCanGoByHandList.Length; i++)
            {
                for(int j = addCanGoByHandList[i].MinRow * MazeCreater.totalCol + addCanGoByHandList[i].MinCol; j <= addCanGoByHandList[i].MaxRow * MazeCreater.totalCol + addCanGoByHandList[i].MaxCol; j++)
                {
                    canGo.Add(canGo.Count, j);
                }
            }
        }
        #endregion

        bool findingRoad;
        #region//尋路
        public void findRoad()
        {
            if (!findingRoad)
            {
                findingRoad = true;
                #region//找路
                #region//設定基本數值
                startRow = Mathf.RoundToInt(transform.position.x);
                startCol = Mathf.RoundToInt(transform.position.y);
                g = new float[MazeCreater.totalRow, MazeCreater.totalRow];
                h = new float[MazeCreater.totalRow, MazeCreater.totalRow];
                f = new float[MazeCreater.totalRow, MazeCreater.totalRow];
                lastPos = new int[MazeCreater.totalRow, MazeCreater.totalRow];
                // key : pos，value : 下個該去的點
                open = new HashSet<int>();
                close = new HashSet<int>();
                int i, j;
                for (i = 0; i < MazeCreater.totalRow; i++)
                {
                    for (j = 0; j < MazeCreater.totalCol; j++)
                    {
                        g[i, j] = 99999;
                        h[i, j] = 99999;
                        f[i, j] = 999999;
                        if (!canGo.ContainsValue(i * MazeCreater.totalCol + j))
                        {
                            close.Add(i * MazeCreater.totalCol + j);
                        }
                    }
                }
                g[startRow, startCol] = 0;
                open.Add(startRow * MazeCreater.totalCol + startCol);
                lastPos[startRow, startCol] = startRow * MazeCreater.totalCol + startCol;
                #endregion

                do
                {
                    #region//偵測是否到出口了
                    for (i = 0; i < endRow.Length; i++)
                    {
                        if (open.Contains(endRow[i] * MazeCreater.totalCol + endCol[i]))
                        {
                            int currentRow = endRow[i], currentCol = endCol[i], lastRow, lastCol;
                            do
                            {
                                lastRow = lastPos[currentRow, currentCol] / MazeCreater.totalCol;
                                lastCol = lastPos[currentRow, currentCol] % MazeCreater.totalCol;
                                roads.Add(new int[] { lastRow, lastCol });
                                currentRow = lastRow;
                                currentCol = lastCol;
                            } while (!(currentRow == startRow && currentCol == startCol));
                            findingRoad = false;
                            return;
                        }
                    }
                    #endregion

                    #region//從open中找f最小的來計算
                    float min = float.MaxValue;
                    int selected = -1;
                    foreach (int pos in open)
                    {
                        if (min > f[pos / MazeCreater.totalCol, pos % MazeCreater.totalCol])
                        {
                            min = f[pos / MazeCreater.totalCol, pos % MazeCreater.totalCol];
                            selected = pos;
                        }
                    }

                    #endregion

                    CheckPassway(selected / MazeCreater.totalCol, selected % MazeCreater.totalCol);

                    if (open.Contains(selected))
                    {
                        open.Remove(selected);
                        close.Add(selected);
                    }
                    else
                    {
                        Debug.LogError("open空了");
                        findingRoad = false;
                        return;
                    }
                } while (true);
                #endregion
            }
        }

        void CheckPassway(int row, int col)
        {
            bool up = canGo.ContainsValue((row - 1) * MazeCreater.totalCol + (col));
            bool down = canGo.ContainsValue((row + 1) * MazeCreater.totalCol + (col));
            bool right = canGo.ContainsValue((row) * MazeCreater.totalCol + (col + 1));
            bool left = canGo.ContainsValue((row) * MazeCreater.totalCol + (col - 1));
            bool upRight = canGo.ContainsValue((row - 1) * MazeCreater.totalCol + (col + 1));
            bool upLeft = canGo.ContainsValue((row - 1) * MazeCreater.totalCol + (col - 1));
            bool downRight = canGo.ContainsValue((row + 1) * MazeCreater.totalCol + (col + 1));
            bool downLeft = canGo.ContainsValue((row + 1) * MazeCreater.totalCol + (col - 1));

            if (up)
            {
                countPassway(row, col, row - 1, col, 1);
                if (right && upRight)
                {
                    countPassway(row, col, row - 1, col + 1, squareRootOfTwo);
                }
                if (left && upLeft)
                {
                    countPassway(row, col, row - 1, col - 1, squareRootOfTwo);
                }
            }
            if (down)
            {
                countPassway(row, col, row + 1, col, 1);
                if (right && downRight)
                {
                    countPassway(row, col, row + 1, col + 1, squareRootOfTwo);
                }
                if (left && downLeft)
                {
                    countPassway(row, col, row + 1, col - 1, squareRootOfTwo);
                }
            }
            if (right)
            {
                countPassway(row, col, row, col + 1, 1);
            }
            if (left)
            {
                countPassway(row, col, row, col - 1, 1);
            }
        }

        void countPassway(int currentRow, int currentCol, int nextRow, int nextCol, float dis)
        {
            if(!close.Contains(nextRow * MazeCreater.totalCol + nextCol))
            {
                if (g[nextRow, nextCol] > g[currentRow, currentCol] + dis)
                {
                    g[nextRow, nextCol] = g[currentRow, currentCol] + dis;
                    if (!open.Contains(nextRow * MazeCreater.totalCol + nextCol))
                    {
                        open.Add(nextRow * MazeCreater.totalCol + nextCol);
                        h[nextRow, nextCol] = countH(currentRow, currentCol);
                    }
                    lastPos[nextRow, nextCol] = currentRow * MazeCreater.totalCol + currentCol;
                    f[nextRow, nextCol] = g[nextRow, nextCol] + h[nextRow, nextCol];
                }
            }
        }

        float countH(int currentRow, int currentCol)
        {
            int absRow, absCol;
            float hypotenuse, dis;
            float min = float.MaxValue;
            for(int i = 0; i < endRow.Length; i++)
            {
                absRow = Mathf.Abs(currentRow - endRow[i]);
                absCol = Mathf.Abs(currentCol - endCol[i]);
                hypotenuse = Mathf.Min(absRow, absCol) * squareRootOfTwo;
                dis = Mathf.Abs(absRow - absCol) + hypotenuse;
                if (min > dis)
                {
                    min = dis;
                }
            }

            return min;
        }
        #endregion

        /*protected void changeDirection()
        {
            Vector3 myPos = transform.position;
            if (nextPos != null &&nextPos.Length>1)
            {
                Vector3 endPos = new Vector3(nextPos[0], nextPos[1], 0);
                // 讓z軸沒有前後誤差值，以免面向錯方向
                endPos.z = myPos.z;
                //計算將要朝向的方向，定義其為物件的x軸方向
                Vector3 vectorToTarget = endPos - myPos;
                //將物件的x軸延z軸旋轉90度尋找y軸
                Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, 90) * vectorToTarget;
                // LookRotation需要給予向前軸與向上軸，物件z軸將指向向前軸，y軸指向向上軸
                //所以只要讓物件z軸面向世界座標z軸，y軸指向面向方向轉90度即可讓物件指向x軸
                Quaternion targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: rotatedVectorToTarget);
                // 讓物件朝指定方向轉指定角度(rotateSpeed * Time.deltaTime讓他變定速旋轉)
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
            }
        }*/
    }

    /// <summary> 儲存資料用 </summary>
    [System.Serializable]
    public class AddCanGoByHandList
    {
        /// <summary> 儲存資料用 </summary>
        [System.Serializable]
        public struct AddCanGoByHandListUnit
        {
            public int MinRow, MinCol;
            public int MaxRow, MaxCol;
        }
    }
}