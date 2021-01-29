using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class CrystalRecovery : Crystal
    {
        public GameObject crystalRecoveryLight;
        protected override void Start()
        {
            base.Start();
        }

        void Update()
        {
        }

        protected override void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.GetComponent<PlayerAttackLineUnit>())
            {
                base.OnTriggerEnter2D(collider);

                Instantiate(crystalRecoveryLight, transform.position, Quaternion.identity, GameManager.triggers);
            }
        }
    }
}