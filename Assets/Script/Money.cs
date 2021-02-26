using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace com.DungeonPad
{
    public class Money : MonoBehaviour
    {
        public Transform child;
        public Collider2D GetCollider;
        public Animator animator;
        float timer = 0;
        public Transform sound;
        public bool moneyB;
        [HideInInspector]public bool final;
        [SerializeField]Transform spriteRenderer;
        void Start()
        {
            transform.eulerAngles = new Vector3(0, Random.Range(0, 2) * 180, Random.Range(-20f,0f));
            transform.position += new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f));
        }

        void Update()
        {
            if (spriteRenderer)
            {
                spriteRenderer.right = Vector3.right;
            }
            Transform minDisPlayer = MinDisPlayer();
            if (GetCollider.enabled == true)
            {
                animator.enabled = false;
            }
            float distance = Vector3.Distance(child.position, minDisPlayer.position);
            print(0);
            if (distance < 0.3f)
            {
                if (!moneyB)
                {
                    PlayerManager.money++;
                    PlayerManager.HP += 10;
                    minDisPlayer.GetComponent<PlayerManager>().reTimer = 0;
                }
                else
                {
                    PlayerManager.moneyB++;
                    int r = 0;
                    string abilityName = "";
                    int times = 0;
                    do
                    {
                        r = Random.Range(0, AbilityManager.Abilitys.Length);
                        abilityName = AbilityManager.Abilitys[r].name;
                        times++;
                        if (final && abilityName == "免疫")
                        {
                            abilityName = "重來一次";
                        }
                        if (GameManager.CurrentSceneName == "Game 0_3")
                        {
                            //HashSet<string> abilityNames = new HashSet<string>() { "守護", "不屈" };
                            if (abilityName != "突進")
                            {
                                abilityName = "重來一次";
                            }
                        }
                    } while ((abilityName == "重來一次" || AbilityManager.AbilityCurrentLevel[abilityName] >= AbilityManager.AbilityCanBuyLevel[abilityName]) && times < 1000);
                    if (times >= 1000)
                    {
                        Debug.LogError("能力都滿了");
                    }
                    try
                    {
                        AbilityData.setPlayerAbility(abilityName, ++AbilityManager.AbilityCurrentLevel[abilityName]);
                    }
                    catch
                    {
                        Debug.LogError("能力有問題");
                    }
                    try
                    {
                        GameManager.showAbilityDetail.showDetail(abilityName + " : " + AbilityManager.Abilitys[r].detail[AbilityManager.AbilityCurrentLevel[abilityName]]);
                    }
                    catch
                    {
                        Debug.LogError("找不到 : 能力敘述文字");
                    }
                }
                sound.SetParent(null);
                sound.GetComponent<AudioSource>().Play();
                Destroy(gameObject);
                Destroy(sound.gameObject,2);
                print(1);
            }
            else if ((distance < 3 && !moneyB) || (distance < 0.5f && moneyB))
            {
                child.position = child.position + Vector3.Normalize(MinDisPlayer().position - child.position) * 10 * Time.deltaTime;
                print(2);
            }
        }

        public Transform MinDisPlayer()
        {
            int i;
            float minDis = float.MaxValue;
            Transform minDisPlayer = null;
            for (i = 0; i < GameManager.players.childCount; i++)
            {
                Transform player = GameManager.players.GetChild(i);
                if (minDis > Vector3.Distance(player.position, child.position))
                {
                    minDis = Vector3.Distance(player.position, child.position);
                    minDisPlayer = player;
                }
            }
            return minDisPlayer;
        }
    }
}