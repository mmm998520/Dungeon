using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.DungeonPad
{
    public class ExitRoller : MonoBehaviour
    {
        float rotationZ;
        bool canUse = true;
        float useSpeed = 30, notUseSpeed = 120;
        public SpriteRenderer spriteRenderer;
        public Sprite non, used;

        void Update()
        {
            if(ExitToucher.P1touch && ExitToucher.P2touch && canUse)
            {
                spriteRenderer.sprite = used;
                rotationZ += Time.deltaTime * useSpeed;
            }
            else
            {
                spriteRenderer.sprite = non;
                rotationZ -= Time.deltaTime * notUseSpeed;
                canUse = false;
            }
            rotationZ = Mathf.Clamp(rotationZ, 0, 370);
            transform.rotation = Quaternion.Euler(0, 0, rotationZ);

            if (rotationZ>360)
            {
                SceneManager.LoadScene("Game 2");
            }
            else if (rotationZ <= 0)
            {
                canUse = true;
            }
        }
    }
}