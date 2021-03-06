﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class CrystalScattering : Crystal
    {
        public GameObject crystalScatteringLight;
        public static float scatteringLightCount = 6;
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

            int startAngle = Random.Range(0, 360);
            for (int i = 0; i < scatteringLightCount; i++)
            {
                float angle = startAngle + (360 / scatteringLightCount * i);
                while (angle >= 360)
                {
                    angle -= 360;
                }
                Instantiate(crystalScatteringLight, transform.position, Quaternion.Euler(0, 0, angle));
            }
        }
    }
}