using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class MonsterAttack : MonoBehaviour
    {
        public float ATK;
        public bool continued = false;
        public string MonsterType;

        protected virtual void OnTriggerEnter2D(Collider2D collider)
        {
            if (!continued)
            {
                if (collider.gameObject.layer == 8)
                {
                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //ShowLockHP.hurtTimer = 0;
                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                    if (PlayerManager.HP / PlayerManager.MaxHP <= 0.4f)
                    {
                        PlayerManager.HP -= ATK * (100f - PlayerManager.reducesDamage) / 100f;
                    }
                    else
                    {
                        PlayerManager.HP -= ATK;
                    }
                    try
                    {
                        Camera.main.GetComponent<Animator>().SetTrigger("Hit");
                    }
                    catch
                    {
                        Debug.LogError("這場景忘了放畫面抖動");
                    }
                    Instantiate(GameManager.Hurted, collider.transform.position, Quaternion.identity, collider.transform);

                    collider.GetComponent<PlayerJoyVibration>().hurt();
                    if(MonsterType == "Spider")
                    {
                        if (collider.GetComponent<PlayerManager>().p1)
                        {
                            GameManager.P1SpiderShooted++;
                        }
                        else
                        {
                            GameManager.P2SpiderShooted++;
                        }
                        GameManager.DiedBecause = "SpiderShoot";
                        GameManager.DiedBecauseTimer = 0;
                    }
                    /*if (collider.transform.childCount>2)
                    {
                        collider.transform.GetChild(2).gameObject.SetActive(true);
                    }*/
                }
            }
        }

        void OnTriggerStay2D(Collider2D collider)
        {
            if (continued)
            {
                Debug.LogError("現在不應該有持續傷害了，如果有請改掉!!" + gameObject.name, gameObject);
                if (collider.gameObject.layer == 8)
                {
                    PlayerManager.HP -= ATK * Time.deltaTime;
                    print(collider.transform.GetChild(2).name);
                    //collider.transform.GetChild(2).gameObject.SetActive(true);
                }
            }
        }
    }
}