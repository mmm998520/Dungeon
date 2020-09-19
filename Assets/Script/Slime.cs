using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class Slime : MonsterManager
    {
        private void Awake()
        {
            rotateSpeed = 200;
        }
        void Update()
        {
            target = MinDisPlayer();
            changeDirection();
            transform.Translate(Vector3.right*Time.deltaTime);
        }
    }
}