﻿using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System;

public class DataReverser : MonoBehaviour
{
    public TextAsset[] textAsset;
    string name = "";
    public int passwayNum, level, function;
    public int num,a;
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            for(int i = 0; i < textAsset.Length; i++)
            {
                string[,] newArray = DataLoader.LoadData(textAsset[i]);
                passwayNum = int.Parse(textAsset[i].name.Split('開')[0]);
                if(passwayNum == 4)
                {
                    a = 0;
                }
                else if (passwayNum == 3)
                {
                    a = 1;
                }
                else if (passwayNum == 2)
                {
                    a = 5;
                }
                for (int j = 0; j < 8; j++)
                {
                    name = a + "_" + level + "_" + function + "_" + num;
                    writeFile("D:/Dmmm998520/HomeWork/fourup/Dungeon/Assets/Resources/RoomDatas"+"/"+a+"/"+level+"/"+function, name, arrayToString(newArray));
                    newArray = DataLoader.rotateArray(newArray);
                    if (j == 3)
                    {
                        newArray = DataLoader.reverserArray(newArray);
                    }
                    if (passwayNum == 4)
                    {
                        num++;
                    }
                    else if (passwayNum == 3)
                    {
                        if (++a >= 5)
                        {
                            a = 1;
                        }
                        if (j % 4 == 3)
                        {
                            num++;
                        }
                    }
                    else if(passwayNum == 2)
                    {
                        if (++a >= 7)
                        {
                            a = 5;
                        }
                        if (j % 2 == 1)
                        {
                            num++;
                        }
                    }
                }
                Debug.LogError("k");
            }
        }
    }

    void writeFile(string path, string fileName ,string content)
    {
        FileStream fs = new FileStream(path + "//" + fileName, FileMode.Create);   //開啟一個寫入流
        string str = content;
        byte[] bytes = Encoding.UTF8.GetBytes(str);
        fs.Write(bytes, 0, bytes.Length);
        fs.Flush();     //流會緩衝，此行程式碼指示流不要緩衝資料，立即寫入到檔案。
        fs.Close();     //關閉流並釋放所有資源，同時將緩衝區的沒有寫入的資料，寫入然後再關閉。
        fs.Dispose();   //釋放流所佔用的資源，Dispose()會呼叫Close(),Close()會呼叫Flush();    也會寫入緩衝區內的資料。
    }

    string arrayToString(string[,] array)
    {
        string content = "";
        int i, j;
        for (i = 0; i < array.GetUpperBound(0) + 1; i++)
        {
            for (j = 0; j < array.GetUpperBound(1) + 1; j++)
            {
                content += array[i, j];
                if (j!= array.GetUpperBound(0))
                {
                    content += ",";
                }
            }
            content += "\r";
        }
        return content;
    }
}
