using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class PlayerCircleAttack : PlayerAttack
    {
        [SerializeField] float growSpeed, destroyTime;
        void Start()
        {
            Destroy(gameObject, destroyTime);
            transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        }
        /*
        void Update()
        {
            transform.localScale += Vector3.one * Time.deltaTime * growSpeed;
        }
        */

        private void OnTriggerEnter2D(Collider2D collider)
        {
            attack(collider, 1);
        }
    }
}
