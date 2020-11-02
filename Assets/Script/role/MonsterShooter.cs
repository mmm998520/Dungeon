using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class MonsterShooter : MonoBehaviour
    {
        public float speed, timer, timerStoper, destoryTime;
        public WaitForSeconds destoryTimer;
        public AudioSource hitSound;

        void Start()
        {
            destoryTimer = new WaitForSeconds(destoryTime);
        }
        void Update()
        {
            transform.Translate(Vector3.right * Time.deltaTime * speed);
            if ((timer += Time.deltaTime) > timerStoper)
            {
                speed = 0;
                Destroy(gameObject, destoryTime);
                hitSound.Play();
                hitSound.transform.parent = null;
                Destroy(hitSound.gameObject, 5);
            }
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.layer == 8 || collider.gameObject.layer == 12)
            {
                speed = 0;
                Destroy(gameObject, destoryTime);
                hitSound.Play();
                hitSound.transform.parent = null;
                Destroy(hitSound.gameObject, 5);
            }
        }
    }
}