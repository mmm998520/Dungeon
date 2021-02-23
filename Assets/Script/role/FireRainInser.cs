using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class FireRainInser : MonoBehaviour
    {
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
                Instantiate(fireRain, new Vector3(Random.Range(1f, MazeCreater.totalRow - 2), Random.Range(1f, MazeCreater.totalCol - 2)), Quaternion.identity, fireRains);
                insTimes.RemoveAt(0);
                if (insTimes.Count <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}