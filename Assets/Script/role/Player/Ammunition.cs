using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class Ammunition : MonoBehaviour
    {
        void Start()
        {

        }

        void Update()
        {

        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            PlayerManager playerManager = collider.GetComponent<PlayerManager>();
            if (playerManager && playerManager.BulletNum < playerManager.MaxBulletNum)
            {
                collider.GetComponent<PlayerManager>().BulletNum++;
                Destroy(gameObject);
            }
        }
    }
}