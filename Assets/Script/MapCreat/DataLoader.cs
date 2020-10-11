using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataLoader : MonoBehaviour
{
    TextAsset[] files;
    /// <summary> 通道型態list->難度list->功能list->關卡設計list ->關卡內容array </summary>
    Dictionary<int,string[,]>[,,,] AllData;
    /// <summary> 層數 </summary>
    int layers = 1;
    /// <summary> 通道型態總數，0 ┼，1 ├，2 ┬，3 ┤，4 ┴，5 ─，6 │ </summary>
    int passwayTypeNum = 7;
    /// <summary> 難度分級數 </summary>
    int level = 3;
    /// <summary> 功能種類數 </summary>
    int functionTypeNum = 3;
    void Start()
    {
        AllData = new Dictionary<int, string[,]>[layers, passwayTypeNum, level, functionTypeNum];
        for(int a = 0; a < files.Length; a++)
        {
            string[] sArray = files[a].name.Split('_');
            int[] num = new int[sArray.Length];
            for (int b=0; b <sArray.Length;b++)
            {
                num[b] = int.Parse(sArray[b]);
            }
            int c = 0;
            AllData[num[c++], num[c++], num[c++], num[c++]].Add(num[c++], LoadData(files[a]));
        }
    }

    void Update()
    {
        
    }

    public static string[,] LoadData(TextAsset file)
    {
        //讀取csv二進位制檔案
        //讀取每一行的內容
        string[] lineArray = file.text.Split("\r"[0]);
        //建立二維陣列
        int i, j;
        string[,] dataArray = new string[lineArray.Length, lineArray[0].Split(',').Length];
        //把csv中的資料儲存在二位陣列中
        for (i = 0; i < lineArray.Length; i++)
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

    public static string[,] rotateArray(string[,] origin)
    {
        string[,] newArray = new string[origin.GetUpperBound(1)+1, origin.GetUpperBound(0)+1];
        int i, j;
        for (i = 0; i < origin.GetUpperBound(0) + 1; i++)
        {
            for (j = 0; j < origin.GetUpperBound(1) + 1; j++)
            {
                newArray[i, origin.GetUpperBound(1) - j] = origin[j, i];
            }
        }
        return newArray;
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
