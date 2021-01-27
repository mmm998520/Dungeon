using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class BatSticked : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.GetComponent<PlayerAttackLineUnit>())
            {
                Destroy(gameObject);
            }
        }
    }
}