using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class FloorLight : MonoBehaviour
    {
        int stat = 0;
        float t = 0;
        public Sprite[] sprites;

        void Start()
        {
            int r = Random.Range(0, sprites.Length);
            for (int i = 0; i < 3; i++)
            {
                transform.GetChild(i).gameObject.SetActive(stat == i);
                transform.GetChild(i).GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[r];
            }
        }

        void Update()
        {
            if ((t += Time.deltaTime) > 0.1f)
            {
                int r = Random.Range(0, sprites.Length);
                t = 0;
                if (++stat > 2)
                {
                    stat = 0;
                }
                for(int i = 0; i < 3; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(stat == i);
                    transform.GetChild(i).GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[r];
                }
            }
        }
    }
}