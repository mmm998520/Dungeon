using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class CrystalRainInser : MonoBehaviour
    {
        public static HashSet<int> insPoses = new HashSet<int>();
        float insCrystalRainTimer = 0;
        List<float> insTimes = new List<float>();
        public GameObject Crystal;
        Transform crystalRains;

        private void Start()
        {
            crystalRains = GameObject.Find("CrystalRains").transform;
            for (int i = crystalRains.childCount; (i < 6 && i < crystalRains.childCount + 1); i++)
            {
                insTimes.Add(Random.Range(0.5f, 1.5f));
            }
            insTimes.Sort();//List升冪排序
        }

        void Update()
        {
            insCrystalRainTimer += Time.deltaTime;
            if (insCrystalRainTimer >= insTimes[0])
            {
                int rRow, rCol, times = 0;
                do
                {
                    times++;
                    rRow = Random.Range(1 + 4, MazeCreater.totalRow - 2 - 4);
                    rCol = Random.Range(1 + 4, MazeCreater.totalCol - 2 - 4);
                } while (insPoses.Contains(rRow * MazeCreater.totalCol + rCol) && times < 500);
                if (times >= 499)
                {
                    Debug.LogError("沒地方放水晶了");
                    return;
                }
                for (int i = -4; i <= 4; i++)
                {
                    for (int j = -4; j <= 4; j++)
                    {
                        insPoses.Add((rRow + i) * MazeCreater.totalCol + (rCol + j));
                    }
                }
                GameObject crystal = Instantiate(Crystal, new Vector3(rRow, rCol), Quaternion.identity, crystalRains);
                crystal.GetComponent<Crystal>().crystalLight.intensity = 0.6f;
                insTimes.RemoveAt(0);
                if (insTimes.Count <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}