using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class WallSpriteSelect : MonoBehaviour
    {
        [SerializeField] bool fire;
        void Start()
        {
            if (!fire && GameManager.layers == 2 && GameManager.CurrentSceneName == "Game 1")
            {
                if (gameObject.name == "Wall(Clone)" || gameObject.name == "Wall")
                {
                    Destroy(gameObject);
                    Instantiate(Resources.Load<GameObject>("Prefabs/Temp/wall_red Variant"), transform.position, Quaternion.identity);
                    return;
                }
            }
            int r = Random.Range(0, transform.childCount);
            for(int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(i == r);
            }
        }
    }
}