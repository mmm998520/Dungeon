using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.BoardGameDungeon
{
    public class Dragon : MonsterManager
    {
        float rotateSpeed = 0.7f;
        float TailFlickTimer = 0;
        public Transform[] side = new Transform[2];
        enum Stat
        {
            normal,
            ShockWave,
            SeaOfFireOccasion,
            SeaOfFire
        }
        Stat stat = Stat.normal;

        void Start()
        {
            monsterStart();

            cd = 3;
            moveSpeed = 0f;
            monsterType = MonsterType.Tauren;
        }

        void Update()
        {
            TailFlickTimer += Time.deltaTime;
            monsterUpdate();
            
        }

        #region//攻擊
        /// <summary> 
        /// 直線吐火 面對最近玩家發動 總時長2秒 
        /// <para> 直線攻擊(火抵達對面牆) </para>
        /// </summary>
        void Spitfire()
        {
            List<Transform> end = new List<Transform>();
            foreach (Transform player in GameManager.Players)
            {
                PlayerManager playerManager = player.GetComponent<PlayerManager>();
                if (!(playerManager.career == Career.Thief && playerManager.statOne))
                {
                    end.Add(player);
                }
            }
            navigateTarget = StraightLineNearest(end.ToArray());

            Transform target = navigateTarget.endTraget;
            Vector3 From = (transform.right * Vector2.one).normalized;
            Vector3 To = (target.position * Vector2.one - transform.position * Vector2.one).normalized;
            float angle = Vector3.SignedAngle(From, To, Vector3.forward);
            if (Mathf.Abs(angle) >= Time.deltaTime * rotateSpeed)
            {
                transform.Rotate(Vector3.forward * rotateSpeed * (angle / Mathf.Abs(angle)));
            }
            else
            {
                transform.Rotate(Vector3.forward * angle);
            }
            attackOccasion(navigateTarget, 9999);
        }

        protected override void attack()
        {
            StartCoroutine("attackWait");
        }

        WaitForSeconds AttackWait = new WaitForSeconds(1);
        IEnumerator attackWait()
        {
            transform.GetChild(2).gameObject.SetActive(true);
            yield return AttackWait;
            transform.GetChild(2).gameObject.SetActive(false);
            if (!paralysis)
            {
                //生成攻擊在觸控方向，並旋轉攻擊朝向該方向
                GameObject attack = Instantiate(MonsterAttack[(int)monsterType], transform.position, transform.rotation);
                //設定攻擊參數
                attack.GetComponent<AttackManager>().setValue(ATK[(int)monsterType, 0], duration[(int)monsterType], continuous[(int)monsterType], null);
            }
        }

        /// <summary>
        /// 半徑火海 震波結束後兩秒發動
        /// <para> 三/九點鐘方向直線吐火，保持，頭逆/順時針往九/三點鐘方向移動，最終龍前方半徑都是火(讓玩家順時或逆時針逃)，半徑火海形成後，最早噴火的地方火會最先開始消失</para>
        /// </summary>
        void SeaOfFireOccasion()
        {
            Transform[] temp = new Transform[1] { null };
            if (Vector3.Angle(Vector3.right, transform.right) < 90)
            {
                temp[0] = side[0];
            }
            else
            {
                temp[0] = side[1];
            }
            navigateTarget = StraightLineNearest(temp);

            Transform target = navigateTarget.endTraget;
            Vector3 From = (transform.right * Vector2.one).normalized;
            Vector3 To = (target.position * Vector2.one - transform.position * Vector2.one).normalized;
            float angle = Vector3.SignedAngle(From, To, Vector3.forward);
            if (Mathf.Abs(angle) >= Time.deltaTime * rotateSpeed)
            {
                transform.Rotate(Vector3.forward * rotateSpeed * (angle / Mathf.Abs(angle)));
            }
            else
            {
                transform.Rotate(Vector3.forward * angle);
                if (Vector3.Angle(Vector3.right, transform.right) < 90)
                {
                    temp[0] = side[1];
                }
                else
                {
                    temp[0] = side[0];
                }
                navigateTarget = StraightLineNearest(temp);

                int r = Random.Range(0, 2);
                if (r < 1)
                {
                    transform.Rotate(Vector3.forward * 0.0001f);
                }
                else
                {
                    transform.Rotate(Vector3.forward * -0.0001f);
                }
                stat = Stat.SeaOfFire;
            }
        }

        float SeaOfFireTimer = 0;
        /// <summary>
        /// 半徑火海 震波結束後兩秒發動
        /// <para> 三/九點鐘方向直線吐火，保持，頭逆/順時針往九/三點鐘方向移動，最終龍前方半徑都是火(讓玩家順時或逆時針逃)，半徑火海形成後，最早噴火的地方火會最先開始消失</para>
        /// </summary>
        void SeaOfFire()
        {
            Transform target = navigateTarget.endTraget;
            Vector3 From = (transform.right * Vector2.one).normalized;
            Vector3 To = (target.position * Vector2.one - transform.position * Vector2.one).normalized;
            float angle = Vector3.SignedAngle(From, To, Vector3.forward);

            if((SeaOfFireTimer+= Time.deltaTime) > 0.2f)
            {
                SeaOfFireTimer = 0;
                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //該攻擊具有不會同時被兩個相同攻擊傷到的特性
                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //生成攻擊在觸控方向，並旋轉攻擊朝向該方向
                GameObject attack = Instantiate(MonsterAttack[(int)monsterType], transform.position, transform.rotation);
                //設定攻擊參數
                attack.GetComponent<AttackManager>().setValue(ATK[(int)monsterType, 0], duration[(int)monsterType], continuous[(int)monsterType], null);
            }
            if (Mathf.Abs(angle) >= Time.deltaTime * rotateSpeed)
            {
                transform.Rotate(Vector3.forward * rotateSpeed * (angle / Mathf.Abs(angle)));
            }
            else
            {
                transform.Rotate(Vector3.forward * angle);
                stat = Stat.normal;
            }
        }

        /// <summary>
        /// 震波 每損30-40血 發動一次 
        /// <para> 以龍為中心擴散兩圈，每圈都會逼玩家遠離龍 </para>
        /// </summary>
        void ShockWave()
        {

        }

        /// <summary> 
        /// 甩尾  感應到後半圓有玩家時發動 間隔5秒
        /// <para> 範圍是龍後方半圓空間切兩刀，中間那一塊 </para>
        /// </summary>
        void TailFlick()
        {
            //生成攻擊在觸控方向，並旋轉攻擊朝向該方向
            GameObject attack = Instantiate(MonsterAttack[(int)monsterType], transform.position, transform.rotation);
            //設定攻擊參數
            attack.GetComponent<AttackManager>().setValue(ATK[(int)monsterType, 0], duration[(int)monsterType], continuous[(int)monsterType], null);
        }

        private void OnTriggerStay2D(Collider2D collider)
        {
            if (collider.GetComponent<PlayerManager>() && TailFlickTimer>5)
            {
                TailFlick();
                TailFlickTimer = 0;
            }
        }
        #endregion
    }
}