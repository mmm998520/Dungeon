using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class Slime : MonsterManager
    {
        Transform hp;

        void Start()
        {
            hp = transform.GetChild(1);
            rotateSpeed = 200;
            target = MinDisPlayer();
            Invoke("Destroy", 10);
        }

        void Update()
        {
            hp.localScale = new Vector3(HP / MaxHP, hp.localScale.y, hp.localScale.z);
            if (Vector3.Distance(transform.position * Vector2.one, target.position * Vector2.one) > 0.5f)
            {
                target = MinDisPlayer();
                moveToTarget();
            }
            else
            {
                transform.position = target.position;
                target.GetComponent<PlayerManager>().speed = 1.5f;
            }
            if (HP <= 0)
            {
                target.GetComponent<PlayerManager>().speed = 3;
                Destroy(gameObject);
            }
        }

        void Destroy()
        {
            target.GetComponent<PlayerManager>().speed = 3;
            MonsterAttack monsterAttack = Instantiate(attack, transform.position, transform.rotation).GetComponent<MonsterAttack>();
            Destroy(monsterAttack.gameObject, 2f);
            monsterAttack.ATK = ATK;
            monsterAttack.continued = true;
            Destroy(gameObject);
        }
    }
}