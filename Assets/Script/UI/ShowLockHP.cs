using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace com.DungeonPad
{
    public class ShowLockHP : MonoBehaviour
    {
        [SerializeField] Image WhiteBakGround;
        [SerializeField] Color origialColor;
        [SerializeField] Image[] Life, MaxLife;

        void Update()
        {
            #region//死亡效果
            PlayerManager.DiedTimer += Time.deltaTime;
            if (PlayerManager.DiedTimer < 1)
            {
                PlayerManager playerManager;
                Rigidbody2D rigidbody;
                for (int i = 0; i < GameManager.players.childCount; i++)
                {
                    playerManager = GameManager.players.GetChild(i).GetComponent<PlayerManager>();
                    rigidbody = GameManager.players.GetChild(i).GetComponent<Rigidbody2D>();
                    rigidbody.velocity = Vector3.zero;
                    playerManager.DashTimer = 10;
                    playerManager.enabled = false;
                }
                WhiteBakGround.color = new Color(origialColor.r, origialColor.g, origialColor.b, Mathf.Pow(PlayerManager.DiedTimer, 2));
                return;
            }
            else if(PlayerManager.DiedTimer < 2)
            {
                if (GameManager.CurrentSceneName.Contains("Game 0"))
                {
                    PlayerManager.DiedTimer = 10;
                    ReGamer.ReAbility();
                    SwitchScenePanel.NextScene = "AfterGame 0";
                    TutorialAfterDiedWord.nextSceneName = GameManager.CurrentSceneName;
                    GameObject.Find("SwitchScenePanel").GetComponent<Animator>().SetTrigger("Loading");
                }
                else
                {
                    GameManager.PlayTime = Time.time;
                    PlayerManager.DiedTimer = 10;
                    SwitchScenePanel.NextScene = "Died";
                    GameObject.Find("SwitchScenePanel").GetComponent<Animator>().SetTrigger("Loading");
                }
                return;
            }
            #endregion

            #region//復活效果
            float lockHPLight = 1 - PlayerManager.lockedHPTimer;
            PlayerManager.lockedHPTimer += Time.deltaTime;

            if(lockHPLight > 0.2f)
            {
                lowSpeed();
                Time.timeScale = 0.17f;
                Time.fixedDeltaTime = 0.02F * Time.timeScale;
                GetComponent<Animator>().SetBool("Died", true);
            }
            else if (lockHPLight > 0.1f)
            {
                GameManager.players.GetChild(0).GetComponent<PlayerManager>().enabled = true;
                GameManager.players.GetChild(1).GetComponent<PlayerManager>().enabled = true;
                Time.timeScale = 1;
                Time.fixedDeltaTime = 0.02F * Time.timeScale;
                GetComponent<Animator>().SetBool("Died", false);
            }
            #endregion
            //float hurtLight = 0.25f - hurtTimer;
            //hurtTimer += Time.deltaTime;
            /*if (lockHPLight > 0)
            {
                WhiteBakGround.color = new Color(0, 0, 0, lockHPLight * 1.5f);
                for (int i = 0; i < Life.Length; i++)
                {
                    if (i < PlayerManager.Life - 1)
                    {
                        Life[i].color = new Color(origialColor.r, origialColor.g, origialColor.b, lockHPLight * 1.5f);
                    }
                    else if (i == PlayerManager.Life - 1)
                    {
                        Life[i].color = new Color(origialColor.r, origialColor.g, origialColor.b, Mathf.Pow(lockHPLight * 1.02f, 20));
                    }
                    else
                    {
                        Life[i].color = Color.clear;
                    }
                }
                for (int i = 0; i < MaxLife.Length; i++)
                {
                    MaxLife[i].color = new Color(origialColor.r, origialColor.g, origialColor.b, lockHPLight * 2);
                }
            }
            else
            {
                WhiteBakGround.color = Color.clear;
                for (int i = 0; i < Life.Length; i++)
                {
                    Life[i].color = Color.clear;
                }
                for (int i = 0; i < MaxLife.Length; i++)
                {
                    MaxLife[i].color = Color.clear;
                }
            }*/
        }

        void lowSpeed()
        {
            PlayerManager playerManager;
            Rigidbody2D rigidbody;
            for (int i = 0; i < GameManager.players.childCount; i++)
            {
                playerManager = GameManager.players.GetChild(i).GetComponent<PlayerManager>();
                rigidbody = GameManager.players.GetChild(i).GetComponent<Rigidbody2D>();
                if (rigidbody.velocity.magnitude > PlayerManager.moveSpeed)
                {
                    rigidbody.velocity = rigidbody.velocity.normalized * PlayerManager.moveSpeed;
                }
                playerManager.DashTimer = 10;
                playerManager.enabled = false;
            }
        }
    }
}