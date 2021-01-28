﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class CrystalScattering : Crystal
    {
        public GameObject crystalScatteringLight;
        float scatteringLightCount = 6;
        void Start()
        {

        }

        void Update()
        {

        }

        public override void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.GetComponent<PlayerAttackLineUnit>())
            {
                base.OnTriggerEnter2D(collider);
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
}