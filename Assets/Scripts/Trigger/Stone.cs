using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.BoardGameDungeon
{
    public class Stone : MonoBehaviour
    {
        public Vector3 dir;
        float speed = 3;
        float ATK = 5;

        void Update()
        {
            transform.Translate(dir*Time.deltaTime * speed);
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.GetComponent<ValueSet>())
            {
                collider.GetComponent<ValueSet>().Hurt += ATK;
            }
            else if(collider.tag == "wall")
            {
                Destroy(collider.gameObject);
            }
            else if(collider.tag == "side")
            {
                Destroy(gameObject);
            }
        }
    }
}
