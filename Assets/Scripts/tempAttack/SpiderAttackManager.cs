using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.BoardGameDungeon
{
    public class SpiderAttackManager : AttackManager
    {
        float timer;
        public Sprite[] sprites = new Sprite[14];

        void Start()
        {

        }

        void Update()
        {
            transform.Translate(Vector3.right * Time.deltaTime * 3);
            timer += Time.deltaTime;
            if (timer > duration / 13 * 13)
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[13];
                Destroy(gameObject);
            }
            else if (timer > duration / 13 * 12)
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[12];
            }
            else if (timer > duration / 13 * 11)
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[11];
            }
            else if (timer > duration / 13 * 10)
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[10];
            }
            else if (timer > duration / 13 * 9)
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[9];
            }
            else if (timer > duration / 13 * 8)
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[8];
            }
            else if (timer > duration / 13 * 7)
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[7];
            }
            else if (timer > duration / 13 * 6)
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[6];
            }
            else if (timer > duration / 13 * 5)
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[5];
            }
            else if (timer > duration / 13 * 4)
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[4];
            }
            else if (timer > duration / 13 * 3)
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[3];
            }
            else if (timer > duration / 13 * 2)
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[2];
            }
            else if (timer > duration / 13 * 1)
            {

                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[1];
            }
        }
    }
}