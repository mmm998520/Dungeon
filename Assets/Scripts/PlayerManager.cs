using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.BoardGameDungeon
{
    public class PlayerManager : MonoBehaviour
    {
        //角色素質之後能做成2維陣列儲存 不同職業(1維) 在 對應等級(2維) 時的素質
        //角色移動速度
        float moveSpeed = 3;

        void Update()
        {
            //對不同觸控點分別處裡
            for (int i = 0; i < Input.touchCount; i++)
            {
                //觸控點經過換算後在遊戲世界的位置
                Vector3 touchPos = Camera.main.ScreenToWorldPoint(Input.touches[i].position);
                touchPos.z = 0;
                //觸控點與角色的距離
                float targetDis = Vector3.Distance(touchPos, new Vector3(transform.position.x, transform.position.y, 0));
                //距離夠近才能進行操作
                if (targetDis < 0.5f)
                {
                    move(touchPos, targetDis);
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
                transform.position += Vector3.Normalize(touchPos - new Vector3(transform.position.x, transform.position.y, 0)) * moveDis;
            }
            //反之，直接瞬移到觸控點(因為移動距離夠)
            else
            {
                transform.position = new Vector3(touchPos.x, touchPos.y, transform.position.z);
            }
        }
    }
}