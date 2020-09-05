using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.BoardGameDungeon
{
    /// <summary> 在陷阱前後產生包夾，如果有牆阻隔，則盡可能選可以完成包夾的位置進行生成 </summary>
    public class Alarm : MonoBehaviour
    {
        public GameObject monster;

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.GetComponent<PlayerManager>())
            {
                //向4方位打ray看有沒有碰到牆，以此來判斷在哪生出怪物
                Vector3 currentPos = transform.position * Vector2.one;
                RaycastHit2D[] hits = new RaycastHit2D[4];
                int hitNum = 0;
                Vector2 dir = Vector3.up;
                for(int i = 0; i < 4; i++)
                {
                    hits[i] = Physics2D.Raycast(currentPos, dir, 2, 1 << 8);
                    dir = Quaternion.Euler(0, 0, 90) * dir;
                    if (hits[i])
                    {
                        hitNum++;
                    }
                }
                //如果所有方向都打到......?
                if (hitNum >= 4)
                {
                    Debug.LogError("我很好奇你現在在哪...");

                }
                else if(hitNum >= 3)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        //如果雷射方向沒有打到牆，就生成在那跟他的反方向
                        if (!hits[i])
                        {
                            Instantiate(monster, transform.position * Vector2.one + dir * 2, Quaternion.identity);
                            Instantiate(monster, transform.position * Vector2.one - dir * 2, Quaternion.identity);
                            break;
                        }
                        dir = Quaternion.Euler(0, 0, 90) * dir;
                    }
                }
                else if (hitNum >= 2)
                {
                    dir = Vector2.up;
                    for (int i = 0; i < 4; i++)
                    {
                        //如果雷射方向有打到牆，就直接生成在那(因為不管怎樣都可以達成包夾)
                        if (!hits[i])
                        {
                            Instantiate(monster, transform.position * Vector2.one + dir * 2, Quaternion.identity);
                        }
                        dir = Quaternion.Euler(0, 0, 90) * dir;
                    }
                }
                else if(hitNum >= 1)
                {
                    RaycastHit2D hit = Physics2D.Raycast(currentPos - Vector3.up * 2, currentPos, 4, 1 << 8);
                    //如果上下有打到牆，生成左右
                    if (hit)
                    {
                        Instantiate(monster, transform.position * Vector2.one + Vector2.right * 2, Quaternion.identity);
                        Instantiate(monster, transform.position * Vector2.one + Vector2.left * 2, Quaternion.identity);
                    }
                    //反之，左右有打到牆，生成上下
                    else
                    {
                        Instantiate(monster, transform.position * Vector2.one + Vector2.up * 2, Quaternion.identity);
                        Instantiate(monster, transform.position * Vector2.one + Vector2.down * 2, Quaternion.identity);
                    }
                }
                //如果所有方向都沒hit
                else
                {
                    //上下或左右隨機一個進行生成
                    if (Random.Range(0, 2) < 1)
                    {
                        Instantiate(monster, transform.position * Vector2.one + Vector2.up * 2, Quaternion.identity);
                        Instantiate(monster, transform.position * Vector2.one + Vector2.down * 2, Quaternion.identity);
                    }
                    else
                    {
                        Instantiate(monster, transform.position * Vector2.one + Vector2.right * 2, Quaternion.identity);
                        Instantiate(monster, transform.position * Vector2.one + Vector2.left * 2, Quaternion.identity);

                    }
                }

                Destroy(gameObject);
            }
        }
    }
}