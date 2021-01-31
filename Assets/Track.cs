using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace com.DungeonPad
{
    public class Track : MonsterShooter
    {
        public Transform Target;
        public float RotateSpeed;

        private void Update()
        {
            timer += Time.deltaTime;
            // 為更加逼真，0.5秒前只前進和減速不進行追蹤
            if (timer < 0.5f)
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
                    transform.right = Vector2.Lerp(transform.right, targetDir, Time.deltaTime / a).normalized;
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

        protected override void OnTriggerEnter2D(Collider2D collider)
        {
            base.OnTriggerEnter2D(collider);
        }
    }
}