using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class SpriteRandomer : MonoBehaviour
    {
        public Sprite[] sprites;
        public bool wall;
        public bool randomRotate;
        public bool holeSide;
        [SerializeField] bool fire;
        void Start()
        {
            if ((GameManager.layers ==2 && GameManager.CurrentSceneName == "Game 1") || GameManager.CurrentSceneName == "Game 4")
            {
                if (!fire && transform.parent.gameObject.name == "floor" || transform.parent.gameObject.name == "floor(Clone)")
                {
                    Destroy(transform.parent.gameObject);
                    Instantiate(Resources.Load<GameObject>("Prefabs/Temp/floor_fire Variant"), transform.position, Quaternion.identity);
                    return;
                }
                else if (transform.parent.gameObject.name.Contains("上") || transform.parent.gameObject.name.Contains("下") || transform.parent.gameObject.name.Contains("左") || transform.parent.gameObject.name.Contains("右"))
                {
                    sprites[0] = Resources.Load<Sprite>("Pictures/floorbroken_red");
                }
            }
            GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Length)];
            if (holeSide)
            {
                Transform[] childs = new Transform[transform.childCount];
                for(int i = 0; i < childs.Length; i++)
                {
                    childs[i] = transform.GetChild(i);
                }
                for (int i = 0; i < childs.Length; i++)
                {
                    childs[i].SetParent(null);
                }
                transform.Rotate(0, 0, 90 * Random.Range(0, 4));
                for (int i = 0; i < childs.Length; i++)
                {
                    childs[i].SetParent(transform);
                }
            }
            else if (!wall)
            {
                transform.rotation = Quaternion.Euler(0, 180 * Random.Range(0, 2), 0);
            }
            else
            {
                int r = Random.Range(0, 2);
                transform.parent.GetChild(0).rotation = Quaternion.Euler(0, 180 * r, 0);
                transform.parent.GetChild(1).rotation = Quaternion.Euler(0, 180 * r, 0);
            }
            if (randomRotate)
            {
                transform.Rotate(0, 0, 90 * Random.Range(0, 4));
            }

        }

        void Update()
        {

        }
    }
}