using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class MagneticField : PlayerAttack
    {
        [HideInInspector] public Transform InsPlayer;
        public Transform LineA, LineB, LineC;
        public Transform PosA, PosB, PosC;
        public float MagneticFieldTimer;

        void Update()
        {
            MagneticFieldTimer += Time.deltaTime;
            if (InsPlayer && InsPlayer.GetComponent<PlayerManager>().DashTimer < 0.3f)
            {
                PosB.position = InsPlayer.position;
            }
            else
            {
                InsPlayer = null;
            }
            setPos(LineA, PosA.position, PosB.position);
            setPos(LineB, PosB.position, PosC.position);
            setPos(LineC, PosA.position, PosC.position);
            Hit(PosA.position, PosB.position);
            Hit(PosB.position, PosC.position);
            Hit(PosA.position, PosC.position);
            monsterManagers.Clear();
            if (MagneticFieldTimer >= 3)
            {
                Destroy(gameObject);
            }
        }

        void setPos(Transform line, Vector2 posA, Vector2 posB)
        {
            Vector2 center, dir;
            float distance, angle;

            center = (posA + posB) / 2;
            distance = Vector3.Distance(posA, posB);
            dir = (posA - posB).normalized;
            angle = Vector3.SignedAngle(Vector3.right, dir, Vector3.forward);

            line.position = center;
            line.rotation = Quaternion.Euler(0, 0, angle);
            line.localScale = new Vector3(distance, 1, 1);
        }

        void Hit(Vector2 posA, Vector2 posB)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(posA, posB - posA, (posB - posA).magnitude);
            Collider2D collider;
            for (int i = 0; i < hits.Length; i++)
            {
                collider = hits[i].collider;
                attack(collider, 3 * Time.deltaTime);
            }
        }
    }
}