using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class Sensor : MonoBehaviour
    {
        public MazeCreater mazeCreater;
        public int row, col;
        static List<GameObject> sensors = new List<GameObject>();
        public static GameObject Exit;
        static Sensor exitSensor;
        private void Awake()
        {
            sensors.Clear();
        }

        void Start()
        {
            sensors.Add(gameObject);
        }

        void Update()
        {
            if(exitSensor == null)
            {
                if (Exit == null)
                {
                    Exit = GameObject.Find("旋鈕(Clone)");
                }
                else
                {
                    float minDis = float.MaxValue;
                    for(int i=0;i< sensors.Count; i++)
                    {
                        if(minDis > Vector3.Distance(Exit.transform.position, sensors[i].transform.position))
                        {
                            minDis = Vector3.Distance(Exit.transform.position, sensors[i].transform.position);
                            exitSensor = sensors[i].GetComponent<Sensor>();
                        }
                    }
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if(collider.gameObject.layer == 8)
            {
                mazeCreater.creat(row, col);
                foreach(Transform child in GameManager.monsters)
                {
                    child.GetComponent<Navigate>().arriveNewRoom(row, col);
                }
                Destroy(gameObject.GetComponent<BoxCollider2D>());
            }
        }

        bool checkEmpty()
        {
            bool empty = !Physics2D.BoxCast(transform.position, transform.localScale, 0, Vector2.right, 0, 1 << 9);
            return empty;
        }
        
        public static bool WeAreInEmptyFinalRoom()
        {
            RaycastHit2D[] hits = Physics2D.BoxCastAll(exitSensor.transform.position, exitSensor.transform.localScale, 0, Vector2.right, 0, 1 << 8);
            bool WeAreHere = (hits.Length >= 2);
            return exitSensor && exitSensor.checkEmpty() && WeAreHere;
        }

        public static int  emptyRoomNum()
        {
            //讓怪物顯示
            List<GameObject> hidedMonster = new List<GameObject>();
            for(int i = 0; i < GameManager.monsters.childCount; i++)
            {
                GameObject monster = GameManager.monsters.GetChild(i).gameObject;
                if (!monster.activeSelf)
                {
                    hidedMonster.Add(monster);
                    monster.SetActive(true);
                }
            }
            //計算空房間數量
            int num = 0;
            for(int i = 0; i < sensors.Count; i++)
            {
                if (!sensors[i].GetComponent<Collider2D>() && sensors[i].GetComponent<Sensor>().checkEmpty())
                {
                    num++;
                }
            }
            //關閉多餘的怪物
            for (int i = 0; i < hidedMonster.Count; i++)
            {
                hidedMonster[i].SetActive(false);
            }
            Debug.LogError(num);
            return num;
        }
    }
}