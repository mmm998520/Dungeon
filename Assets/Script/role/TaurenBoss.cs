using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace com.DungeonPad
{
    public class TaurenBoss : MonsterManager
    {
        public bool attacking = false;
        Transform minDisPlayer;
        public RectTransform HPBar;
        public Image hpBar;
        public Sprite YellowHP, RedHP;
        public float InvincibleTimer = 10;//無敵
        public Animator animator;
        public Vector3 RecordDir;

        public bool punching = false;
        bool throwByClockwise;
        float playerAngle;
        public GameObject Axe, AxeAll, accurateAxe;

        public float punchCD, punchCDTimer, throwAxe90CD, throwAxe90CDTimer, throwAxe180CD, throwAxe180CDTimer, accurateAxeCD, accurateAxeCDTimer;

        public float SleepTimer;
        public SpriteRenderer SleepUI;

        public GameObject punchSFX, punchHitSFX, throwAxeSFX, throwAxeAllSFX;

        [SerializeField] GameObject Go;

        string[] lastUseSkills = new string[2] { "", "" };
        void Start()
        {
            speed = 3;
            rigidbody = GetComponent<Rigidbody2D>();
            addCanGoByHand();
            endRow = new int[1];
            endCol = new int[1];
            animator = GetComponent<Animator>();
        }

        protected override void Update()
        {
            if (!canMove)
            {
                return;
            }
            base.Update();
            SleepTimer += Time.deltaTime;
            punchCDTimer += Time.deltaTime;
            throwAxe90CDTimer += Time.deltaTime;
            throwAxe180CDTimer += Time.deltaTime;
            accurateAxeCDTimer += Time.deltaTime;
            CDTimer += Time.deltaTime;
            InvincibleTimer += Time.deltaTime;
            float scale = HP / MaxHP;
            HPBar.localScale = new Vector3(scale, 1, 1);
            if (scale <= 0.5f)
            {
                hpBar.sprite = RedHP;
            }
            else
            {
                hpBar.sprite = YellowHP;
            }
            minDisPlayer = MinDisPlayer();

            animator.SetBool("Sleep", SleepTimer < 0f);
            SleepUI.enabled = SleepTimer < 0f;
            if (SleepTimer < 0f)
            {
                rigidbody.velocity = Vector3.zero;
            }
            else if ((Vector3.Distance(minDisPlayer.position, transform.position) > 5 && !attacking) || CDTimer < CD)
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("TaurenBossWalk"))
                {
                    resetRoad();
                    move();
                    print("statB");
                }
            }
            else if (!animator.GetBool("Punch") && !animator.GetBool("ThrowAxe90") && !animator.GetBool("ThrowAxe180") && !animator.GetBool("AccurateAxe"))
            {
                List<string> CDs = new List<string>() { "Punch", "ThrowAxe90", "AccurateAxe"};
                if (HP < MaxHP / 2)
                {
                    CDs.Add("ThrowAxe180");
                }
                int r;
                bool canUseThisAttack = false;
                do
                {
                    if (CDs.Count > 0)
                    {
                        if (MinDisPlayer().position.x - transform.position.x > 0)
                        {
                            transform.rotation = Quaternion.Euler(0, 0, 0);
                        }
                        else
                        {
                            transform.rotation = Quaternion.Euler(0, 180, 0);
                        }
                        r = Random.Range(0, CDs.Count);
                        if (!(lastUseSkills[0] == CDs[r] && lastUseSkills[1] == CDs[r]))
                        {
                            switch (CDs[r])
                            {
                                case "Punch":
                                    if (punchCDTimer > punchCD)
                                    {
                                        animator.SetBool("Punch", true);
                                        canUseThisAttack = true;
                                    }
                                    break;
                                case "ThrowAxe90":
                                    if (throwAxe90CDTimer > throwAxe90CD)
                                    {
                                        throwByClockwise = (Random.Range(0, 2) > 0);
                                        animator.SetBool("ThrowAxe90", true);
                                        playerAngle = Vector2.SignedAngle(Vector2.right, minDisPlayer.position - transform.position);
                                        canUseThisAttack = true;
                                    }
                                    break;
                                case "ThrowAxe180":
                                    if (throwAxe180CDTimer > throwAxe180CD)
                                    {
                                        throwByClockwise = (Random.Range(0, 2) > 0);
                                        animator.SetBool("ThrowAxe180", true);
                                        playerAngle = 0;
                                        canUseThisAttack = true;
                                    }
                                    break;
                                case "AccurateAxe":
                                    if (accurateAxeCDTimer > accurateAxeCD)
                                    {
                                        animator.SetBool("AccurateAxe", true);
                                        canUseThisAttack = true;
                                    }
                                    break;
                            }
                        }
                        if (!canUseThisAttack)
                        {
                            CDs.RemoveAt(r);
                            attacking = true;
                        }
                        else
                        {
                            if(CDs[r] != "AccurateAxe")
                            {
                                lastUseSkills[0] = lastUseSkills[1];
                                lastUseSkills[1] = CDs[r];
                            }
                            break;
                        }
                    }
                    else
                    {
                        Debug.LogError("沒有可用的攻擊");
                        resetRoad();
                        move();
                        break;
                    }
                } while (true);
            }
        }

        void reSpeed()
        {
            rigidbody.velocity = Vector3.zero;
            punching = false;
        }

        #region//Punch
        void Record()
        {
            RecordDir = Vector3.Normalize(minDisPlayer.position - transform.position);
            if (RecordDir.x > 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }

        void prePunch()
        {
            Record();
            rigidbody.velocity = Vector3.zero;
        }

        void punch()
        {
            InvincibleTimer = 0;
            rigidbody.velocity = RecordDir * 15;
            punching = true;
        }

        void PunchSFX()
        {
            Destroy(Instantiate(punchSFX, transform.position + Vector3.back * 10, Quaternion.identity), 3);
        }

        void endPunch()
        {
            animator.SetBool("Punch", false);
            Debug.LogError("Punch : " + animator.GetBool("Punch"));
            CDTimer = 0;
            punchCDTimer = 0;
        }
        #endregion



        #region//ThrowAxe
        void ThrowAxeSFX()
        {
            //Destroy(Instantiate(throwAxeSFX, transform.position + Vector3.back * 10, Quaternion.identity), 3);
        }

        void ThrowAxe(float angle)
        {
            if (!throwByClockwise)
            {
                angle *= -1;
            }
            GameObject temp = Instantiate(Axe, transform.position, Quaternion.Euler(0, 0, angle + playerAngle));
            Debug.Log(temp.transform.rotation.eulerAngles.z);
        }

        float axeNum;
        void CountThrowAxeCircleAxeNum(int _axeNum)
        {
            axeNum = _axeNum;
        }

        void ThrowAxeCircle(float angle)
        {
            if (!throwByClockwise)
            {
                angle *= -1;
            }
            float preAngle = 360f / axeNum;
            List<GameObject> axes = new List<GameObject>();
            for (int i = 0; i < axeNum; i++)
            {
                axes.Add(Instantiate(AxeAll, transform.position, Quaternion.Euler(0, 0, angle + preAngle * i)));
            }
            GameObject SFX = Instantiate(throwAxeAllSFX, CameraManager.center, Quaternion.identity);
            SFX.GetComponent<SFXManager>().gameObjects = axes;
            Destroy(SFX, 10);
        }

        void endThrowAxe90()
        {
            animator.SetBool("ThrowAxe90", false);
            CDTimer = 0;
            throwAxe90CDTimer = 0;
        }

        void endThrowAxe180()
        {
            animator.SetBool("ThrowAxe180", false);
            CDTimer = 0;
            throwAxe180CDTimer = 0;
        }
        #endregion

        #region//AccurateAxe

        public static T FindKeyByValue<T, W>(Dictionary<T, W> dict, W val)
        {
            T key = default;
            foreach (KeyValuePair<T, W> pair in dict)
            {
                if (EqualityComparer<W>.Default.Equals(pair.Value, val))
                {
                    key = pair.Key;
                    return key;
                }
            }
            Debug.LogError("not found key");
            return key;
        }

        void removeCanGo(int row, int col)
        {
            int value = row * MazeCreater.totalCol + col;
            int key = FindKeyByValue(canGo, value);
            canGo.Remove(key);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        void addSton()
        {
            
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        void AccurateAxe()
        {
            for (int i = 0; i < GameManager.players.childCount; i++)
            {
                Transform target = GameManager.players.GetChild(i);
                Instantiate(accurateAxe, target.position, Quaternion.identity).GetComponent<AccurateAxe>().target = target;
            }
        }

        void endAccurateAxe()
        {
            animator.SetBool("AccurateAxe", false);
            Debug.LogError("AccurateAxe : " + animator.GetBool("AccurateAxe"));
            CDTimer = 0;
            accurateAxeCDTimer = 0;
        }
        #endregion

        void resetRoad()
        {
            endRow[0] = Mathf.RoundToInt(minDisPlayer.position.x);
            endCol[0] = Mathf.RoundToInt(minDisPlayer.position.y);
            findRoad();
        }

        void move()
        {
            if (roads.Count>1)
            {
                Vector3 nextPos = new Vector3(roads[roads.Count - 2][0], roads[roads.Count - 2][1]);
                rigidbody.velocity = Vector3.Normalize(nextPos - transform.position) * speed;
                if (Vector3.Distance(transform.position, nextPos) < 0.5f)
                {
                    roads.RemoveAt(roads.Count - 1);
                }
            }
            else
            {
                rigidbody.velocity = Vector3.zero;
            }
            if (rigidbody.velocity.x > 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            if (rigidbody.velocity.x < 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }

        #region//Back
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (punching)
            {
                Destroy(Instantiate(punchHitSFX, transform.position + Vector3.back * 10, Quaternion.identity), 3);
                animator.SetBool("Punch", false);
                InvincibleTimer = 10;
                rigidbody.velocity = RecordDir * -5;
                animator.SetBool("Back", true);
                if (collision.gameObject.layer == 12)
                {
                    animator.SetBool("BackHurt", true);
                }
                else
                {
                    animator.SetBool("BackHurt", false);
                }
            }
        }

        public void endBack()
        {
            rigidbody.velocity = Vector3.zero;
            animator.SetBool("Back", false);
            punching = false;
        }

        public void endBackHurt()
        {
            animator.SetBool("BackHurt", false);
        }
        #endregion
        public GameObject[] DestoryWall;
        private void OnDestroy()
        {
            HPBar.localScale = Vector3.zero;
            if (!GameManager.DEMO)
            {
                for (int i = 0; i < DestoryWall.Length; i++)
                {
                    Destroy(DestoryWall[i]);
                }
                Go.SetActive(true);
            }
            else
            {
                Transform DragonBossSprite = transform.GetChild(0).GetChild(0);
                DragonBossSprite = Instantiate(DragonBossSprite.gameObject, DragonBossSprite.position, DragonBossSprite.rotation).transform;
                DragonBossSprite.GetComponent<DragonBossDied>().enabled = true;
                DragonBossSprite.localScale = transform.GetChild(0).GetChild(0).lossyScale;
            }
        }
    }
}