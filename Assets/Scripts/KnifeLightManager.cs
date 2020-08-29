using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.BoardGameDungeon
{
    public class KnifeLightManager : AttackManager
    {
        float timer;
        public Sprite[] sprites = new Sprite[5];
        private void Update()
        {
            timer += Time.deltaTime;
            if (timer > 0.4f)
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[4];
                Destroy(gameObject);
            }
            else if (timer > 0.3f)
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[3];
            }
            else if (timer > 0.2f)
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[2];
            }
            else if (timer > 0.1f)
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprites[1];
            }
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (!continuous)
            {
                hurt(collider, ATK);
            }
        }

        void OnTriggerStay2D(Collider2D collider)
        {
            if (continuous)
            {
                hurt(collider, Time.deltaTime * ATK / duration);
            }
        }
    }
}