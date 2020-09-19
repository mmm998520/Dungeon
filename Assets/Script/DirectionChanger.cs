﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class DirectionChanger : NavigationManager
    {
        float rotateSpeed = 100;
        public Transform target;

        protected void setNavigateTarget()
        {
            int[] a = new int[1] { (int)target.transform.position.x }, b = new int[1] { (int)target.transform.position.y };
            int pos = FindRoad((int)transform.position.x, (int)transform.position.y, a, b, orangePos);
            target = GameManager.maze.GetChild(pos);
            print(pos / MapCreater.totalCol + "," + pos % MapCreater.totalCol);
        }

        protected void changeDirection()
        {
            Vector3 myPos = transform.position;
            Vector3 endPos = target.position;
            // 讓z軸沒有前後誤差值，以免面向錯方向
            endPos.z = myPos.z;
            //計算將要朝向的方向，定義其為物件的x軸方向
            Vector3 vectorToTarget = endPos - myPos;
            //將物件的x軸延z軸旋轉90度尋找y軸
            Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, 90) * vectorToTarget;
            // LookRotation需要給予向前軸與向上軸，物件z軸將指向向前軸，y軸指向向上軸
            //所以只要讓物件z軸面向世界座標z軸，y軸指向面向方向轉90度即可讓物件指向x軸
            Quaternion targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: rotatedVectorToTarget);
            // 讓物件朝指定方向轉指定角度(rotateSpeed * Time.deltaTime讓他變定速旋轉)
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }
    }
}