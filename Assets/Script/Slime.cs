using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class Slime : MonsterManager
    {
        private void Awake()
        {
            rotateSpeed = 200;
        }

        void Start()
        {
            target = MinDisPlayer();
            Invoke("Destroy", 10);
        }

        void Update()
        {
            if (Vector3.Distance(transform.position, target.position) > 0.3f)
            {
                target = MinDisPlayer();
                moveToTarget();
            }
            else
            {
                transform.position = target.position;
            }
            if (HP <= 0)
            {
                Destroy(gameObject);
            }
        }

        void Destroy()
        {
            MonsterAttack monsterAttack = Instantiate(attack, transform.position, transform.rotation).GetComponent<MonsterAttack>();
            Destroy(monsterAttack.gameObject, 2f);
            monsterAttack.ATK = ATK;
            monsterAttack.continued = true;
            CDTimer = 0;
            preparationTimer = 0;
            prepare = false;
            Destroy(gameObject);
        }
    }
}