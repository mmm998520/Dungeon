using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class PlayerAttackLines : MonoBehaviour
    {
        public Transform p1, p2;
        Vector3 p1pos, p2pos, center, dir;
        float distance, angle;
        public Transform[] playerAttackLines;
        public Transform[] attackSprite;
        public Transform p1AttackLight, p2AttackLight;

        void Start()
        {

        }

        void Update()
        {
            setPos();
        }

        void setPos()
        {
            p1pos = p1.position;
            p2pos = p2.position;
            center = (p1pos + p2pos) / 2;
            distance = Vector3.Distance(p1pos, p2pos);
            dir = (p1pos - p2pos).normalized;
            angle = Vector3.SignedAngle(Vector3.right, dir, Vector3.forward);

            for (int i = 0; i < playerAttackLines.Length; i++)
            {
                playerAttackLines[i].position = center;
                playerAttackLines[i].rotation = Quaternion.Euler(0, 0, angle);
                playerAttackLines[i].localScale = new Vector3(distance, 1, 1);
            }
            p1AttackLight.position = p1pos;
            p1AttackLight.rotation = Quaternion.Euler(0, 0, angle + 180);
            p2AttackLight.position = p2pos;
            p2AttackLight.rotation = Quaternion.Euler(0, 0, angle);
            if(attackSprite[0].localScale.x > attackSprite[1].localScale.x)
            {
                p1AttackLight.localScale = Vector3.one * attackSprite[0].localScale.x;
                p2AttackLight.localScale = Vector3.one * attackSprite[0].localScale.x;
            }
            else
            {
                p1AttackLight.localScale = Vector3.one * attackSprite[1].localScale.x;
                p2AttackLight.localScale = Vector3.one * attackSprite[1].localScale.x;
            }
        }
    }
}