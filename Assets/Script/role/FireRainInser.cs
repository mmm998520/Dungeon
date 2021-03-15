using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class FireRainInser : MonoBehaviour
    {
        public static HashSet<int> insPoses = new HashSet<int>();
        float insFireRainTimer = 0;
        List<float> insTimes = new List<float>();
        public GameObject fireRain;
        Transform fireRains;
        public int fireRainNum;

        private void Start()
        {
            fireRains = GameObject.Find("FireRains").transform;
            for(int i = fireRains.childCount; (i < 30 && i < fireRains.childCount + fireRainNum); i++)
            {
                insTimes.Add(Random.Range(0.5f, 1.5f));
            }
            insTimes.Sort();//List升冪排序
        }

        void Update()
        {
            insFireRainTimer += Time.deltaTime;
            if (insFireRainTimer >= insTimes[0])
            {
                int rRow, rCol, times = 0;
                do
                {
                    times++;
                    rRow = Random.Range(1, MazeCreater.totalRow - 2);
                    rCol = Random.Range(1, MazeCreater.totalCol - 2);
                } while (insPoses.Contains(rRow * MazeCreater.totalCol + rCol) && times < 500);
                if(times >= 499)
                {
                    Debug.LogError("沒地方放岩漿了");
                    return;
                }
                for(int i = -2; i <= 2; i++)
                {
                    for (int j = -2; j <= 2; j++)
                    {
                        insPoses.Add((rRow + i) * MazeCreater.totalCol + (rCol + j));
                    }
                }
                Instantiate(fireRain, new Vector3(rRow, rCol), Quaternion.identity, fireRains);
                insTimes.RemoveAt(0);
                if (insTimes.Count <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}