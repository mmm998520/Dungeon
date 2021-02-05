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
        public Image hpBar, hpBarBottom;
        public Sprite GreenHP, GreenHPBottom, YellowHP, YellowHPBottom, RedHP, RedHPBottom;
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
        void Start()
        {
            speed = 3;
            rigidbody = GetComponent<Rigidbody2D>();
            addCanGoByHand();
            endRow = new int[1];
            endCol = new int[1];
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
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
            if (scale < 0.3333f)
            {
                hpBar.sprite = RedHP;
                hpBarBottom.sprite = RedHPBottom;
            }
            else if (scale < 0.6666f)
            {
                hpBar.sprite = YellowHP;
                hpBarBottom.sprite = YellowHPBottom;
            }
            else
            {
                hpBar.sprite = GreenHP;
                hpBarBottom.sprite = GreenHPBottom;
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
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("DragonBossWalk"))
                {
                    resetRoad();
                    move();
                    print("statB");
                }
            }
            else if (!animator.GetBool("Normal Attack") && !animator.GetBool("FireBall") && !animator.GetBool("FireBallBounce") && !animator.GetBool("FireBallFast") && !animator.GetBool("FireBallTrack") && !animator.GetBool("FireRain"))
            {
                List<string> CDs = new List<string>() {"Normal Attack",/* "FireBall",*/ "FireBallFast", "FireBallTrack", "FireRain" };
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
                            case "Normal Attack":
                                if (normalAttackCDTimer > normalAttackCD)
                                {
                                    animator.SetBool("Normal Attack", true);
                                    canUseThisAttack = true;
                                }
                                break;
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
            CDTimer = 0;
            normalAttackCDTimer = 0;
        }
        #endregion

        #region//FireBall
        void FireBallBounce(float positionOffset)
        {
            Instantiate(fireBallBounce, transform.position + Quaternion.Euler(0, 0, playerAngle) * Vector3.up * positionOffset, Quaternion.Euler(0, 0, playerAngle));
        }

        void FireBall(float positionOffset)
        {
            Instantiate(fireBall, transform.position + Quaternion.Euler(0, 0, playerAngle) * Vector3.up * positionOffset, Quaternion.Euler(0, 0, playerAngle)).GetComponent<Track>().Target = GameManager.players.GetChild(Random.Range(0, 2));
        }

        void FireBallFsat(float positionOffset)
        {
            Instantiate(fireBallFast, transform.position + Quaternion.Euler(0, 0, playerAngle) * Vector3.up * positionOffset, Quaternion.Euler(0, 0, playerAngle)).GetComponent<Track>().Target = GameManager.players.GetChild(Random.Range(0, 2));
        }

        void FireBallTrack(float positionOffset)
        {
            Instantiate(fireBallTrack, transform.position + Quaternion.Euler(0, 0, playerAngle) * Vector3.up * positionOffset, Quaternion.Euler(0, 0, playerAngle + Random.Range(-10, 10))).GetComponent<Track>().Target = GameManager.players.GetChild(Random.Range(0,2));
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
            Instantiate(fireRainInser);
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
            DestoryWall.SetActive(false);
            NewWall.SetActive(true);
        }
    }
}