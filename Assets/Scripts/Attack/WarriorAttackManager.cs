using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.BoardGameDungeon
{
    public class WarriorAttackManager : AttackManager
    {
        float timer;
        public Sprite[] sprites = new Sprite[5];
        private void Update()
        {
            timer += Time.deltaTime;
            if (timer > duration / 4 * 4)
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[4];
                Destroy(gameObject);
            }
            else if (timer > duration / 4 * 3)
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[3];
            }
            else if (timer > duration / 4 * 2)
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[2];
            }
            else if (timer > duration / 4 * 1)
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[1];
            }
        }
    }
}