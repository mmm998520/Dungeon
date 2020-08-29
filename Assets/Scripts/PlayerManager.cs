using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace com.BoardGameDungeon
{
    public class PlayerManager : MonoBehaviour
    {
        public Text text;
        //角色素質之後能做成2維陣列儲存 不同職業(1維) 在 對應等級(2維) 時的素質
        //角色移動速度
        float moveSpeed = 5;

        //攻擊招式，跟素質一樣可用陣列處理
        public GameObject Attack;

        //紀錄點擊間隔用的計時器
        float TouchBeganTimer = 0;

        //攻擊模式開關
        bool attackMode = false;

        void Update()
        {
            TouchBeganTimer += Time.deltaTime;
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
                //給攻擊的前推多一點空間，但要超出基本操作範圍
                if(targetDis < 1 && targetDis>0.3f && attackMode)
                {
                    text.text += "c";
                    Debug.LogError("c");
                    attack(touchPos);
                    attackMode = false;
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
                transform.position += Vector3.Normalize(touchPos * Vector2.one  - transform.position * Vector2.one) * moveDis;
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
            Debug.LogError("a");
            text.text += "a";
            //如果接觸點是點擊瞬間則開始倒數計時 (看時間內有沒有其他點擊發生)
            if(phase == TouchPhase.Began)
            {
                Debug.LogError("b");
                text.text += "b";
                //與上次點擊的間隔夠短就能開啟攻擊模式 (快點兩下)
                if (TouchBeganTimer < 0.5f)
                {
                    attackMode = true;
                }
                //計時器歸零，開始計算間隔
                TouchBeganTimer = 0;
            }
        }

        void attack(Vector3 touchPos)
        {
            Debug.LogError("d");
            text.text += "d";
            //生成攻擊在觸控方向，並旋轉攻擊朝向該方向
            float angle = Vector3.SignedAngle(Vector3.right, touchPos * Vector2.one - transform.position * Vector2.one, Vector3.forward);
            Instantiate(Attack, transform.position + Vector3.Normalize(touchPos * Vector2.one - transform.position * Vector2.one) * 0.7f, Quaternion.Euler(0,0,angle));
        }
    }
}