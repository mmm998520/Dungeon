using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class PlayerAttack : MonoBehaviour
    {
        public HashSet<MonsterManager> monsterManagers = new HashSet<MonsterManager>();
        protected virtual bool attack(Collider2D collider, float damage)
        {
            bool attack = false;
            if (collider.GetComponent<MonsterManager>() && !monsterManagers.Contains(collider.GetComponent<MonsterManager>()))
            {
                monsterManagers.Add(collider.GetComponent<MonsterManager>());
                if (!(collider.GetComponent<TaurenBoss>() && collider.GetComponent<TaurenBoss>().InvincibleTimer < 0.4f))
                {
                    collider.GetComponent<MonsterManager>().HP -= damage;
                    attack = true;
                }
                print(collider.gameObject.name);
                if (collider.GetComponent<MonsterManager>().HP <= 0)
                {
                    collider.GetComponent<MonsterManager>().beforeDied();
                    Debug.LogWarning("hitTimes");
                }
            }
            if (collider.GetComponent<BatSticked>() || collider.GetComponent<Bubble>() || (collider.GetComponent<MonsterShooter>() && collider.GetComponent<MonsterShooter>().canRemoveByPlayerAttack) || (collider.GetComponent<MonsterShooter_Bounce>() && collider.GetComponent<MonsterShooter_Bounce>().canRemoveByPlayerAttack))
            {
                Destroy(collider.gameObject);
            }
            if (collider.name == "hit role collider")
            {
                Destroy(collider.transform.parent.gameObject);
            }
            return attack;
        }
    }
}