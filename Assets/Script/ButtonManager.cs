using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class ButtonManager : MonoBehaviour
    {
        public bool pushButton = false;
        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.layer == 8)
            {
                pushButton = true;
            }
        }
        private void OnTriggerExit2D(Collider2D collider)
        {
            if (collider.gameObject.layer == 8)
            {
                pushButton = false;
            }
        }
    }
}