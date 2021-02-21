using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class BatSticked : MonoBehaviour
    {
        [SerializeField] float timer, timerStoper, SingleDamage;

        private void Start()
        {
            PlayerManager.batStickedNum++;
        }

        private void Update()
        {
            timer += Time.deltaTime;
            if (timer > timerStoper)
            {
                timer = 0;
                PlayerManager.HP -= SingleDamage * (100f - PlayerManager.reducesDamage) / 100f;
                Instantiate(GameManager.Hurted, transform.parent.position, Quaternion.identity, transform.parent);
            }
        }

        private void OnDestroy()
        {
            PlayerManager.batStickedNum--;
        }
    }
}