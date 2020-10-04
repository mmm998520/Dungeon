using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class TriggerGen : MonoBehaviour
    {
        List<int> canGenPos = new List<int>();
        public Transform map;
        void Start()
        {
            int i;
            for(i = 0; i < MapCreater.totalRow[MapCreater.level] * MapCreater.totalCol[MapCreater.level]; i++)
            {
                canGenPos.Add(i);
            }
            map = GameManager.maze;
            for (i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<Trigger>().start();
            }
        }

        public GameObject gen(GameObject prefab)
        {
            int r = Random.Range(0, canGenPos.Count);
            GameObject gened = Instantiate(prefab,transform.GetChild(canGenPos[r]).position,Quaternion.identity);
            gened.GetComponent<Trigger>().GenNum = canGenPos[r];
            canGenPos.RemoveAt(r);
            return gened;
        }

        public void rePos(Transform gened)
        {
            print(canGenPos.Count);
            int r = Random.Range(0, canGenPos.Count);
            print(canGenPos[r]);
            gened.position = map.GetChild(canGenPos[r]).position;
            canGenPos.Add(gened.GetComponent<Trigger>().GenNum);
            gened.GetComponent<Trigger>().GenNum = canGenPos[r];
            canGenPos.RemoveAt(r);
        }
    }
}