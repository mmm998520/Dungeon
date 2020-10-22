using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace com.DungeonPad
{
    [SerializeField]
    public class DataLoader : MonoBehaviour
    {
        public TextAsset[] files;
        /// <summary> 通道型態list->難度list->功能list->關卡設計list ->關卡內容array </summary>
        public static Dictionary<int, string[,]>[,,,] AllRoomDatas;
        /// <summary> 層數 </summary>
        int layers = 2;
        /// <summary> 通道型態總數，0 ┼，1 ├，2 ┬，3 ┤，4 ┴，5 ─，6 │ </summary>
        int passwayTypeNum = 7;
        /// <summary> 難度分級數 </summary>
        int level = 3;
        /// <summary> 功能種類數 </summary>
        int functionTypeNum = 3;
        public void LoadAll()
        {
            AllRoomDatas = new Dictionary<int, string[,]>[layers, passwayTypeNum, level, functionTypeNum];
            int i, j, k, l;
            for (i = 0; i < layers; i++)
            {
                for (j = 0; j < passwayTypeNum; j++)
                {
                    for (k = 0; k < level; k++)
                    {
                        for (l = 0; l < functionTypeNum; l++)
                        {
                            AllRoomDatas[i, j, k, l] = new Dictionary<int, string[,]>();
                        }
                    }
                }
            }
            for (i = 0; i < files.Length; i++)
            {
                string[] sArray = files[i].name.Split('_');
                int[] num = new int[sArray.Length];
                for (j = 0; j < sArray.Length; j++)
                {
                    num[j] = int.Parse(sArray[j]);
                }
                k = 0;
                AllRoomDatas[num[k++], num[k++], num[k++], num[k++]].Add(num[k++], LoadData(files[i]));
            }
            files = null;
        }

        public static string[,] LoadData(TextAsset file)
        {
            //讀取csv二進位制檔案
            //讀取每一行的內容
            string[] lineArray = file.text.Split("\r"[0]);
            //建立二維陣列
            int i, j;
            string[,] dataArray = new string[lineArray.Length - 1, lineArray[0].Split(',').Length];
            //把csv中的資料儲存在二位陣列中
            for (i = 0; i < lineArray.Length - 1; i++)
            {
                if (lineArray[i].Split('\n').Length != 1)
                {
                    lineArray[i] = lineArray[i].Split('\n')[1];
                }
                string[] data = lineArray[i].Split(',');
                for (j = 0; j < data.Length; j++)
                {
                    dataArray[i, j] = data[j];
                }
            }
            return dataArray;
        }

        public static string[,] Rotate(string[,] origin)
        {
            string[,] rotated = new string[origin.GetUpperBound(0) + 1, origin.GetUpperBound(0) + 1];
            int newColumn, newRow = 0;
            for (int i = 0; i < origin.GetUpperBound(0) + 1; i++)
            {
                newColumn = 0;
                for (int j = origin.GetUpperBound(1); j >= 0; j--)
                {
                    rotated[newRow, newColumn] = origin[j, i];
                    newColumn++;
                }
                newRow++;
            }
            return rotated;
        }

        public static string[,] reverserArray(string[,] origin)
        {
            string[,] newArray = new string[origin.GetUpperBound(0) + 1, origin.GetUpperBound(1) + 1];
            int i, j;
            for (i = 0; i < origin.GetUpperBound(0) + 1; i++)
            {
                for (j = 0; j < origin.GetUpperBound(1) + 1; j++)
                {
                    newArray[i, j] = origin[origin.GetUpperBound(0) - i, j];
                }
            }
            return newArray;
        }
    }
}