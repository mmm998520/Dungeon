using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.BoardGameDungeon
{
    public class QuarterAttackManager : AttackManager
    {
        float timer;
        public Sprite[] sprites = new Sprite[8];
        private void Update()
        {
            timer += Time.deltaTime;
            if (timer > duration / 7 * 7)
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[7];
                Destroy(gameObject);
            }
            else if (timer > duration / 7 * 6)
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[6];
            }
            else if (timer > duration / 7 * 5)
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[5];
            }
            else if (timer > duration / 7 * 4)
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[4];
            }
            else if (timer > duration / 7 * 3)
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[3];
            }
            else if (timer > duration / 7 * 2)
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[2];
            }
            else if (timer > duration / 8 * 1)
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[1];
            }
        }
    }
}