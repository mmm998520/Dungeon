using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace com.BoardGameDungeon
{
    public class PlayerManager : ValueSet
    {
        public Career career;
        /// <summary> 等級與當前經驗值，升級後經驗值歸零累加 </summary>
        public int level = 1;
        public float exp = 0;
        public static int[] expToNextLevel = new int[4] { 0, 30, 50,999999 };

        /// <summary> 紀錄點擊間隔用的計時器 </summary>
        float TouchBeganTimer = 0;
        /// <summary> 紀錄點擊瞬間的點，可用於計算雙擊後觸控點是否有發生拖動 </summary>
        Vector3 TouchBeganPos;
        /// <summary> 攻擊模式開關 </summary>
        bool attackMode = false;
        /// <summary> 攻擊開關開啟計時器，太久沒攻擊則關閉 </summary>
        float attackModeTimer = 0;
        /// <summary> 若玩家發動持續性技能，紀錄該技能是否生效 </summary>
        public bool statOne = false, statTwo = false;

        void Awake()
        {
            if(SceneManager.GetActiveScene().name != "Game")
            {
                Hurt = PlayerPrefs.GetFloat(name+ "Hurt");
                level = PlayerPrefs.GetInt(name+ "Level");
                exp = PlayerPrefs.GetFloat(name+ "Exp");
            }
        }
        void Start()
        {
            //角色素質用2維陣列儲存， 不同職業(1維) 在 對應等級(2維) 時的素質
            //刺客 -> 戰士 -> 法師
            ATK = new float[(int)Career.Count, 4] { { 0, 5, 7, 10 }, { 0, 2, 4, 6 }, { 0, 6, 8, 12 } };
            HP = new float[(int)Career.Count, 4] { { 0, 20, 30, 40 }, { 0, 40, 55, 70 }, { 0, 20, 30, 40 } };
            duration = new float[(int)Career.Count] { 0.4f, 0.4f, 2 };
            continuous = new bool[(int)Career.Count] { false, false, true };
            cd = 0;
        }

        void Update()
        {
            cdTimer += Time.deltaTime;
            timer();
            levelUp();
            died((int)career, level);

            touchBehavior();
            
            #region//電腦測試用
            if (Input.touchCount == 0)
            {
                if (Input.anyKeyDown)
                {
                    //向指定pos打出兩道射線(有間距)判定打到甚麼來決定能不能通過
                    Vector3 dir = GameManager.Players.GetChild(1).position * Vector2.one - transform.position * Vector2.one;
                    Vector3 tempDir = Quaternion.Euler(0, 0, 90) * dir.normalized / 2;
                    RaycastHit2D hit1 = Physics2D.Raycast(transform.position + tempDir, dir, dir.magnitude - 0.1f);
                    Debug.DrawRay(transform.position + tempDir, dir, Color.red, 2);
                    RaycastHit2D hit2 = Physics2D.Raycast(transform.position - tempDir, dir, dir.magnitude - 0.1f);
                    Debug.DrawRay(transform.position - tempDir, dir, Color.red, 2);
                    if (hit1 || hit2)
                    {
                        //print("!hit");
                    }
                    else
                    {
                        //print("short");
                    }
                    attack(Vector3.right);
                }
                transform.Translate(Input.GetAxis("Vertical") * Vector3.up * Time.deltaTime + Input.GetAxis("Horizontal") * Vector3.right * Time.deltaTime);
            }
            #endregion
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
                if (targetDis < 1.25f && cdTimer > cd)
                {
                    move(touchPos, targetDis);
                    readyForAttack(touch.phase);
                }
                //偵測雙擊後的點與觸控點距離，
                if (Vector3.Distance(TouchBeganPos, touchPos) > 0.2f && targetDis < 1.5f && attackMode)
                {
                    attack(touchPos);
                    cdTimer = 0;
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
            AttackManager attack = Instantiate(Attack[(int)career], transform.position, Quaternion.Euler(0, 0, angle)).GetComponent<AttackManager>();
            //設定攻擊參數
            attack.setValue(ATK[(int)career, level], duration[(int)career], continuous[(int)career], this);
            if(career == Career.Thief && statTwo)
            {
                attack.poison = true;
            }
        }

        /// <summary> 攻擊開關開啟計時器、點擊間隔計時器 </summary>
        void timer()
        {
            //攻擊開關開啟計時器
            if (attackMode)
            {
                transform.GetChild(0).gameObject.SetActive(true);
                attackModeTimer += Time.deltaTime;
            }
            else if (attackModeTimer != 0)
            {
                transform.GetChild(0).gameObject.SetActive(false);
                attackModeTimer = 0;
            }
            //點擊間隔計時器
            TouchBeganTimer += Time.deltaTime;
        }

        void levelUp()
        {
            if (exp > expToNextLevel[level])
            {
                exp -= expToNextLevel[level++];
            }
        }

        protected override void afterDied()
        {
            PlayerPrefs.SetFloat(name + "Hurt", Hurt);
            PlayerPrefs.SetInt(name + "Level", level);
            PlayerPrefs.SetFloat(name + "Exp", exp);
        }

        #region//技能
        /// <summary> 隱身 </summary>
        public void ThiefOne_Stealth()
        {
            statOne = true;
            //攻擊後結束，時間到結束
        }
        /// <summary> 塗毒 </summary>
        public void ThiefTwo_Poison()
        {
            statTwo = true;
            //攻擊後結束，時間到結束
        }

        /// <summary> 無敵 </summary>
        public void WarriorOne_Invincible()
        {
            statOne = true;
            //攻擊後結束，時間到結束
        }
        #endregion
    }
}