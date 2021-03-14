using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class PlayerAttack : MonoBehaviour
    {
        public HashSet<MonsterManager> monsters = new HashSet<MonsterManager>();
        protected virtual bool attack(Collider2D collider, float damage)
        {
            bool attack = false;
            if (collider.GetComponent<MonsterManager>())
            {
                MonsterManager monsterManager = collider.GetComponent<MonsterManager>();
                if (!monsters.Contains(monsterManager))
                {
                    monsters.Add(monsterManager);
                    if (!(collider.GetComponent<TaurenBoss>() && collider.GetComponent<TaurenBoss>().InvincibleTimer < 0.4f))
                    {
                        monsterManager.HP -= damage;
                        try
                        {
                            monsterManager.HitedAnimator.SetBool("Hit", true);
                        }
                        catch
                        {
                            Debug.LogError("這種怪沒放到受傷特效 : " + collider.name, collider.gameObject);
                        }
                        try
                        {
                            Camera.main.GetComponent<Animator>().SetTrigger("Hit");
                        }
                        catch
                        {
                            Debug.LogError("這場景忘了放畫面抖動");
                        }
                        attack = true;
                    }
                    print(collider.gameObject.name);
                    if (monsterManager.HP <= 0)
                    {
                        monsterManager.beforeDied();
                        Debug.LogWarning("hitTimes");
                    }
                }
            }
            if (collider.GetComponent<Bubble>() || (collider.GetComponent<MonsterShooter>() && collider.GetComponent<MonsterShooter>().canRemoveByPlayerAttack) || collider.GetComponent<MonsterShooter_Bounce>() || (collider.GetComponent<MonsterShooter_Bounce>() && collider.GetComponent<MonsterShooter_Bounce>().canRemoveByPlayerAttack))
            {
                Destroy(collider.gameObject);
            }
            if (collider.GetComponent<BatSticked>())
            {
                Destroy(collider.gameObject);
                int r = Random.Range(1, 3);
                for(int i = 0; i < r; i++)
                {
                    Instantiate(GameManager.gameManager.money, transform.position, Quaternion.identity);
                }
                if (Random.Range(0, 100) < 1)
                {
                    Instantiate(GameManager.gameManager.reLifeParticle, transform.position, Quaternion.identity);
                }
            }
            if (collider.name == "hit role collider")
            {
                Destroy(collider.transform.parent.gameObject);
            }
            return attack;
        }
    }
}