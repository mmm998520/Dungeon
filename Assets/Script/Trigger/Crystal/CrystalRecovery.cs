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

        public override void hited()
        {
            if (crystalStat == CrystalStat.use)
            {
                return;
            }
            base.hited();

            Instantiate(crystalRecoveryLight, transform.position, Quaternion.identity, GameManager.triggers);
        }
    }
}