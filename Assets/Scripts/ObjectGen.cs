using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.BoardGameDungeon
{
    /// <summary>
    /// <para>讓物件在四處隨機生成，不會生成在四角(玩家出生點)</para>
    /// <para>*** 有生成地點條件的優先生成(擺前面)，避免生成不了的麻煩 ***</para>
    /// </summary>
    public class ObjectGen : MonoBehaviour
    {
        public List<GameObject> Objects = new List<GameObject>();
        public List<int> Num = new List<int>();
        public List<int> TaurenRangeRow = new List<int>();
        public List<int> TaurenRangeCol = new List<int>();
        public GameObject StabPrefab;
        void Start()
        {
            Dictionary<GameObject, int> ObjectNum = new Dictionary<GameObject, int>();
            List<int[]> pos = new List<int[]>();
            Transform[] Stabs = new Transform[Num[Objects.IndexOf(StabPrefab)]];

            for (int i = 0; i < Objects.Count; i++)
            {
                ObjectNum.Add(Objects[i], Num[i]);
            }
            for (int i = 0; i < MazeGen.row; i++)
            {
                for (int j = 0; j < MazeGen.col; j++)
                {
                    if (!((i == 0 && j == 0) || (i == 0 && j == MazeGen.col - 1) || (i == MazeGen.row - 1 && j == 0) || (i == MazeGen.row - 1 && j == MazeGen.col - 1)))
                    {
                        pos.Add(new int[2] { i, j });
                    }
                }
            }

            foreach (var obj in ObjectNum)
            {
                for(int i = 0; i < obj.Value; i++)
                {
                    GameObject ins = null;
                    if(obj.Key.name == "Slime")
                    {
                        int s = 0;
                        do
                        {
                            s++;
                            int r = Random.Range(0, pos.Count);
                            //不能生成在中央
                            if(pos[r][0] <= 1 || pos[r][1] <= 1 || pos[r][0] >= MazeGen.row - 2 || pos[r][1] >= MazeGen.col - 2)
                            {
                                ins = Instantiate(obj.Key, new Vector3(pos[r][0] * 2 + 1, pos[r][1] * 2 + 1, 0), Quaternion.identity);
                                pos.RemoveAt(r);
                                break;
                            }
                        } while (s<100);
                    }
                    else if(obj.Key.name == "Tauren")
                    {
                        int s = 0;
                        do
                        {
                            s++;
                            int r = Random.Range(0, pos.Count);
                            //不能生成在周圍
                            if (!(pos[r][0] <= 1 || pos[r][1] <= 1 || pos[r][0] >= MazeGen.row - 2 || pos[r][1] >= MazeGen.col - 2))
                            {
                                if (i == 0 && pos[r][0] <= 5 && pos[r][1] <= 3)
                                {
                                    ins = Instantiate(obj.Key, new Vector3(pos[r][0] * 2 + 1, pos[r][1] * 2 + 1, 0), Quaternion.identity);
                                    pos.RemoveAt(r);
                                    break;
                                }
                                if (i == 1 && pos[r][0] >= 6 && pos[r][1] >= 5)
                                {
                                    ins = Instantiate(obj.Key, new Vector3(pos[r][0] * 2 + 1, pos[r][1] * 2 + 1, 0), Quaternion.identity);
                                    pos.RemoveAt(r);
                                    break;
                                }
                                else if (i == 2 && pos[r][0] <= 5 && pos[r][1] >= 5)
                                {
                                    ins = Instantiate(obj.Key, new Vector3(pos[r][0] * 2 + 1, pos[r][1] * 2 + 1, 0), Quaternion.identity);
                                    pos.RemoveAt(r);
                                    break;
                                }
                                else if (i == 3 && pos[r][0] >= 6 && pos[r][1] <= 3)
                                {
                                    ins = Instantiate(obj.Key, new Vector3(pos[r][0] * 2 + 1, pos[r][1] * 2 + 1, 0), Quaternion.identity);
                                    pos.RemoveAt(r);
                                    break;
                                }
                            }
                        } while (s<100);
                    }
                    else if(obj.Key.name == "Exit")
                    {
                        int s = 0;
                        do
                        {
                            s++;
                            int r = Random.Range(0, pos.Count);
                            //不能生成在周圍
                            if (!(pos[r][0] <= 1 || pos[r][1] <= 1 || pos[r][0] >= MazeGen.row - 2 || pos[r][1] >= MazeGen.col - 2))
                            {
                                ins = Instantiate(obj.Key, new Vector3(pos[r][0] * 2 + 1, pos[r][1] * 2 + 1, 0), Quaternion.identity);
                                pos.RemoveAt(r);
                                break;
                            }
                        } while (s < 100);
                    }
                    else if(obj.Key.name == "Spine")
                    {
                        if (i < 2)
                        {
                            if (Stabs[i] != null)
                            {
                                ins = Instantiate(obj.Key, Stabs[i].position, Quaternion.identity);
                            }
                            else
                            {
                                Debug.LogError("Why!!!!!");
                            }
                        }
                        else if (i < 6)
                        {
                            int r = Random.Range(0, pos.Count);
                            ins = Instantiate(obj.Key, new Vector3(pos[r][0] * 2 + 1, pos[r][1] * 2 + 1, 0), Quaternion.identity);
                            pos.RemoveAt(r);
                        }
                        else
                        {
                            Debug.LogError("數值設定有問題");
                        }
                        ins.transform.parent = Stabs[i];
                    }
                    else
                    {
                        int r = Random.Range(0, pos.Count);
                        ins = Instantiate(obj.Key, new Vector3(pos[r][0] * 2 + 1, pos[r][1] * 2 + 1, 0), Quaternion.identity);
                        pos.RemoveAt(r);
                    }
                    if(ins!= null)
                    {
                        if (ins.name == "Tauren(Clone)")
                        {
                            if(i == 0)
                            {
                                ins.GetComponent<Tauren>().range = new Transform[8];
                                for (int j = 0; j < 8; j++)
                                {
                                    ins.GetComponent<Tauren>().range[j] = GameManager.Floors.GetChild(TaurenRangeRow[j] * MazeGen.col + TaurenRangeCol[j]);
                                }
                            }
                            else if(i == 1)
                            {
                                ins.GetComponent<Tauren>().range = new Transform[8];
                                for (int j = 8; j < 16; j++)
                                {
                                    ins.GetComponent<Tauren>().range[j-8] = GameManager.Floors.GetChild(TaurenRangeRow[j] * MazeGen.col + TaurenRangeCol[j]);
                                }
                            }
                            else if(i == 2)
                            {
                                ins.GetComponent<Tauren>().range = new Transform[7];
                                for (int j = 16; j < 23; j++)
                                {
                                    ins.GetComponent<Tauren>().range[j-16] = GameManager.Floors.GetChild(TaurenRangeRow[j] * MazeGen.col + TaurenRangeCol[j]);
                                }
                            }
                            else if(i == 3)
                            {
                                ins.GetComponent<Tauren>().range = new Transform[7];
                                for (int j = 23; j < 30; j++)
                                {
                                    ins.GetComponent<Tauren>().range[j-23] = GameManager.Floors.GetChild(TaurenRangeRow[j] * MazeGen.col + TaurenRangeCol[j]);
                                }
                            }
                            else
                            {
                                Debug.LogError("數值設定有問題");
                            }
                        }
                        else if (ins.name == "Exit(Clone)")
                        {
                            ins.transform.parent = GameObject.Find("ExitSwitcher(Clone)").transform;
                            ins.SetActive(false);
                        }
                        else if (ins.name == "Exp(Clone)")
                        {
                            if (i < 3)
                            {
                                ins.GetComponent<Exp>().career = ValueSet.Career.Thief;
                                ins.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Picture/Trigger/Exp/Green");
                            }
                            else if (i < 6)
                            {
                                ins.GetComponent<Exp>().career = ValueSet.Career.Warrior;
                                ins.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Picture/Trigger/Exp/Red");
                            }
                            else if (i < 9)
                            {
                                ins.GetComponent<Exp>().career = ValueSet.Career.Magician;
                                ins.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Picture/Trigger/Exp/Blue");
                            }
                            else
                            {
                                Debug.LogError("數值設定有問題");
                            }
                        }
                        else if (ins.name == "Medical(Clone)")
                        {
                            if (i < 4)
                            {
                                ins.GetComponent<Medical>().recovery = 5;
                                ins.transform.localScale *= 0.7f;
                            }
                            else if (i < 7)
                            {
                                ins.GetComponent<Medical>().recovery = 10;
                            }
                            else
                            {
                                Debug.LogError("數值設定有問題");
                            }
                        }
                        else if (ins.name == "Stab(Clone)")
                        {
                            Stabs[i] = ins.transform;
                        }
                        else if(ins.name == "Spine(Clone)")
                        {
                            if (i < 2)
                            {
                                Stabs[i].GetComponent<Stab>().spines.Add(ins.transform);
                            }
                            else
                            {
                                for(int j =2; j < obj.Value; j++)
                                {
                                    Stabs[j].GetComponent<Stab>().spines.Add(ins.transform);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}