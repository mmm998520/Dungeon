using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class AccurateAxe : MonoBehaviour
    {
        Animator animator;
        public Transform target;
        public float speed;
        public GameObject attack;
        public float attackLength;
        bool canFollow = true;

        [SerializeField] Transform rightSpider, leftSpider, upSpider, downSpider;
        void Start()
        {
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            if (canFollow)
            {
                follow();
            }
            rightSpider.position = new Vector3(21, rightSpider.position.y, 0);
            leftSpider.position = new Vector3(0, leftSpider.position.y, 0);
            upSpider.position = new Vector3(upSpider.position.x, 11.3f, 0);
            downSpider.position = new Vector3(downSpider.position.x, 0.3f, 0);
        }

        void follow()
        {
            Vector3 dir = target.position - transform.position;
            if (Vector3.Magnitude(dir) > Time.deltaTime * 1.5f)
            {
                transform.Translate(Vector3.Normalize(dir) * Time.deltaTime * speed);
            }
            else
            {
                transform.Translate(dir);
            }
        }

        void Attack()
        {
            canFollow = false;
            Instantiate(attack, transform.position + Vector3.left * attackLength, Quaternion.identity);
            Instantiate(attack, transform.position + Vector3.down * attackLength, Quaternion.Euler(0, 0, 90));
            Instantiate(attack, transform.position + Vector3.right * attackLength, Quaternion.Euler(0, 0, 180));
            Instantiate(attack, transform.position + Vector3.up * attackLength, Quaternion.Euler(0,0, 270));
        }

        void destroy()
        {
            Destroy(gameObject);
        }
    }
}
