using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueSet : MonoBehaviour
{
    /// <summary>
    /// 角色職業  刺客 -> 戰士 -> 法師 >>>> 角色種類總數
    /// </summary>
    public enum Career
    {
        Thief = 0,
        Warrior = 1,
        Magician = 2,
        /// <summary>
        /// 角色種類總數
        /// </summary>
        Count = 3
    }
    
    /// <summary>
    /// 角色素質用2維陣列儲存， 不同職業(1維) 在 對應等級(2維) 時的素質。
    /// 刺客 -> 戰士 -> 法師。
    /// 怪物等級為0。
    /// HP紀錄血量上限，傷害用累計的，超過上限 -> 死。
    /// </summary>
    public float[,] ATK,HP;
    public float Hurt = 0;
    //移動速度 "目前" 統一
    protected float moveSpeed = 10;

    //招式
    /// <summary>
    /// 攻擊招式，跟素質一樣可用陣列處理
    /// </summary>
    public GameObject[] Attack = new GameObject[3];
    /// <summary>
    /// 怪物攻擊招式，跟素質一樣可用陣列處理
    /// </summary>
    public GameObject[] MonsterAttack = new GameObject[3];
    /// <summary>
    /// 攻擊招式持續時間列表
    /// </summary>
    public float[] duration;
    /// <summary>
    /// 攻擊招式是否為持續傷害
    /// </summary>
    public bool[] continuous;
}
