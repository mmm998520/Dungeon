using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /// <summary> 共通父物件，提供其他component使用 </summary>
    public static Transform Floors,Walls,Players;

    void Awake()
    {
        Floors = GameObject.Find("Floors").transform;
        Walls = GameObject.Find("Walls").transform;
        Players = GameObject.Find("Players").transform;
    }
}
