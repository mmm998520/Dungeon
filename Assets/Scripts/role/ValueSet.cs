using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueSet : MonoBehaviour
{
    /// <summary>
    /// <para>角色職業</para>
    /// <para>刺客 -> 戰士 -> 法師 >>>> 角色種類總數</para>
    /// </summary>
    public enum Career
    {
        /// <summary> 刺客 </summary>
        Thief = 0,
        /// <summary> 戰士 </summary>
        Warrior = 1,
        /// <summary> 法師 </summary>
        Magician = 2,
        /// <summary> 角色種類總數 </summary>
        Count = 3
    }

    /// <summary> 怪物種類 </summary>
    public enum MonsterType
    {
        /// <summary> 蜘蛛 </summary>
        spider = 0,
        /// <summary> 牛頭人 </summary>
        Tauren = 1,
        /// <summary> 史萊姆 </summary>
        Slime = 2,
        /// <summary> 蝙蝠 </summary>
        bat = 3,
        /// <summary> 狼 </summary>
        wolf = 4,
        /// <summary> 幽靈 </summary>
        ghost = 5,
        /// <summary> 骷髏騎士 </summary>
        SkeletonKnight = 6,
        /// <summary> 龍 </summary>
        Dragon = 7,
        /// <summary> 怪物種類總數 </summary>
        Count = 8
    }

    /// <summary>
    /// <para>角色素質用2維陣列儲存， 不同職業(1維) 在 對應等級(2維) 時的素質。</para> 
    /// <para>刺客 -> 戰士 -> 法師。</para> 
    /// <para>怪物等級為0。</para> 
    /// <para>HP紀錄血量上限，傷害用累計的，超過上限 -> 死。</para> 
    /// </summary>
    public float[,] ATK,HP;
    public float Hurt = 0;
    //移動速度 "目前" 統一
    protected float moveSpeed = 10;

    //招式
    /// <summary> 攻擊招式，跟素質一樣可用陣列處理 </summary>
    public GameObject[] Attack = new GameObject[3];
    /// <summary> 怪物攻擊招式，跟素質一樣可用陣列處理 </summary>
    public GameObject[] MonsterAttack = new GameObject[8];
    /// <summary> 攻擊招式持續時間列表 </summary>
    public float[] duration;
    /// <summary> 攻擊招式是否為持續傷害 </summary>
    public bool[] continuous;

    protected void died(int type,int level)
    {
        if (Hurt > HP[type,level])
        {
            Destroy(gameObject);
        }
    }
}
