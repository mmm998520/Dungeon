using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataLoader : MonoBehaviour
{
    /// <summary> 通道型態list->難度list->功能list->關卡設計list ->關卡內容array </summary>
    List<List<List<List<string[][]>>>> AllData = new List<List<List<List<string[][]>>>>();
    /// <summary> 通道型態總數、難度分級數、功能種類數、單功能房間變化可能數 </summary>
    int passwayTypeNum = 6, level = 3, functionTypeNum = 3, roomNum = 3;
    void Start()
    {
        int i, j, k, l;
        string fileName;
        for (i = 0; i < passwayTypeNum; i++)
        {
            AllData.Add(new List<List<List<string[][]>>>());
            for (j = 0; j < level; j++)
            {
                AllData[i].Add(new List<List<string[][]>>());
                for (k = 0; k < functionTypeNum; k++)
                {
                    AllData[i][j].Add(new List<string[][]>());
                    for (l = 0; l < roomNum; l++)
                    {
                        fileName = i + "_" + j + "_" + k + "_" + l;
                        if (Resources.Load(fileName, typeof(TextAsset)))
                        {
                            AllData[i][j][k].Add(LoadData(fileName));
                        }
                    }
                }
            }
        }
        GameObject.Find("").g
    }

    void Update()
    {
        
    }

    string[][] LoadData(string fileName)
    {
        //讀取csv二進位制檔案
        TextAsset binAsset = Resources.Load(fileName, typeof(TextAsset)) as TextAsset;
        //讀取每一行的內容
        string[] lineArray = binAsset.text.Split("\r"[0]);
        //建立二維陣列
        string[][]  dataArray = new string[lineArray.Length][];
        //把csv中的資料儲存在二位陣列中
        for (int i = 0; i < lineArray.Length; i++)
        {
            dataArray[i] = lineArray[i].Split(',');
        }
        return dataArray;
    }
}
