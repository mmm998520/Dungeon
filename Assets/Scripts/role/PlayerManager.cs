using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace com.BoardGameDungeon
{
    public class PlayerManager : ValueSet
    {
        public Career career;
        /// <summary> 等級與當前經驗值，升級後經驗值不會歸零而是累加 </summary>
        public int level = 1, exp = 0;

        /// <summary> 紀錄點擊間隔用的計時器 </summary>
        float TouchBeganTimer = 0;
        /// <summary> 紀錄點擊瞬間的點，可用於計算雙擊後觸控點是否有發生拖動 </summary>
        Vector3 TouchBeganPos;
        /// <summary> 攻擊模式開關 </summary>
        bool attackMode = false;
        /// <summary> 攻擊開關開啟計時器，太久沒攻擊則關閉 </summary>
        float attackModeTimer = 0;
        void Start()
        {
            //角色素質用2維陣列儲存， 不同職業(1維) 在 對應等級(2維) 時的素質
            //刺客 -> 戰士 -> 法師
            ATK = new float[(int)Career.Count, 4] { { 0, 5, 7, 10 }, { 0, 2, 4, 6 }, { 0, 6, 8, 12 } };
            HP = new float[(int)Career.Count, 4] { { 0, 20, 30, 40 }, { 0, 40, 55, 70 }, { 0, 20, 30, 40 } };
            duration = new float[(int)Career.Count] { 0.4f, 0.4f, 2 };
            continuous = new bool[(int)Career.Count] { false, false, true };
        }

        void Update()
        {
            timer();

            //電腦測試用
            if (Input.touchCount == 0)
            {
                if (Input.anyKeyDown)
                {
                    GameObject attack = Instantiate(Attack[(int)career], transform.position, Quaternion.identity);
                    attack.GetComponent<AttackManager>().setValue(ATK[(int)career, level], duration[(int)career], continuous[(int)career], true);
                }
            }

            touchBehavior();
        }

        /// <summary> 統整觸控行為 </summary>
        void touchBehavior()
        {
            //對不同觸控點分別處裡
            for (int i = 0; i < Input.touchCount; i++)
            {
                //觸控點經過換算後在遊戲世界的位置
                Touch touch = Input.touches[i];
                Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
                touchPos.z = 0;
                //觸控點與角色的距離
                float targetDis = Vector3.Distance(touchPos, transform.position * Vector2.one);
                //距離夠近才能進行操作
                if (targetDis < 0.5f)
                {
                    move(touchPos, targetDis);
                    readyForAttack(touch.phase);
                }
                //偵測雙擊後的點與觸控點距離，
                if (Vector3.Distance(TouchBeganPos, touchPos) > 0.5f && targetDis < 1 && attackMode)
                {
                    attack(touchPos);
                }
            }
        }

        void move(Vector3 touchPos, float targetDis)
        {
            //換算後的移動距離
            float moveDis = Time.deltaTime * moveSpeed;
            //如果觸控點到角色的距離大於移動距離，則朝對應方向移動
            if (targetDis > moveDis)
            {
                transform.position += Vector3.Normalize(touchPos * Vector2.one - transform.position * Vector2.one) * moveDis;
            }
            //反之，直接瞬移到觸控點(因為移動距離夠)
            else
            {
                transform.position = new Vector3(touchPos.x, touchPos.y, transform.position.z);
            }
        }

        //預備攻擊，如果快點兩下就能進入攻擊模式
        void readyForAttack(TouchPhase phase)
        {
            //如果接觸點是點擊瞬間則開始倒數計時 (看時間內有沒有其他點擊發生)
            if (phase == TouchPhase.Began)
            {
                //與上次點擊的間隔夠短就能開啟攻擊模式 (快點兩下)
                if (TouchBeganTimer < 0.5f)
                {
                    attackMode = true;

                }
                //計時器歸零，開始計算間隔
                TouchBeganTimer = 0;
                //紀錄點擊瞬間的點
                TouchBeganPos = transform.position * Vector2.one;
            }
        }

        void attack(Vector3 touchPos)
        {
            attackMode = false;
            //生成攻擊在觸控方向，並旋轉攻擊朝向該方向
            float angle = Vector3.SignedAngle(Vector3.right, touchPos * Vector2.one - transform.position * Vector2.one, Vector3.forward);
            GameObject attack = Instantiate(Attack[(int)career], transform.position, Quaternion.Euler(0, 0, angle));
            //設定攻擊參數
            attack.GetComponent<AttackManager>().setValue(ATK[(int)career, level], duration[(int)career], continuous[(int)career], true);
        }

        /// <summary> 攻擊開關開啟計時器、點擊間隔計時器 </summary>
        void timer()
        {
            //攻擊開關開啟計時器
            if (attackMode)
            {
                attackModeTimer += Time.deltaTime;
            }
            else if (attackModeTimer != 0)
            {
                attackModeTimer = 0;
            }
            //點擊間隔計時器
            TouchBeganTimer += Time.deltaTime;
        }
    }
}