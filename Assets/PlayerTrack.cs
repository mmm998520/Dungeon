using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class PlayerTrack : PlayerAttack
    {
        public Transform Target;
        public float RotateSpeed;
        [SerializeField] float timer, timerStoper, speed;

        private void Update()
        {
            timer += Time.deltaTime;
            // 為更加逼真，0.2秒前只前進和減速不進行追蹤
            if (timer < 0.2f)
            {
                speed -= 1 * Time.deltaTime;
                transform.position += transform.right * speed * Time.deltaTime;
            }
            else
            {
                // 開始追蹤敵人
                Vector2 targetDir = (Target.position - transform.position).normalized;
                float a = Vector2.Angle(transform.right, targetDir) / RotateSpeed;
                speed += 0.5f * Time.deltaTime;

                if (a > 0.1f || a < -0.1f)
                {
                    transform.right = Vector2.Lerp(transform.right, targetDir, Time.deltaTime * RotateSpeed).normalized;
                }
                else
                {
                    transform.right = Vector2.Lerp(transform.right, targetDir, 1).normalized;
                }

                transform.position += transform.right * speed * Time.deltaTime;
            }
            if (timer > timerStoper)
            {
                // 超過生命週期爆炸（不同與擊中敵人）
                Destroy(gameObject);
            }
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if(attack(collider, 0.3f))
            {
                Destroy(gameObject);
            }
            if (collider.gameObject.layer == 12)
            {
                Destroy(gameObject);
            }
        }
    }
}