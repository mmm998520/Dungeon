using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class BatSticked : MonoBehaviour
    {
        [SerializeField] float timer, timerStoper, SingleDamage;
        [SerializeField] Transform SFX;
        private void Start()
        {
            PlayerManager.batStickedNum++;
        }

        private void Update()
        {
            if(transform.position.x - transform.parent.parent.position.x > 0)
            {
                transform.right = Vector3.left;
            }
            else
            {
                transform.right = Vector3.right;
            }
            timer += Time.deltaTime;
            if (timer > timerStoper)
            {
                timer = 0;
                if (PlayerManager.HP / PlayerManager.MaxHP <= 0.4f)
                {
                    PlayerManager.HP -= SingleDamage * (100f - PlayerManager.reducesDamage) / 100f;
                }
                else
                {
                    PlayerManager.HP -= SingleDamage;
                }
                try
                {
                    Camera.main.GetComponent<Animator>().SetTrigger("Hit");
                }
                catch
                {
                    Debug.LogError("這場景忘了放畫面抖動");
                }
                Instantiate(GameManager.Hurted, transform.parent.position, Quaternion.identity, transform.parent);
            }
        }
        private void LateUpdate()
        {
            SFX.transform.position = new Vector3(SFX.transform.position.x, SFX.transform.position.y, -10);
        }
        private void OnDestroy()
        {
            PlayerManager.batStickedNum--;
        }
    }
}