using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class MagneticField : MonoBehaviour
    {
        public static bool useMagneticField;
        public PlayerManager p1, p2;
        public Transform P1Before_P1After, P2Before_P2After, P1Before_P2Before, P1After_P2After;
        public Transform PosA, PosB, PosC, PosD;
        HashSet<MonsterManager> monsterManagers = new HashSet<MonsterManager>();

        void LateUpdate()
        {
            P1Before_P1After.gameObject.SetActive(useMagneticField);
            P2Before_P2After.gameObject.SetActive(useMagneticField);
            P1Before_P2Before.gameObject.SetActive(useMagneticField);
            P1After_P2After.gameObject.SetActive(useMagneticField);
            PosA.gameObject.SetActive(useMagneticField);
            PosB.gameObject.SetActive(useMagneticField);
            PosC.gameObject.SetActive(useMagneticField);
            PosD.gameObject.SetActive(useMagneticField);
            if (useMagneticField)
            {
                setPos(P1Before_P1After, p1.posBeforeDash, p1.posAfterDash);
                setPos(P2Before_P2After, p2.posBeforeDash, p2.posAfterDash);
                setPos(P1Before_P2Before, p1.posBeforeDash, p2.posBeforeDash);
                setPos(P1After_P2After, p1.posAfterDash, p2.posAfterDash);
                PosA.position = p1.posBeforeDash;
                PosB.position = p1.posAfterDash;
                PosC.position = p2.posBeforeDash;
                PosD.position = p2.posAfterDash;
                Hit(p1.posBeforeDash, p1.posAfterDash);
                Hit(p2.posBeforeDash, p2.posAfterDash);
                Hit(p1.posBeforeDash, p2.posBeforeDash);
                Hit(p1.posAfterDash, p2.posAfterDash);
                monsterManagers.Clear();
                useMagneticField = false;
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
                if (collider.GetComponent<MonsterManager>() && !monsterManagers.Contains(collider.GetComponent<MonsterManager>()))
                {
                    monsterManagers.Add(collider.GetComponent<MonsterManager>());
                    if (!(collider.GetComponent<TaurenBoss>() && collider.GetComponent<TaurenBoss>().InvincibleTimer < 0.4f))
                    {
                        collider.GetComponent<MonsterManager>().HP -= 3 * Time.deltaTime;
                    }
                    print(collider.gameObject.name);
                    if (collider.GetComponent<MonsterManager>().HP <= 0)
                    {
                        collider.GetComponent<MonsterManager>().beforeDied();
                        Debug.LogWarning("hitTimes");
                    }
                }
                if (collider.GetComponent<BatSticked>() || collider.GetComponent<Bubble>() || (collider.GetComponent<MonsterShooter>() && collider.GetComponent<MonsterShooter>().canRemoveByPlayerAttack) || (collider.GetComponent<MonsterShooter_Bounce>() && collider.GetComponent<MonsterShooter_Bounce>().canRemoveByPlayerAttack))
                {
                    Destroy(collider.gameObject);
                }
                if (collider.name == "hit role collider")
                {
                    Destroy(collider.transform.parent.gameObject);
                }
            }
        }
    }
}