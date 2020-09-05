using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.BoardGameDungeon
{
    public class Spine : MonoBehaviour
    {
        int ATK = 5;
        public PlayerManager user;
        public float popTimer = 10;
        const float popTimerStoper = 1;

        private void Update()
        {
            if ((popTimer += Time.deltaTime) > popTimerStoper)
            {
                transform.GetComponent<Collider2D>().enabled = false;
                transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = false;
            }
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.GetComponent<ValueSet>())
            {
                collider.GetComponent<ValueSet>().Hurt += ATK;
            }
            MonsterManager.addHurtMe(collider, user);
        }
    }
}