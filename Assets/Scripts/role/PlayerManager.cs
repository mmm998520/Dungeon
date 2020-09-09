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
        public static int[] expToNextLevel = new int[4] { 0, 30, 50, 999999 };

        /// <summary> 紀錄點擊間隔用的計時器 </summary>
        float TouchBeganTimer = 0;
        /// <summary> 紀錄點擊瞬間的點，可用於計算雙擊後觸控點是否有發生拖動 </summary>
        Vector3 TouchBeganPos;
        /// <summary> 攻擊模式開關 </summary>
        bool attackMode = false;
        /// <summary> 攻擊開關開啟計時器，太久沒攻擊則關閉 </summary>
        //float attackModeTimer = 0;
        /// <summary> 技能cd </summary>
        float skillOneCD = 0,skillOneCDTimer = 0, skillTwoCD = 0, skillTwoCDTimer = 0;
        /// <summary> 技能cd </summary>
        public float skillOneContinued = 0, skillOneContinuedTimer = 0, skillTwoContinued = 0, skillTwoContinuedTimer = 0;
        /// <summary> 若玩家發動持續性技能，紀錄該技能是否生效 </summary>
        public bool statOne = false, statTwo = false;
        /// <summary> 戰士專用技能bool，確保衝鋒中不會有其他操作 </summary>
        bool cahrge = false;
        /// <summary> 技能按鈕 </summary>
        public GameObject[] skillButton = new GameObject[2];

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
                if (Vector3.Distance(TouchBeganPos, touchPos) > 0.2f && targetDis < 1.5f)
                {
                    Vector3 dir = touchPos * Vector2.one - TouchBeganPos * Vector2.one;
                    
                    if(attackMode)
                    {
                        attack(dir);
                        cdTimer = 0;
                    }
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

        void attack(Vector3 dir)
        {
            attackMode = false;
            //生成攻擊在觸控方向，並旋轉攻擊朝向該方向
            float angle = Vector3.SignedAngle(Vector3.right, dir, Vector3.forward);
            AttackManager attack = Instantiate(Attack[(int)career], transform.position, Quaternion.Euler(0, 0, angle)).GetComponent<AttackManager>();
            //設定攻擊參數
            attack.setValue(ATK[(int)career, level], duration[(int)career], continuous[(int)career], this);
            if (career == Career.Thief)
            {
                /*if (statOne)
                {
                    statOne = false;
                    skillOneContinuedTimer = 100;
                }*/
                if (statTwo)
                {
                    attack.poison = true;
                    statTwo = false;
                    skillTwoContinuedTimer = 100;
                }
            }
        }

        /// <summary> 攻擊開關開啟計時器、點擊間隔計時器 </summary>
        void timer()
        {
            //攻擊開關開啟計時器
            transform.GetChild(0).gameObject.SetActive(attackMode);

            //點擊間隔計時器
            TouchBeganTimer += Time.deltaTime;

            //技能用
            if (level > 1)
            {
                skillButton[0].SetActive(true);
                if((skillOneContinuedTimer += Time.deltaTime) / skillOneContinued <= 1)
                {
                    skillButton[0].transform.GetChild(0).localScale = new Vector3(1, 1 - ((skillOneContinuedTimer += Time.deltaTime) / skillOneContinued), 1);
                }
                else
                {
                    skillButton[0].transform.GetChild(0).localScale = new Vector3(1, 0, 1);
                }
                if((skillOneCDTimer += Time.deltaTime) / skillOneCD <= 1)
                {
                    skillButton[0].transform.GetChild(1).localScale = new Vector3(1, (skillOneCDTimer += Time.deltaTime) / skillOneCD, 1);
                }
                else
                {
                    skillButton[0].transform.GetChild(1).localScale = new Vector3(1, 0, 1);
                }
                if (skillOneContinuedTimer > skillOneContinued)
                {
                    statOne = false;
                }
            }
            if (level > 2)
            {
                skillButton[1].SetActive(true);
                if((skillTwoContinuedTimer += Time.deltaTime) / skillTwoContinued <= 1)
                {
                    skillButton[1].transform.GetChild(0).localScale = new Vector3(1, 1 - ((skillTwoContinuedTimer += Time.deltaTime) / skillTwoContinued), 1);
                }
                else
                {
                    skillButton[1].transform.GetChild(0).localScale = new Vector3(1, 0, 1);
                }
                if((skillTwoCDTimer += Time.deltaTime) / skillTwoCD <= 1)
                {
                    skillButton[1].transform.GetChild(1).localScale = new Vector3(1, (skillTwoCDTimer += Time.deltaTime) / skillTwoCD, 1);
                }
                else
                {
                    skillButton[1].transform.GetChild(1).localScale = new Vector3(1, 0, 1);
                }

                if (skillTwoContinuedTimer > skillTwoContinued)
                {
                    statTwo = false;
                }
            }

            //出口倒計時
            if (exit)
            {
                if ((exitTimer += Time.deltaTime) > 3)
                {
                    PlayerPrefs.SetFloat(name + "Hurt", Hurt);
                    PlayerPrefs.SetInt(name + "Level", level);
                    PlayerPrefs.SetFloat(name + "Exp", exp);
                    if (GameManager.Players.childCount == 1)
                    {
                        SceneManager.LoadScene("Game2");
                    }
                    Destroy(gameObject);
                }
                else
                {
                    print(exitTimer);
                }
            }
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
        public void skill(int skillNum)
        {
            bool one = (skillNum == 1 && skillOneCDTimer > skillOneCD);
            bool two = (skillNum == 2 && skillTwoCDTimer > skillTwoCD);
            switch (career)
            {
                case Career.Thief:
                    if(one)
                    {
                        ThiefOne_Stealth();
                    }
                    else if (two)
                    {
                        ThiefTwo_Poison();
                    }
                    break;
                case Career.Warrior:
                    if (one)
                    {
                        WarriorOne_Invincible();
                    }
                    else if (two)
                    {
                        WarriorTwo_Charge();
                    }
                    break;
                case Career.Magician:
                    if (one)
                    {
                        MagicianOne_Recover();
                    }
                    else if (two)
                    {
                        MagicianTwo_Range();
                    }
                    break;
            }
        }

        /// <summary> 隱身 </summary>
        public void ThiefOne_Stealth()
        {
            statOne = true;
            //攻擊後結束，時間到結束
            skillOneCD = 15;
            skillOneCDTimer = 0;
            skillOneContinued = 4;
            skillOneContinuedTimer = 0;
        }
        /// <summary> 塗毒 </summary>
        public void ThiefTwo_Poison()
        {
            statTwo = true;
            //攻擊後結束，時間到結束
            skillTwoCD = 20;
            skillTwoCDTimer = 0;
            skillTwoContinued = 999999999;
            skillTwoContinuedTimer = 0;
        }

        /// <summary> 無敵 </summary>
        public void WarriorOne_Invincible()
        {
            statOne = true;
            //時間到結束
            skillOneCD = 25;
            skillOneCDTimer = 0;
            skillOneContinued = 4;
            skillOneContinuedTimer = 0;
        }

        /// <summary> 衝鋒 </summary>
        public void WarriorTwo_Charge()
        {
            StartCoroutine("WarriorChargeStat");
            //攻擊後結束，變成衝鋒狀態
            skillTwoCD = 20;
            skillTwoCDTimer = 0;
            skillTwoContinued = 3;
            skillTwoContinuedTimer = 0;
        }

        /// <summary> 恢復 </summary>
        public void MagicianOne_Recover()
        {
            Destroy(Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Attack/Recover"), transform.position, Quaternion.identity, transform), 5);
            skillOneCD = 30;
            skillOneCDTimer = 0;
        }

        /// <summary> 範圍攻擊 </summary>
        public void MagicianTwo_Range()
        {
            AttackManager attack = Instantiate(Resources.Load<GameObject>("Prefabs/Attack/Attack_Magician"), transform.position, Quaternion.identity).GetComponent<AttackManager>();
            Destroy(attack.gameObject, 3);
            //設定攻擊參數
            attack.setValue(18, 3, true, this);
            attack.transform.localScale *= 5;
            skillTwoCD = 60;
            skillTwoCDTimer = 0;
        }
        #endregion

        WaitForSeconds three = new WaitForSeconds(3);
        /// <summary> 戰士專用衝鋒狀態，會在2秒內持續向前 </summary>
        IEnumerator WarriorChargeStat()
        {
            cahrge = true;
            AttackManager attack = Instantiate(Resources.Load<GameObject>("Prefabs/Attack/Attack_Magician"), transform.position, Quaternion.identity, transform).GetComponent<AttackManager>();
            //設定攻擊參數
            Destroy(attack.gameObject, 3);
            attack.setValue(0, 3, false, this);
            //attack.transform.localScale *= 0.5f;
            cdTimer = 0;
            yield return three;
            cahrge = false;
        }

        float exitTimer = 0;
        bool exit = false;
        void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.GetComponent<Exit>())
            {
                exit = true;
            }
        }

        void OnTriggerExit2D(Collider2D collider)
        {
            if (collider.GetComponent<Exit>())
            {
                exitTimer = 0;
                exit = false;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (cahrge)
            {
                Transform Collision = collision.transform;
                if (Collision.tag == "monster")
                {
                    Collision.GetComponent<MonsterManager>().cahrged = (Collision.position * Vector2.one - transform.position * Vector2.one).normalized;
                    Collision.GetComponent<MonsterManager>().cahrgedSpeed = 4;
                }
            }
        }
    }
}