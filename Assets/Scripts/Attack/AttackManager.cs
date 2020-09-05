using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.BoardGameDungeon
{
    public class AttackManager : MonoBehaviour
    {
        //攻擊力(總)
        public  float ATK;
        //持續時間
        public float duration;
        //紀錄該傷害是否為持續傷害
        public bool continuous = false;
        //紀錄該傷害是誰發出的避免誤傷、給予經驗值計算標準，若怪物發出則為null
        public PlayerManager user;

        //創建後使用該數值設定攻擊
        public void setValue(float _ATK, float _duration, bool _continuous, PlayerManager _user)
        {
            ATK = _ATK; duration = _duration; continuous = _continuous; user = _user;
        }

        protected void hurt(Collider2D collider, float ATK)
        {
            collider.GetComponent<ValueSet>().Hurt += ATK;
        }

        protected void OnTriggerEnter2D(Collider2D collider)
        {
            if (!continuous && ((collider.tag !="player" && user != null) || (collider.tag == "player" && user == null)))
            {
                hurt(collider, ATK);
            }
            MonsterManager.addHurtMe(collider, user);
        }

        protected void OnTriggerStay2D(Collider2D collider)
        {
            if (continuous && ((collider.tag != "player" && user != null) || (collider.tag == "player" && user == null)))
            {
                hurt(collider, Time.deltaTime * ATK / duration);
            }
        }
    }
}