using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace com.DungeonPad
{
    public class DragonBoss : MonsterManager
    {
        public bool attacking = false;
        Transform minDisPlayer;
        public RectTransform HPBar;
        public Image hpBar;
        public Sprite YellowHP, RedHP;
        public float InvincibleTimer = 10;//無敵
        public Animator animator;
        public Vector3 RecordDir;

        public bool normalAttacking = false;
        bool throwByClockwise;
        float playerAngle;
        public GameObject fireBall, fireBallBounce, fireBallFast, fireBallTrack, fireRainInser, crystalInser;

        public float normalAttackCD, normalAttackCDTimer, fireBallCD, fireBallCDTimer, fireBallBounceCD, fireBallBounceCDTimer, fireBallFastCD, fireBallFastCDTimer, fireBallTrackCD, fireBallTrackCDTimer, fireRainCD, fireRainCDTimer;

        public float SleepTimer;
        public SpriteRenderer SleepUI;

        [SerializeField] GameObject FireSFX, normalAttackSFX;
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
            base.Update();
            SleepTimer += Time.deltaTime;
            normalAttackCDTimer += Time.deltaTime;
            fireBallCDTimer += Time.deltaTime;
            fireBallBounceCDTimer += Time.deltaTime;
            fireBallFastCDTimer += Time.deltaTime;
            fireBallTrackCDTimer += Time.deltaTime;
            fireRainCDTimer += Time.deltaTime;
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
            float dis = Vector3.Distance(minDisPlayer.position, transform.position);
            if (SleepTimer < 0f)
            {
                rigidbody.velocity = Vector3.zero;
            }
            else if ((dis > 5 && !attacking) || CDTimer < CD)
            {
                if (dis < 2.6f)
                {
                    if (normalAttackCDTimer > normalAttackCD && (!animator.GetBool("Normal Attack") && !animator.GetBool("FireBall") && !animator.GetBool("FireBallBounce") && !animator.GetBool("FireBallFast") && !animator.GetBool("FireBallTrack") && !animator.GetBool("FireRain")))
                    {
                        animator.SetBool("Normal Attack", true);
                    }
                }
                else if (animator.GetCurrentAnimatorStateInfo(0).IsName("DragonBossWalk"))
                {
                    resetRoad();
                    move();
                    print("statB");
                }
            }
            else if (!animator.GetBool("Normal Attack") && !animator.GetBool("FireBall") && !animator.GetBool("FireBallBounce") && !animator.GetBool("FireBallFast") && !animator.GetBool("FireBallTrack") && !animator.GetBool("FireRain"))
            {
                List<string> CDs = new List<string>() {"FireBallFast", "FireBallTrack", "FireRain" };
                int r;
                if (HP <= MaxHP)
                {
                    CDs.Add("FireBallBounce");
                }
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
                        switch (CDs[r])
                        {
                            case "FireBall":
                                if (fireBallCDTimer > fireBallCD)
                                {
                                    throwByClockwise = (Random.Range(0, 2) > 0);
                                    animator.SetBool("FireBall", true);
                                    playerAngle = Vector2.SignedAngle(Vector2.right, minDisPlayer.position - transform.position);
                                    canUseThisAttack = true;
                                }
                                break;
                            case "FireBallBounce":
                                if (fireBallBounceCDTimer > fireBallBounceCD)
                                {
                                    throwByClockwise = (Random.Range(0, 2) > 0);
                                    animator.SetBool("FireBallBounce", true);
                                    playerAngle = Vector2.SignedAngle(Vector2.right, minDisPlayer.position - transform.position);
                                    canUseThisAttack = true;
                                }
                                break;
                            case "FireBallFast":
                                if (fireBallFastCDTimer > fireBallFastCD)
                                {
                                    throwByClockwise = (Random.Range(0, 2) > 0);
                                    animator.SetBool("FireBallFast", true);
                                    playerAngle = Vector2.SignedAngle(Vector2.right, minDisPlayer.position - transform.position);
                                    canUseThisAttack = true;
                                }
                                break;
                            case "FireBallTrack":
                                if (fireBallTrackCDTimer > fireBallTrackCD)
                                {
                                    throwByClockwise = (Random.Range(0, 2) > 0);
                                    if (HP < 30)
                                    {
                                        animator.SetInteger("Stat", 30);
                                    }
                                    else if (HP < 60)
                                    {
                                        animator.SetInteger("Stat", 60);
                                    }
                                    else
                                    {
                                        animator.SetInteger("Stat", 90);
                                    }
                                    animator.SetBool("FireBallTrack", true);

                                    playerAngle = Vector2.SignedAngle(Vector2.right, minDisPlayer.position - transform.position);
                                    canUseThisAttack = true;
                                }
                                break;
                            case "FireRain":
                                if (fireRainCDTimer > fireRainCD)
                                {
                                    throwByClockwise = (Random.Range(0, 2) > 0);
                                    animator.SetBool("FireRain", true);
                                    playerAngle = Vector2.SignedAngle(Vector2.right, minDisPlayer.position - transform.position);
                                    canUseThisAttack = true;
                                }
                                break;
                        }
                        if (!canUseThisAttack)
                        {
                            CDs.RemoveAt(r);
                            attacking = true;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        if (dis < 2.6f && normalAttackCDTimer > normalAttackCD)
                        {
                            animator.SetBool("Normal Attack", true);
                        }
                        else
                        {
                            Debug.LogError("沒有可用的攻擊");
                            resetRoad();
                            move();
                        }
                        break;
                    }
                } while (true);
            }
        }

        void reSpeed()
        {
            rigidbody.velocity = Vector3.zero;
            normalAttacking = false;
        }

        #region//Normal Attack
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

        void preNormalAttack()
        {
            Record();
            rigidbody.velocity = Vector3.zero;
        }

        void NormalAttackSFX()
        {
            Destroy(Instantiate(normalAttackSFX, transform.position + Vector3.back * 10, Quaternion.identity), 5);
        }

        void normalAttack()
        {
            InvincibleTimer = 0;
            rigidbody.velocity = RecordDir * 15;
            normalAttacking = true;
        }

        void endNormalAttack()
        {
            animator.SetBool("Normal Attack", false);
            Debug.LogError("Normal Attack : " + animator.GetBool("Normal Attack"));
            normalAttackCDTimer = 0;
        }
        #endregion

        #region//FireBall
        void FireBallBounce(float positionOffset)
        {
            //Instantiate(fireBallBounce, transform.position + Quaternion.Euler(0, 0, playerAngle) * Vector3.up * positionOffset, Quaternion.Euler(0, 0, playerAngle));
            if (transform.eulerAngles.y <= 200 && transform.eulerAngles.y >= 160)
            {
                Instantiate(fireBallBounce, transform.position + new Vector3(-2, 0.6f, 0), Quaternion.Euler(0, 0, playerAngle + positionOffset));
            }
            else
            {
                Instantiate(fireBallBounce, transform.position + new Vector3(2, 0.6f, 0), Quaternion.Euler(0, 0, playerAngle + positionOffset));
            }
        }

        void fireSFX()
        {
            Destroy(Instantiate(FireSFX, transform.position + Vector3.back * 10, Quaternion.identity), 3);
        }

        void FireBall(float positionOffset)
        {
            //Instantiate(fireBall, transform.position + Quaternion.Euler(0, 0, playerAngle) * Vector3.up * positionOffset, Quaternion.Euler(0, 0, playerAngle)).GetComponent<MonsterTrack>().Target = GameManager.players.GetChild(Random.Range(0, 2));
            if (transform.eulerAngles.y <= 200 && transform.eulerAngles.y >= 160)
            {
                Instantiate(fireBall, transform.position + new Vector3(-2, 0.6f, 0), Quaternion.Euler(0, 0, playerAngle + positionOffset)).GetComponent<MonsterTrack>().Target = GameManager.players.GetChild(Random.Range(0, 2));
            }
            else
            {
                Instantiate(fireBall, transform.position + new Vector3(2, 0.6f, 0), Quaternion.Euler(0, 0, playerAngle + positionOffset)).GetComponent<MonsterTrack>().Target = GameManager.players.GetChild(Random.Range(0, 2));
            }
            Destroy(Instantiate(FireSFX, transform.position + Vector3.back * 10, Quaternion.identity), 3);
        }

        void FireBallFsat(float positionOffset)
        {
            //Instantiate(fireBallFast, transform.position + Quaternion.Euler(0, 0, playerAngle) * Vector3.up * positionOffset, Quaternion.Euler(0, 0, playerAngle)).GetComponent<MonsterTrack>().Target = GameManager.players.GetChild(Random.Range(0, 2));
            if (transform.eulerAngles.y <= 200 && transform.eulerAngles.y >= 160)
            {
                Instantiate(fireBallFast, transform.position + new Vector3(-2, 0.6f, 0), Quaternion.Euler(0, 0, playerAngle + positionOffset)).GetComponent<MonsterTrack>().Target = GameManager.players.GetChild(Random.Range(0, 2));
            }
            else
            {
                Instantiate(fireBallFast, transform.position + new Vector3(2, 0.6f, 0), Quaternion.Euler(0, 0, playerAngle + positionOffset)).GetComponent<MonsterTrack>().Target = GameManager.players.GetChild(Random.Range(0, 2));
            }
            Destroy(Instantiate(FireSFX, transform.position + Vector3.back * 10, Quaternion.identity), 3);
        }

        void FireBallTrack(float positionOffset)
        {
            //Instantiate(fireBallTrack, transform.position + Quaternion.Euler(0, 0, playerAngle) * Vector3.up * positionOffset, Quaternion.Euler(0, 0, playerAngle + Random.Range(-10, 10))).GetComponent<MonsterTrack>().Target = GameManager.players.GetChild(Random.Range(0,2));
            if (transform.eulerAngles.y <= 200 && transform.eulerAngles.y >= 160)
            {
                Instantiate(fireBallTrack, transform.position + new Vector3(-2, 0.6f, 0), Quaternion.Euler(0, 0, playerAngle + Random.Range(-45, 45) + positionOffset)).GetComponent<MonsterTrack>().Target = GameManager.players.GetChild(Random.Range(0, 2));
            }
            else
            {
                Instantiate(fireBallTrack, transform.position + new Vector3(2, 0.6f, 0), Quaternion.Euler(0, 0, playerAngle + Random.Range(-45, 45) + positionOffset)).GetComponent<MonsterTrack>().Target = GameManager.players.GetChild(Random.Range(0, 2));
            }
            Destroy(Instantiate(FireSFX, transform.position + Vector3.back * 10, Quaternion.identity), 3);
        }

        void endFireBall()
        {
            animator.SetBool("FireBall", false);
            CDTimer = 0;
            fireBallCDTimer = 0;
        }

        void endFireBallBounce()
        {
            animator.SetBool("FireBallBounce", false);
            CDTimer = 0;
            fireBallBounceCDTimer = 0;
        }

        void endFireBallFast()
        {
            animator.SetBool("FireBallFast", false);
            CDTimer = 0;
            fireBallFastCDTimer = 0;
        }

        void endFireBallTrack()
        {
            animator.SetBool("FireBallTrack", false);
            CDTimer = 0;
            fireBallTrackCDTimer = 0;
        }
        #endregion

        #region//FireRain
        void FireRain()
        {
            if (HP < 30)
            {
                Instantiate(fireRainInser).GetComponent<FireRainInser>().fireRainNum = 6;
            }
            else if (HP < 60)
            {
                Instantiate(fireRainInser).GetComponent<FireRainInser>().fireRainNum = 4;
            }
            else
            {
                Instantiate(fireRainInser).GetComponent<FireRainInser>().fireRainNum = 2;
            }
            Destroy(Instantiate(FireSFX, transform.position + Vector3.back * 10, Quaternion.identity), 3);
        }
        void CrystalRain()
        {
            Instantiate(crystalInser);
        }
        void endFireRain()
        {
            animator.SetBool("FireRain", false);
            CDTimer = 0;
            fireRainCDTimer = 0;
        }
        #endregion

        /*
        void resetRoad()
        {
            endRow = new int[1] { Mathf.RoundToInt(minDisPlayer.position.x) };
            endCol = new int[1] { Mathf.RoundToInt(minDisPlayer.position.y) };
            int pos = endRow[0] * MazeCreater.totalCol + endCol[0];
            if (CrystalSidePos.ContainsValue(pos))
            {
                int posX = Mathf.RoundToInt(minDisPlayer.position.x), posY = Mathf.RoundToInt(minDisPlayer.position.y);
                endRow = new int[5];
                endCol = new int[5];
                int t = 0;
                for(int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (i == 0 || j == 0)
                        {
                            endRow[t] = posX + i;
                            endCol[t] = posY + j;
                            t++;
                        }
                    }
                }
            }
            findRoad();
        }
        */

        void resetRoad()
        {
            endRow[0] = Mathf.RoundToInt(minDisPlayer.position.x);
            endCol[0] = Mathf.RoundToInt(minDisPlayer.position.y);
            int pos = endRow[0] * MazeCreater.totalCol + endCol[0];
            if (CrystalSidePos.ContainsValue(pos))
            {
                roads.Clear();
                roads.Add(new int[] { endRow[0], endCol[0] });
                roads.Add(new int[] { endRow[0], endCol[0] });
            }
            else
            {
                findRoad();
            }
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
            if (normalAttacking)
            {
                animator.SetBool("Normal Attack", false);
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
            normalAttacking = false;
        }

        public void endBackHurt()
        {
            animator.SetBool("BackHurt", false);
        }
        #endregion

        public GameObject DestoryWall;
        public GameObject NewWall;
        private void OnDestroy()
        {
            HPBar.localScale = Vector3.zero;
            SwitchScenePanel.NextScene = "Win";
            GameObject.Find("SwitchScenePanel").GetComponent<Animator>().SetTrigger("Loading");
            //DestoryWall.SetActive(false);
            //NewWall.SetActive(true);
        }
    }
}