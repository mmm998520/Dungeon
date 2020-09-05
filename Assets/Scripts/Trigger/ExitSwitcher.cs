using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.BoardGameDungeon
{
    public class ExitSwitcher : MonoBehaviour
    {
        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<PlayerManager>())
            {
                transform.GetChild(1).gameObject.SetActive(true);
                transform.GetChild(1).parent = null;
                Destroy(gameObject);
            }
        }
    }
}