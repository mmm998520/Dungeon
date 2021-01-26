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

        private void Start()
        {
            for (int i = 0; i < 6; i++)
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
                GameObject crystal = Instantiate(Crystal, new Vector3(Random.Range(1f, MazeCreater.totalRow - 2), Random.Range(1f, MazeCreater.totalCol - 2)), Quaternion.identity);
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