﻿using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System;

namespace com.DungeonPad
{
    public class DataReverser : MonoBehaviour
    {
        public enum InputType
        {
            手動輸入,
            自動輸入_起點,
            自動輸入_過程,
            自動輸入_終點,
        }
        public InputType inputType;
        public TextAsset[] textAsset;
        string name = "";
        public int layers, passwayNum, level, function;
        public int num, passwayType;
        void Start()
        {

        }

        void Update()
        {
            switch (inputType)
            {
                case InputType.自動輸入_起點:
                    function = 0;
                    break;
                case InputType.自動輸入_過程:
                    function = 1;
                    break;
                case InputType.自動輸入_終點:
                    function = 2;
                    break;
            }
                    if (Input.GetKeyDown(KeyCode.R))
            {
                switch (inputType)
                {
                    case InputType.手動輸入:
                        input();
                        break;
                    case InputType.自動輸入_起點:
                        function = 0;
                        num = 0;
                        for (level = 1; level < 8; level++)
                        {
                            input();
                            num = 0;
                        }
                        textAsset = new TextAsset[0];
                        passwayNum++;
                        level = 1;
                        break;
                    case InputType.自動輸入_過程:
                        function = 1;
                        input();
                        textAsset = new TextAsset[0];
                        passwayNum++;
                        num = 0;
                        break;
                    case InputType.自動輸入_終點:
                        function = 2;
                        num = 0;
                        for (level = 1; level < 8; level++)
                        {
                            input();
                            num = 0;
                        }
                        textAsset = new TextAsset[0];
                        passwayNum++;
                        level = 1;
                        break;
                }

            }
        }

        void input()
        {
            for (int i = 0; i < textAsset.Length; i++)
            {
                string[,] newArray = DataLoader.LoadData_Create(textAsset[i]);
                passwayNum = int.Parse(textAsset[i].name.Split('開')[0]);
                if (passwayNum == 4)
                {
                    passwayType = 0;
                }
                else if (passwayNum == 3)
                {
                    passwayType = 1;
                }
                else if (passwayNum == 2)
                {
                    passwayType = 5;
                }
                for (int j = 0; j < 8; j++)
                {
                    name = layers + "_" + passwayType + "_" + level + "_" + function + "_" + num;
                    writeFile(Application.dataPath + "/Resources/RoomDatas/" + layers + "/" + passwayType + "/" + level + "/" + function, name, arrayToString(newArray));
                    newArray = Rotate(newArray);
                    if (j == 3)
                    {
                        newArray = reverserArray(newArray);
                    }
                    if (passwayNum == 4)
                    {
                        num++;
                    }
                    else if (passwayNum == 3)
                    {
                        if (++passwayType >= 5)
                        {
                            passwayType = 1;
                        }
                        if (j % 4 == 3)
                        {
                            num++;
                        }
                    }
                    else if (passwayNum == 2)
                    {
                        if (++passwayType >= 7)
                        {
                            passwayType = 5;
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

        void writeFile(string path, string fileName, string content)
        {
            FileStream fs = new FileStream(path + "//" + fileName + ".txt", FileMode.Create);   //開啟一個寫入流
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
                    if (j != array.GetUpperBound(0))
                    {
                        content += ",";
                    }
                }
                content += "\r";
            }
            return content;
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
                    switch (rotated[newRow, newColumn])
                    {
                        #region//翻轉門
                        case "w1上4":
                            rotated[newRow, newColumn] = "w1右4";
                            break;
                        case "w1上3":
                            rotated[newRow, newColumn] = "w1右3";
                            break;
                        case "w1上2":
                            rotated[newRow, newColumn] = "w1右2";
                            break;
                        case "w1右4":
                            rotated[newRow, newColumn] = "w1下4";
                            break;
                        case "w1右3":
                            rotated[newRow, newColumn] = "w1下3";
                            break;
                        case "w1右2":
                            rotated[newRow, newColumn] = "w1下2";
                            break;
                        case "w1下4":
                            rotated[newRow, newColumn] = "w1左4";
                            break;
                        case "w1下3":
                            rotated[newRow, newColumn] = "w1左3";
                            break;
                        case "w1下2":
                            rotated[newRow, newColumn] = "w1左2";
                            break;
                        case "w1左4":
                            rotated[newRow, newColumn] = "w1上4";
                            break;
                        case "w1左3":
                            rotated[newRow, newColumn] = "w1上3";
                            break;
                        case "w1左2":
                            rotated[newRow, newColumn] = "w1上2";
                            break;
                        case "w2上4":
                            rotated[newRow, newColumn] = "w2右4";
                            break;
                        case "w2上3":
                            rotated[newRow, newColumn] = "w2右3";
                            break;
                        case "w2上2":
                            rotated[newRow, newColumn] = "w2右2";
                            break;
                        case "w2右4":
                            rotated[newRow, newColumn] = "w2下4";
                            break;
                        case "w2右3":
                            rotated[newRow, newColumn] = "w2下3";
                            break;
                        case "w2右2":
                            rotated[newRow, newColumn] = "w2下2";
                            break;
                        case "w2下4":
                            rotated[newRow, newColumn] = "w2左4";
                            break;
                        case "w2下3":
                            rotated[newRow, newColumn] = "w2左3";
                            break;
                        case "w2下2":
                            rotated[newRow, newColumn] = "w2左2";
                            break;
                        case "w2左4":
                            rotated[newRow, newColumn] = "w2上4";
                            break;
                        case "w2左3":
                            rotated[newRow, newColumn] = "w2上3";
                            break;
                        case "w2左2":
                            rotated[newRow, newColumn] = "w2上2";
                            break;
                        #endregion
                        #region//翻轉洞(岸
                        case "d上":
                            rotated[newRow, newColumn] = "d右";
                            break;
                        case "d右":
                            rotated[newRow, newColumn] = "d下";
                            break;
                        case "d下":
                            rotated[newRow, newColumn] = "d左";
                            break;
                        case "d左":
                            rotated[newRow, newColumn] = "d上";
                            break;
                        case "d上右":
                            rotated[newRow, newColumn] = "d右下";
                            break;
                        case "d右下":
                            rotated[newRow, newColumn] = "d下左";
                            break;
                        case "d下左":
                            rotated[newRow, newColumn] = "d左上";
                            break;
                        case "d左上":
                            rotated[newRow, newColumn] = "d上右";
                            break;
                        case "上右d":
                            rotated[newRow, newColumn] = "右下d";
                            break;
                        case "右下d":
                            rotated[newRow, newColumn] = "下左d";
                            break;
                        case "下左d":
                            rotated[newRow, newColumn] = "左上d";
                            break;
                        case "左上d":
                            rotated[newRow, newColumn] = "上右d";
                            break;
                        case "fd上":
                            rotated[newRow, newColumn] = "fd右";
                            break;
                        case "fd右":
                            rotated[newRow, newColumn] = "fd下";
                            break;
                        case "fd下":
                            rotated[newRow, newColumn] = "fd左";
                            break;
                        case "fd左":
                            rotated[newRow, newColumn] = "fd上";
                            break;
                        case "fd上右":
                            rotated[newRow, newColumn] = "fd右下";
                            break;
                        case "fd右下":
                            rotated[newRow, newColumn] = "fd下左";
                            break;
                        case "fd下左":
                            rotated[newRow, newColumn] = "fd左上";
                            break;
                        case "fd左上":
                            rotated[newRow, newColumn] = "fd上右";
                            break;
                        case "上右fd":
                            rotated[newRow, newColumn] = "右下fd";
                            break;
                        case "右下fd":
                            rotated[newRow, newColumn] = "下左fd";
                            break;
                        case "下左fd":
                            rotated[newRow, newColumn] = "左上fd";
                            break;
                        case "左上fd":
                            rotated[newRow, newColumn] = "上右fd";
                            break;
                            #endregion
                    }
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
                    switch (newArray[i, j])
                    {
                        #region//翻轉門
                        case "w1上4":
                            newArray[i, j] = "w1下4";
                            break;
                        case "w1上3":
                            newArray[i, j] = "w1下3";
                            break;
                        case "w1上2":
                            newArray[i, j] = "w1下2";
                            break;
                        case "w1下4":
                            newArray[i, j] = "w1上4";
                            break;
                        case "w1下3":
                            newArray[i, j] = "w1上3";
                            break;
                        case "w1下2":
                            newArray[i, j] = "w1上2";
                            break;
                        case "w2上4":
                            newArray[i, j] = "w2下4";
                            break;
                        case "w2上3":
                            newArray[i, j] = "w2下3";
                            break;
                        case "w2上2":
                            newArray[i, j] = "w2下2";
                            break;
                        case "w2下4":
                            newArray[i, j] = "w2上4";
                            break;
                        case "w2下3":
                            newArray[i, j] = "w2上3";
                            break;
                        case "w2下2":
                            newArray[i, j] = "w2上2";
                            break;
                        #endregion
                        #region//翻轉洞(岸
                        case "d上":
                            newArray[i, j] = "d下";
                            break;
                        case "d下":
                            newArray[i, j] = "d上";
                            break;
                        case "d上右":
                            newArray[i, j] = "d右下";
                            break;
                        case "d右下":
                            newArray[i, j] = "d上右";
                            break;
                        case "d下左":
                            newArray[i, j] = "d左上";
                            break;
                        case "d左上":
                            newArray[i, j] = "d下左";
                            break;
                        case "上右d":
                            newArray[i, j] = "右下d";
                            break;
                        case "右下d":
                            newArray[i, j] = "上右d";
                            break;
                        case "下左d":
                            newArray[i, j] = "左上d";
                            break;
                        case "左上d":
                            newArray[i, j] = "下左d";
                            break;
                        case "fd上":
                            newArray[i, j] = "fd下";
                            break;
                        case "fd下":
                            newArray[i, j] = "fd上";
                            break;
                        case "fd上右":
                            newArray[i, j] = "fd右下";
                            break;
                        case "fd右下":
                            newArray[i, j] = "fd上右";
                            break;
                        case "fd下左":
                            newArray[i, j] = "fd左上";
                            break;
                        case "fd左上":
                            newArray[i, j] = "fd下左";
                            break;
                        case "上右fd":
                            newArray[i, j] = "右下fd";
                            break;
                        case "右下fd":
                            newArray[i, j] = "上右fd";
                            break;
                        case "下左fd":
                            newArray[i, j] = "左上fd";
                            break;
                        case "左上fd":
                            newArray[i, j] = "下左fd";
                            break;
                            #endregion
                    }
                }
            }
            return newArray;
        }
    }
}