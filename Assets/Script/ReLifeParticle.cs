using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class ReLifeParticle : MonoBehaviour
    {
        public SpriteRenderer spriteRenderer;
        bool spriteColorAlphaUp = false;
        float spriteColorAMax = 1, spriteColorAMin = 0.5f;
        public CircleCollider2D circleCollider;
        public Transform child;
        void Start()
        {
            transform.eulerAngles = new Vector3(0, Random.Range(0, 2) * 180, Random.Range(-20f,0f));
            transform.position += new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f));
        }

        void Update()
        {
            transform.GetChild(0).right = Vector3.right;
            circleCollider.offset = child.localPosition;
            if (spriteColorAlphaUp)
            {
                spriteRenderer.color = new Color(1, 1, 1, Mathf.Lerp(spriteRenderer.color.a, spriteColorAMax, Time.deltaTime));
                if((spriteColorAMax - spriteRenderer.color.a) <= 0.05f)
                {
                    spriteColorAlphaUp = false;
                }
            }
            else
            {
                spriteRenderer.color = new Color(1, 1, 1, Mathf.Lerp(spriteRenderer.color.a, spriteColorAMin, Time.deltaTime));
                if ((spriteRenderer.color.a - spriteColorAMin) <= 0.05f)
                {
                    spriteColorAlphaUp = true;
                }
            }
            if (track)
            {
                Track();
            }
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.GetComponent<PlayerManager>())
            {
                if (PlayerManager.Life < PlayerManager.MaxLife)
                {
                    track = true;
                }
            }
        }

        bool track;
        float timer, speed = 5, RotateSpeed = 1;
        void Track()
        {
            Vector2 targetDir = Quaternion.Euler(0,0,Random.Range(0, 360)) * Vector3.right;
            timer += Time.deltaTime;
            if (timer < 0.3f)
            {
                speed -= 1 * Time.deltaTime;
                transform.position += transform.right * speed * Time.deltaTime;
            }
            else
            {
                targetDir = Camera.main.ScreenToWorldPoint(new Vector3(56, 1014, 10)) - transform.position;
                float a = Vector2.Angle(transform.right, targetDir) / RotateSpeed;
                speed += timer * timer * timer * 100f * Time.deltaTime;

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

            
            transform.localScale = Vector3.one * Mathf.Lerp(transform.localScale.x, 0, Time.deltaTime / 2);

            if (Camera.main.WorldToScreenPoint(transform.position).x < 56 && Camera.main.WorldToScreenPoint(transform.position).y > 1014)
            {
                Destroy(gameObject);
                PlayerManager.Life++;
            }
        }
    }
}