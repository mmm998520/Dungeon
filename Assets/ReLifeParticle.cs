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
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.GetComponent<PlayerManager>())
            {
                if (PlayerManager.Life < PlayerManager.MaxLife)
                {
                    Destroy(gameObject);
                    PlayerManager.Life++;
                }
            }
        }
    }
}