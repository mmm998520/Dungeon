using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.BoardGameDungeon
{
    public class Exit : MonoBehaviour
    {
        void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.GetComponent<PlayerManager>())
            {
                PlayerPrefs.SetFloat(collider.name + "Hurt", collider.GetComponent<PlayerManager>().Hurt);
                PlayerPrefs.SetInt(collider.name + "Level", collider.GetComponent<PlayerManager>().level);
                PlayerPrefs.SetFloat(collider.name + "Exp", collider.GetComponent<PlayerManager>().exp);
                Destroy(collider.gameObject);
            }
        }
    }
}