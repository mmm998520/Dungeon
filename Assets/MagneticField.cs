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

        void LateUpdate()
        {
            if (useMagneticField)
            {
                setPos(P1Before_P1After, p1.posBeforeDash, p1.posAfterDash);
                setPos(P2Before_P2After, p2.posBeforeDash, p2.posAfterDash);
                setPos(P1Before_P2Before, p1.posBeforeDash, p2.posBeforeDash);
                setPos(P1After_P2After, p1.posAfterDash, p2.posAfterDash);
                useMagneticField = false;
            }
            else
            {
                P1Before_P1After.gameObject.SetActive(false);
                P2Before_P2After.gameObject.SetActive(false);
                P1Before_P2Before.gameObject.SetActive(false);
                P1After_P2After.gameObject.SetActive(false);
            }
        }

        void setPos(Transform line, Vector2 posA, Vector2 posB)
        {
            line.gameObject.SetActive(true);
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
    }
}