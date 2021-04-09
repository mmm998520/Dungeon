using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class CrystalScattering : Crystal
    {
        public GameObject crystalScatteringLight;
        public static float scatteringLightCount = 6;

        [SerializeField] GameObject CrystalHitSFX;
        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            base.Update();
        }

        public override void hited()
        {
            if (crystalStat == CrystalStat.use)
            {
                return;
            }
            base.hited();
            Destroy(Instantiate(CrystalHitSFX, transform.position + Vector3.back * 10, Quaternion.identity), 3);

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

            for (int i = -4; i <= 4; i++)
            {
                for (int j = -4; j <= 4; j++)
                {
                    CrystalRainInser.insPoses.Remove(((int)transform.position.x + i) * MazeCreater.totalCol + ((int)transform.position.y + j));
                }
            }
        }
    }
}