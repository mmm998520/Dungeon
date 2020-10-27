using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace com.DungeonPad
{
    public class SmallMap : MonoBehaviour
    {
        public int unitLong;
        public RectTransform playerPos;
        public void start()
        {
            playerPos = transform.parent.GetChild(1).GetComponent<RectTransform>();
            transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(unitLong, unitLong);
            RectTransform temp;
            int i, j;
            for(i = 0; i < 48; i++)
            {
                for(j = 0; j < 48; j++)
                {
                    if (!(i == 0 && j == 0))
                    {
                        temp = Instantiate(transform.GetChild(0).gameObject, Vector3.zero, Quaternion.identity, transform).GetComponent<RectTransform>();
                        temp.anchoredPosition = new Vector2(unitLong * i, unitLong * j);
                        temp.name = i + "_" + j;
                    }
                }
            }
        }

        public void update()
        {
            if (Time.time > 0.1f)
            {
                Transform temp;
                Collider2D[] colliders = Physics2D.OverlapCircleAll(CameraManager.center, 4, 1 << 15);
                for (int i = 0; i < colliders.Length; i++)
                {
                    temp = transform.GetChild(Mathf.RoundToInt(colliders[i].transform.position.x) * MazeCreater.totalCol + Mathf.RoundToInt(colliders[i].transform.position.y));
                    temp.GetComponent<Image>().enabled = false;
                    colliders[i].enabled = false;
                }
                colliders = Physics2D.OverlapCircleAll(CameraManager.center, 7, 1 << 15);
                for (int i = 0; i < colliders.Length; i++)
                {
                    temp = transform.GetChild(Mathf.RoundToInt(colliders[i].transform.position.x) * MazeCreater.totalCol + Mathf.RoundToInt(colliders[i].transform.position.y));
                    if (temp.GetComponent<Image>().color.a > 0.75f)
                    {
                        temp.GetComponent<Image>().color = new Color(1, 1, 1, 0.75f);
                    }
                }
                colliders = Physics2D.OverlapCircleAll(CameraManager.center, 6, 1 << 15);
                for (int i = 0; i < colliders.Length; i++)
                {
                    temp = transform.GetChild(Mathf.RoundToInt(colliders[i].transform.position.x) * MazeCreater.totalCol + Mathf.RoundToInt(colliders[i].transform.position.y));
                    if (temp.GetComponent<Image>().color.a > 0.5f)
                    {
                        temp.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
                    }
                }
                colliders = Physics2D.OverlapCircleAll(CameraManager.center, 5, 1 << 15);
                for (int i = 0; i < colliders.Length; i++)
                {
                    temp = transform.GetChild(Mathf.RoundToInt(colliders[i].transform.position.x) * MazeCreater.totalCol + Mathf.RoundToInt(colliders[i].transform.position.y));
                    temp.GetComponent<Image>().color = new Color(1, 1, 1, 0.25f);
                }
            }
            playerPos.anchoredPosition = new Vector2(unitLong * CameraManager.center.x, unitLong * CameraManager.center.y);
        }
    }
}