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
        PolygonCollider2D polygon;
        MeshRenderer meshRenderer;
        float timer;

        [SerializeField] Material[] materials;

        private void Start()
        {
            polygon = GetComponent<PolygonCollider2D>();
            meshRenderer = GetComponent<MeshRenderer>();
        }

        void Update()
        {
            if (InsPlayer && InsPlayer.GetComponent<PlayerManager>().DashTimer < 0.3f)
            {
                PosB.position = InsPlayer.position;
                Vector2[] poses = new Vector2[] { PosA.position, PosB.position, PosC.position };
                polygon.SetPath(0, poses);
                GetComponent<ColliderToMesh>().start();
            }
            else
            {
                InsPlayer = null;
            }
            setPos(LineA, PosA.position, PosB.position);
            setPos(LineB, PosB.position, PosC.position);
            setPos(LineC, PosA.position, PosC.position);
            //Hit(PosA.position, PosB.position);
            //Hit(PosB.position, PosC.position);
            //Hit(PosA.position, PosC.position);

            timer += Time.deltaTime;
            if (timer > 3)
            {
                GetComponent<Animator>().SetTrigger("end");
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

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.GetComponent<Bubble>() || (collider.GetComponent<MonsterShooter>() && collider.GetComponent<MonsterShooter>().canRemoveByPlayerAttack) || (collider.GetComponent<MonsterShooter_Bounce>() && collider.GetComponent<MonsterShooter_Bounce>().canRemoveByPlayerAttack))
            {
                Destroy(collider.gameObject);
            }
        }


        void Hit(Vector2 posA, Vector2 posB)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(posA, posB - posA, (posB - posA).magnitude);
            Collider2D collider;
            for (int i = 0; i < hits.Length; i++)
            {
                collider = hits[i].collider;
                attack(collider, 3 * Time.deltaTime);
                if (collider.GetComponent<Bubble>() || (collider.GetComponent<MonsterShooter>() && collider.GetComponent<MonsterShooter>().canRemoveByPlayerAttack) || (collider.GetComponent<MonsterShooter_Bounce>() && collider.GetComponent<MonsterShooter_Bounce>().canRemoveByPlayerAttack))
                {
                    Destroy(collider.gameObject);
                }
            }
        }

        void switchMaterial(int materialNum)
        {
            meshRenderer.material = materials[materialNum];
        }
    }
}