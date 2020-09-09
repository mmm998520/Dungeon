using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.BoardGameDungeon
{
    public class CircleAttackManager : AttackManager
    {
        float timer;
        public Sprite[] sprites = new Sprite[21];
        private void Update()
        {
            timer += Time.deltaTime;
            if (timer > duration / 20 * 20)
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[20];
            }
            else if (timer > duration / 20 * 19)
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[19];
            }
            else if (timer > duration / 20 * 18)
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[18];
            }
            else if (timer > duration / 20 * 17)
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[17];
            }
            else if (timer > duration / 20 * 16)
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[16];
            }
            else if (timer > duration / 20 * 15)
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[15];
            }
            else if (timer > duration / 20 * 14)
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[14];
            }
            else if (timer > duration / 20 * 13)
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[13];
            }
            else if (timer > duration / 20 * 12)
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[12];
            }
            else if (timer > duration / 20 * 11)
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[11];
            }
            else if (timer > duration / 20 * 10)
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[10];
            }
            else if (timer > duration / 20 * 9)
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[9];
            }
            else if (timer > duration / 20 * 8)
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[8];
            }
            else if (timer > duration / 20 * 7)
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[7];
            }
            else if (timer > duration / 20 * 6)
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[6];
            }
            else if (timer > duration / 20 * 5)
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[5];
            }
            else if (timer > duration / 20 * 4)
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[4];
            }
            else if (timer > duration / 20 * 3)
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[3];
            }
            else if (timer > duration / 20 * 2)
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[2];
            }
            else if (timer > duration / 20 * 1)
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[1];
            }
        }
    }
}