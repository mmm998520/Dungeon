using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class CrystalRainInser : MonoBehaviour
    {
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
                GameObject crystal = Instantiate(Crystal, new Vector3(Random.Range(1f, MazeCreater.totalRow - 2), Random.Range(1f, MazeCreater.totalCol - 2)), Quaternion.identity, crystalRains);
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